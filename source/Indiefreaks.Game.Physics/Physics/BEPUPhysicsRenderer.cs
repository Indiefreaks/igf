using System;
using BEPU.Drawer.Lines;
using BEPU.Drawer.Models;
using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Physics
{
    public class BEPUPhysicsRenderer : IRenderableManager, IUpdatableManager, IManagerService, ISubmit<BEPUCollisionMove>
    {
        private readonly IManagerServiceProvider _sceneInterface;

        public ModelDrawer ModelDrawer;
        public ContactDrawer ContactDrawer;
        public BoundingBoxDrawer BoundingBoxDrawer;
        public SimulationIslandDrawer SimulationIslandDrawer;
        public BasicEffect LineDrawer;
        public SpriteBatch UIDrawer;
        private ISceneState _sceneState;

        public BEPUPhysicsRenderer(IManagerServiceProvider sceneInterface)
        {
            _sceneInterface = sceneInterface;
            ManagerProcessOrder = 100;


            if (Application.Graphics.GraphicsDevice.GraphicsProfile == GraphicsProfile.HiDef)
                ModelDrawer = new InstancedModelDrawer(Application.Instance);
            else
                ModelDrawer = new BruteModelDrawer(Application.Instance);

            ContactDrawer = new ContactDrawer(Application.Instance);
            BoundingBoxDrawer = new BoundingBoxDrawer(Application.Instance);
            SimulationIslandDrawer = new SimulationIslandDrawer(Application.Instance);

            LineDrawer = new BasicEffect(Application.Graphics.GraphicsDevice);

            Visible = true;

        }
        #region Implementation of IUnloadable

        public void Unload()
        {
            ModelDrawer.Clear();
        }

        #endregion

        #region Implementation of IManager

        public void ApplyPreferences(ISystemPreferences preferences)
        {
        }

        public void Submit(BEPUCollisionMove bepuCollisionMove)
        {
            if (ModelDrawer.Contains(bepuCollisionMove.SpaceObject))
                return;

            ModelDrawer.Add(bepuCollisionMove.SpaceObject);
        }

        public void Move(BEPUCollisionMove bepuCollisionMove)
        {
        }

        public void Remove(BEPUCollisionMove bepuCollisionMove)
        {
            if(ModelDrawer.Contains(bepuCollisionMove.SpaceObject))
                ModelDrawer.Remove(bepuCollisionMove.SpaceObject);
        }

        public void Clear()
        {
        }

        public IManagerServiceProvider OwnerSceneInterface
        {
            get { return _sceneInterface; }
        }

        public void Update(GameTime gameTime)
        {
            ModelDrawer.Update();     
        }

        #endregion

        public bool Visible { get; set; }

        #region Implementation of IRenderableManager

        public void BeginFrameRendering(ISceneState scenestate)
        {
            _sceneState = scenestate;
        }

        public void EndFrameRendering()
        {
            var bepuPhysicsManager = _sceneInterface.GetManager<BEPUPhysicsManager>(false);
            if(bepuPhysicsManager == null)
                return;

            if(!Visible || _sceneState == null)
                return;
            
            ModelDrawer.Draw(_sceneState.View, _sceneState.Projection);

            LineDrawer.LightingEnabled = false;
            LineDrawer.VertexColorEnabled = true;
            LineDrawer.World = Matrix.Identity;
            LineDrawer.View = _sceneState.View;
            LineDrawer.Projection = _sceneState.Projection;

            ContactDrawer.Draw(LineDrawer, bepuPhysicsManager.Space);

            BoundingBoxDrawer.Draw(LineDrawer, bepuPhysicsManager.Space);

            SimulationIslandDrawer.Draw(LineDrawer, bepuPhysicsManager.Space);

            Application.Graphics.GraphicsDevice.SamplerStates.Reset();
        }

        #endregion

        #region Implementation of IManagerService

        public Type ManagerType
        {
            get { return typeof (BEPUPhysicsRenderer); }
        }

        public int ManagerProcessOrder { get; set; }

        #endregion
    }
}