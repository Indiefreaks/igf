using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Rendering.Instancing
{
    /// <summary>
    ///   The InstanceFactory is responsible of creating and maintaining InstanceEntity instances
    /// </summary>
    public class InstanceFactory : IDisposable
    {
        private readonly List<InstanceEntity> _entities = new List<InstanceEntity>();
        private readonly List<short> _indices = new List<short>(256);
        private readonly List<SceneObject> _sceneObjects = new List<SceneObject>(1);
        private readonly IInstanceSource _source;

        private readonly List<VertexPositionNormalTextureBumpSkin> _vertices =
            new List<VertexPositionNormalTextureBumpSkin>(256);

        private GraphicsDevice _graphicsDevice;
        private IndexBuffer _indexBuffer;
        private int _instancesCount;
        private Effect _shader;
        private VertexBuffer _vertexBuffer;

        /// <summary>
        ///   Creates a new instance of the class
        /// </summary>
        /// <param name = "graphicsDevice">The GraphicsDevice instance</param>
        /// <param name = "source">The mesh data information used to create instances</param>
        /// <param name = "shader">The shader shared accross all instances</param>
        /// <remarks>
        ///   The provided shader should be either LightingEffect or DeferredObjectEffect and set is Skinned property to true
        /// </remarks>
        protected internal InstanceFactory(GraphicsDevice graphicsDevice, IInstanceSource source, Effect shader)
        {
            _graphicsDevice = graphicsDevice;
            _source = source;
            _shader = shader;
        }

        /// <summary>
        ///   Returns the number of instances created
        /// </summary>
        public int InstancesCount
        {
            get { return _instancesCount; }
        }

        /// <summary>
        ///   Create one unique instance of the mesh
        /// </summary>
        /// <param name = "name">The name of the instance</param>
        /// <param name = "transform">The Matrix containing the instance World position/rotation/scale</param>
        /// <returns>Returns the created InstanceEntity so the developer can change its World matrix</returns>
        public InstanceEntity CreateInstance(string name, Matrix transform)
        {
            var instanceIndex = _instancesCount%75;

            if (_instancesCount < 75)
            {
                AddGeometry(instanceIndex);
                BuildGeometry();
            }

            var instanceEntity = new InstanceEntity(name, instanceIndex, GetSceneObject(), transform);

            _entities.Add(instanceEntity);
            _instancesCount++;

            if(_instancesCount >= 75)
                ClearIntermediateData();

            return instanceEntity;
        }

        /// <summary>
        ///   Creates an array of instances of the mesh
        /// </summary>
        /// <param name = "names">The names array for all the instances to be created</param>
        /// <param name = "transforms">The World matrix array for all the instances to be created</param>
        /// <returns>Returns the array of InstanceEntity created</returns>
        public InstanceEntity[] CreateInstances(string[] names, Matrix[] transforms)
        {
            var instances = new InstanceEntity[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                instances[i] = CreateInstance(names[i], transforms[i]);
            }

            return instances;
        }

        /// <summary>
        ///   Add the required Geometry for hardware instancing
        /// </summary>
        /// <param name = "instancetransformindex">the index used in the SceneObejct.SkinBones array to position the instance</param>
        /// <returns>Returns true if succeeed; false otherwise.</returns>
        private bool AddGeometry(int instancetransformindex)
        {
            // Get the number of vertices already in the container object to readjust the new instance's destination indices.
            int indexoffset = _vertices.Count;

            // Avoid overflowing the indices.
            int finalmaxindex = indexoffset + _source.Vertices.Length;
            if (finalmaxindex > short.MaxValue)
                return false;

            // Copy all vertices into the container object.
            foreach (VertexPositionNormalTextureBump vert in _source.Vertices)
            {
                VertexPositionNormalTextureBumpSkin dest = new VertexPositionNormalTextureBumpSkin();

                // Copy source data.
                dest.Position = vert.Position;
                dest.TextureCoordinate = vert.TextureCoordinate;
                dest.Normal = vert.Normal;
                dest.Tangent = vert.Tangent;
                dest.Binormal = vert.Binormal;

                // Add instance information that tells SunBurn which transform to use in the instance transform array.
                dest.BoneIndex0 = (byte) instancetransformindex;
                // Required for instancing.
                dest.BoneWeights = Vector4.UnitX;

                _vertices.Add(dest);
            }

            // Copy all indices into the container object, adjusting for existing instances.
            foreach (short index in _source.Indices)
                _indices.Add((short) (index + indexoffset));

            return true;
        }

        /// <summary>
        ///   Builds the Geometry into the required VertexBuffer and IndexBuffer
        /// </summary>
        private void BuildGeometry()
        {
            ClearGraphicsResources(false);

            // Create and fill the buffers.
            _vertexBuffer = new VertexBuffer(_graphicsDevice, typeof (VertexPositionNormalTextureBumpSkin),
                                             _vertices.Count, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(_vertices.ToArray());

            // Create and fill the buffers.
            _indexBuffer = new IndexBuffer(_graphicsDevice, typeof (short), _indices.Count, BufferUsage.WriteOnly);
            _indexBuffer.SetData(_indices.ToArray());

            //ClearIntermediateData();
        }

        /// <summary>
        ///   Retrieves the best SceneObject to add a new instance
        /// </summary>
        /// <returns></returns>
        private SceneObject GetSceneObject()
        {
            if (_sceneObjects.Count == 0)
                return CreateNewSceneObject();
            else
            {
                var sceneObject = _sceneObjects[_sceneObjects.Count - 1];
                if (_instancesCount%75 != 0)
                {
                    var boundingSphere = new BoundingSphere(Vector3.Zero, float.MaxValue);
                    var boundingBox = BoundingBox.CreateFromSphere(boundingSphere);

                    var primitiveCount = _source.Indices.Length/3*(_instancesCount%75 + 1);
                    var vertexRange = _source.Vertices.Length*(_instancesCount%75 + 1);

                    sceneObject.RenderableMeshes[0].Build(sceneObject, _shader, Matrix.Identity, boundingSphere,
                                                          boundingBox, _indexBuffer,
                                                          _vertexBuffer, 0, PrimitiveType.TriangleList,
                                                          primitiveCount, 0, vertexRange,
                                                          0, true);
                    return sceneObject;
                }
                else
                    return CreateNewSceneObject();
            }
        }

        /// <summary>
        ///   Creates a new SceneObject to host up to 75 instances
        /// </summary>
        /// <returns></returns>
        private SceneObject CreateNewSceneObject()
        {
            var boundingSphere = new BoundingSphere(Vector3.Zero, float.MaxValue);
            var boundingBox = BoundingBox.CreateFromSphere(boundingSphere);

            var primitiveCount = _source.Indices.Length/3*((_instancesCount%75) + 1);
            var vertexRange = _source.Vertices.Length*((_instancesCount%75) + 1);

            var sceneObject = new SceneObject(_shader, boundingSphere, boundingBox, Matrix.Identity, _indexBuffer,
                                              _vertexBuffer, 0, PrimitiveType.TriangleList, primitiveCount, 0,
                                              vertexRange,
                                              0)
                                  {
                                      UpdateType = UpdateType.None,
                                      Visibility = ObjectVisibility.RenderedAndCastShadows,
                                      SkinBones = new Matrix[75]
                                  };
            for (int i = 0; i < sceneObject.SkinBones.Length; i++)
            {
                sceneObject.SkinBones[i] = Matrix.Identity;
            }

            _sceneObjects.Add(sceneObject);
            SceneInterface.ActiveSceneInterface.ObjectManager.Submit(sceneObject);

            return sceneObject;
        }

        /// <summary>
        ///   Clear the VertexBuffer and IndexBuffer
        /// </summary>
        private void ClearGraphicsResources(bool totally)
        {
            if (_vertexBuffer != null)
            {
                if (totally)
                    _vertexBuffer.Dispose();
                _vertexBuffer = null;
            }

            if (_indexBuffer != null)
            {
                if (totally)
                    _indexBuffer.Dispose();
                _indexBuffer = null;
            }
        }

        /// <summary>
        ///   Clear the IntermediateData used to build geometry
        /// </summary>
        private void ClearIntermediateData()
        {
            _vertices.Clear();
            _indices.Clear();
        }

        #region Implementation of IDisposable

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            _graphicsDevice = null;
            _shader = null;

            _entities.Clear();
            _sceneObjects.Clear();

            ClearGraphicsResources(true);
            ClearIntermediateData();
        }

        #endregion
    }
}