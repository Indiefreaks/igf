using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Indiefreaks.Xna.Core
{
    /// <summary>
    /// Classes that implement this interface get automatic content loading/unloading management capabilities
    /// </summary>
    public interface IContentHost
    {
        /// <summary>
        ///   Load all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        /// <param name = "manager">XNA content manage</param>
        void LoadContent(IContentCatalogue catalogue, ContentManager manager);

        /// <summary>
        ///   Unload all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        void UnloadContent(IContentCatalogue catalogue);
    }

    /// <summary>
    /// Contract for classes that are responsible of loading/unloading content
    /// </summary>
    public interface IContentCatalogue : IDisposable
    {
        /// <summary>
        ///   Register an <see cref = "IContentHost" /> instance with this content manager
        /// </summary>
        /// <param name = "host"></param>
        void Add(IContentHost host);

        /// <summary>
        ///   Unregister an <see cref = "IContentHost" /> instance with this content manager. NOTE: Instances are stored by weak reference and do not need to be manually removed (see remarks)
        /// </summary>
        /// <remarks>
        ///   <para>Instances are stored by weak reference, so this method should only be called when removing the object early is desired.</para>
        ///   <para>Instances will not be kept alive when added, and do not need to be removed to make sure they are garbage collected</para>
        /// </remarks>
        /// <param name = "host"></param>
        void Remove(IContentHost host);
    }

    /// <summary>
    ///   The ContentCatalogue class encapsulates the Xna ContentManager class to Load content as WeakReferences avoiding therefore to manage Content memory.
    /// </summary>
    public sealed class ContentCatalogue : IContentCatalogue
    {
        private readonly Application _application;
        private readonly List<IContentHost> _delayedAddList = new List<IContentHost>();
        private readonly List<IContentHost> _delayedRemoveList = new List<IContentHost>();
        private readonly List<WeakReference> _highPriorityItems = new List<WeakReference>();
        private readonly Stack<WeakReference> _nullReferences = new Stack<WeakReference>();
        private readonly object _sync = new object();
        private List<WeakReference> _buffer;
        private bool _created;
        private List<WeakReference> _items = new List<WeakReference>();
        private ContentManager _manager;
        private IGraphicsDeviceService _service;

        /// <summary>
        ///   Construct a object content manager, creating an XNA content manager
        /// </summary>
        /// <param name = "application">Application instance</param>
        public ContentCatalogue(Application application)
            : this(application, application.Content)
        {
        }

        /// <summary>
        ///   Construct a object content manager, creating an XNA content manager
        /// </summary>
        /// <param name = "application">Application instance</param>
        /// <param name = "rootDirectory">Root content directory</param>
        public ContentCatalogue(Application application, string rootDirectory)
            : this(application, new ContentManager(application.Services, rootDirectory))
        {
        }

        /// <summary>
        ///   Construct a object content manager
        /// </summary>
        /// <param name = "application">Application instance</param>
        /// <param name = "manager">XNA ContentManager instatnce</param>
        public ContentCatalogue(Application application, ContentManager manager)
        {
            if (application == null || manager == null)
                throw new ArgumentNullException();

            _application = application;
            _service = (IGraphicsDeviceService)manager.ServiceProvider.GetService(typeof(IGraphicsDeviceService));

            if (_service == null)
                throw new ArgumentException("manager.Services.IGraphicsDeviceService not found");

            _created = _service.GraphicsDevice != null;

            //_service.DeviceDisposing += DeviceResetting;
            //_service.DeviceResetting += DeviceResetting;
            //_service.DeviceCreated += DeviceCreated;
            //_service.DeviceReset += DeviceReset;

            _manager = manager;
        }

        /// <summary>
        ///   Gets or sets the ContentManager root directory.
        /// </summary>
        public string RootDirectory
        {
            get { return _manager.RootDirectory; }
            set { _manager.RootDirectory = value; }
        }

        #region IContentCatalogue Members

        /// <summary>
        ///   Register an <see cref = "IContentHost" /> instance with this content manager
        /// </summary>
        /// <param name = "host"></param>
        public void Add(IContentHost host)
        {
            if (Monitor.TryEnter(_sync))
            {
                try
                {
                    if (_manager == null)
                        throw new ObjectDisposedException("this");
                    if (host == null)
                        throw new ArgumentNullException();
                    if (_nullReferences.Count > 0)
                    {
                        WeakReference wr = _nullReferences.Pop();
                        wr.Target = host;
                        _items.Add(wr);
                    }
                    else
                        _items.Add(new WeakReference(host));

                    if (_created)
                        host.LoadContent(this, _manager);
                }
                finally
                {
                    Monitor.Exit(_sync);
                }
            }
            else
            {
                lock (_delayedAddList)
                    _delayedAddList.Add(host);
            }
        }

        /// <summary>
        ///   Unregister an <see cref = "IContentHost" /> instance with this content manager. NOTE: Instances are stored by weak reference and do not need to be manually removed (see remarks)
        /// </summary>
        /// <remarks>
        ///   <para>Instances are stored by weak reference, so this method should only be called when removing the object early is desired.</para>
        ///   <para>Instances will not be kept alive when added, and do not need to be removed to make sure they are garbage collected</para>
        /// </remarks>
        /// <param name = "host"></param>
        public void Remove(IContentHost host)
        {
            if (Monitor.TryEnter(_sync))
            {
                try
                {
                    foreach (WeakReference wr in _items.Where(wr => wr.Target == host))
                    {
                        if (_items.Count > 1)
                        {
                            wr.Target = _items[_items.Count - 1].Target;
                            _items[_items.Count - 1].Target = null;
                        }
                        else
                            wr.Target = null;
                        break;
                    }
                    _nullReferences.Push(_items[_items.Count - 1]);
                    _items.RemoveAt(_items.Count - 1);
                }
                finally
                {
                    Monitor.Exit(_sync);
                }
            }
            else
            {
                lock (_delayedRemoveList)
                    _delayedRemoveList.Add(host);
            }
        }

        /// <summary>
        ///   Dispose the Content manager and unload all instances
        /// </summary>
        public void Dispose()
        {
            if (_items != null)
            {
                CallUnload();
                _items.Clear();
            }
            if (_service != null)
            {
                _service.DeviceDisposing -= DeviceResetting;
                _service.DeviceResetting -= DeviceResetting;
                _service.DeviceCreated -= DeviceCreated;
                _service.DeviceReset -= DeviceReset;
                _service = null;
            }
            if (_manager != null)
            {
                _manager.Dispose();
                _manager = null;
            }
            _buffer = null;
            _items = null;
        }

        #endregion

        private void DeviceResetting(object sender, EventArgs e)
        {
            CallUnload();
        }

        private void DeviceReset(object sender, EventArgs e)
        {
            CallLoad();
        }

        private void DeviceCreated(object sender, EventArgs e)
        {
            _created = true;
            CallLoad();
        }

        private void ProcessDelayed()
        {
            lock (_delayedAddList)
            {
                foreach (IContentHost owner in _delayedAddList)
                    Add(owner);
                _delayedAddList.Clear();
            }
            lock (_delayedRemoveList)
            {
                foreach (IContentHost owner in _delayedRemoveList)
                    Remove(owner);
                _delayedRemoveList.Clear();
            }
        }

        internal void AddHighPriority(IContentHost host)
        {
            lock (_sync)
            {
                if (_manager == null)
                    throw new ObjectDisposedException("this");
                if (host == null)
                    throw new ArgumentNullException();
                if (_nullReferences.Count > 0)
                {
                    WeakReference wr = _nullReferences.Pop();
                    wr.Target = host;
                    _highPriorityItems.Add(wr);
                }
                else
                    _highPriorityItems.Add(new WeakReference(host));

                if (_created)
                    host.LoadContent(this, _manager);

                ProcessDelayed();
            }
        }

        private void CallLoad()
        {
            lock (_sync)
            {
                if (_buffer == null)
                    _buffer = new List<WeakReference>();

                for (int i = 0; i < _highPriorityItems.Count; i++)
                {
                    var loader = _highPriorityItems[i].Target as IContentHost;
                    if (loader != null)
                        loader.LoadContent(this, _manager);
                    else
                    {
                        _nullReferences.Push(_highPriorityItems[i]);
                        _highPriorityItems.RemoveAt(i--);
                    }
                }

                foreach (WeakReference wr in _items)
                {
                    var loader = wr.Target as IContentHost;
                    if (loader != null)
                        _buffer.Add(wr);
                    else
                        _nullReferences.Push(wr);
                }
                foreach (IContentHost loader in _buffer.Select(wr => wr.Target).OfType<IContentHost>())
                {
                    loader.LoadContent(this, _manager);
                }

                List<WeakReference> list = _items;
                _items = _buffer;
                _buffer = list;

                _buffer.Clear();

                ProcessDelayed();
            }
        }

        private void CallUnload()
        {
            lock (_sync)
            {
                if (_buffer == null)
                    _buffer = new List<WeakReference>();

                for (int i = 0; i < _highPriorityItems.Count; i++)
                {
                    var loader = _highPriorityItems[i].Target as IContentHost;
                    if (loader != null)
                        loader.UnloadContent(this);
                    else
                    {
                        _nullReferences.Push(_highPriorityItems[i]);
                        _highPriorityItems.RemoveAt(i--);
                    }
                }

                foreach (WeakReference wr in _items)
                {
                    var loader = wr.Target as IContentHost;
                    if (loader != null)
                        _buffer.Add(wr);
                    else
                        _nullReferences.Push(wr);
                }
                foreach (IContentHost loader in _buffer.Select(wr => wr.Target).OfType<IContentHost>())
                {
                    loader.UnloadContent(this);
                }

                List<WeakReference> list = _items;
                _items = _buffer;
                _buffer = list;

                _buffer.Clear();

                ProcessDelayed();
            }
        }
    }
}