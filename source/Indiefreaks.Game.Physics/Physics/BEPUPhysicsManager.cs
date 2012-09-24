using System;
using System.Collections.Generic;
using BEPUphysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Collision;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Editor;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Physics
{
    /// <summary>
    /// Manager wrapping BEPUPhysics features
    /// </summary>
    public class BEPUPhysicsManager : ICollisionManager, ISubmit<BEPUCollisionMove>
    {
        private readonly List<SceneEntity> _sceneEntities = new List<SceneEntity>();
        private readonly IManagerServiceProvider _sceneInterface;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public BEPUPhysicsManager(IManagerServiceProvider sceneInterface)
        {
            _sceneInterface = sceneInterface;
            ManagerProcessOrder = 10;
            Space = new Space();
            UseInternalTimeStepping = false;
        }

        /// <summary>
        /// Returns the current BEPUPhysics Space instance
        /// </summary>
        public Space Space { get; private set; }

        /// <summary>
        /// BEPUphysics supports internal timestepping. If you set this property to true, it is performing internal time stepping.  
        /// </summary>
        /// <remarks>
        /// This means it accumulates the time provided in the parameter and takes possibly multiple steps of length Space.TimeStepSettings.TimeStepDuration to get as close as possible to 0 accumulated time.  There is always some time left over, though.  For smooth movement when using internal time stepping, you may need to use the interpolation systems in BEPUphysics.  More information is available in the Updating Asynchronously documentation.  Note that you don't need to be updating asynchronously to make use of internal time stepping and interpolation- it's just a common use case.
        /// 
        /// If the "UseInternalTimeStepping" property is true, then Space.Update(dt) is called and the Space's interpolation buffers are enabled. 
        /// While the interpolation buffers are enabled, anything graphical referring to entity position and orientation should make use of the values in the buffer instead of the entity's direct properties.
        /// 
        /// You can get the states on a per-entity basis by using Entity.BufferedStates.InterpolatedStates. If the interpolation buffers are off, the properties will fall through to the usual Entity.Position and Entity.Orientation properties.
        /// 
        /// There are some complexities to consider though. Interpolated values are not 'true' values that reflect the current simulation state, but rather an arbitrary smoothing system for graphics. If a user wishes to create a system which relies on the true, uninterpolated states, they would need to go through the entity's direct properties.
        /// </remarks>
        public bool UseInternalTimeStepping { get; set; }

        #region Implementation of IUnloadable

        /// <summary>
        /// Disposes any graphics resource used internally by this object, and removes
        ///             scene resources managed by this object. Commonly used during Game.UnloadContent.
        /// </summary>
        public void Unload()
        {
            Clear();
        }

        #endregion

        #region Implementation of IManager

        /// <summary>
        /// Use to apply user quality and performance preferences to the resources managed by this object.
        /// </summary>
        /// <param name="preferences"/>
        public void ApplyPreferences(ISystemPreferences preferences)
        {
        }

        private List<BEPUCollisionMove> _bepuCollisionMoves = new List<BEPUCollisionMove>(); 

        public void Submit(BEPUCollisionMove bepuCollisionMove)
        {
            if (!_bepuCollisionMoves.Contains(bepuCollisionMove))
            {
                _bepuCollisionMoves.Add(bepuCollisionMove);
                Space.Add(bepuCollisionMove.SpaceObject);
            }
        }

        public void Move(BEPUCollisionMove bepuCollisionMove)
        {
        }

        public void Remove(BEPUCollisionMove bepuCollisionMove)
        {
            if (_bepuCollisionMoves.Contains(bepuCollisionMove))
            {
                _bepuCollisionMoves.Remove(bepuCollisionMove);
                Space.Remove(bepuCollisionMove.SpaceObject);

                var bepuPhysicsRenderer = OwnerSceneInterface.GetManager<BEPUPhysicsRenderer>(false);
                if(bepuPhysicsRenderer != null)
                    bepuPhysicsRenderer.Remove(bepuCollisionMove);
            }
        }

        /// <summary>
        /// Removes resources managed by this object. Commonly used while clearing the scene.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < Space.Entities.Count; i++)
                Space.Remove(Space.Entities[i]);
        }

        public IManagerServiceProvider OwnerSceneInterface
        {
            get { return _sceneInterface; }
        }

        /// <summary>
        /// Updates the object and its contained resources.
        /// </summary>
        /// <param name="gameTime"/>
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _bepuCollisionMoves.Count; i++)
            {
                _bepuCollisionMoves[i].Begin();
            }

                if (_sceneInterface.GetManager<SunBurnEditor>(false) != null && _sceneInterface.GetManager<SunBurnEditor>(true).EditorAttached)
                    return;

            if (UseInternalTimeStepping)
                Space.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
            else
                Space.Update();


            for (int i = 0; i < _bepuCollisionMoves.Count; i++)
            {
                _bepuCollisionMoves[i].End();
            }
        }

        #endregion

        #region Implementation of IManagerService

        /// <summary>
        /// Gets the manager specific Type used as a unique key for storing and
        ///             requesting the manager from the IManagerServiceProvider.
        /// </summary>
        public Type ManagerType
        {
            get { return typeof (BEPUPhysicsManager); }
        }

        /// <summary>
        /// Sets the order this manager is processed relative to other managers
        ///             in the IManagerServiceProvider. Managers with lower processing order
        ///             values are processed first.
        ///             In the case of BeginFrameRendering and EndFrameRendering, BeginFrameRendering
        ///             is processed in the normal order (lowest order value to highest), however
        ///             EndFrameRendering is processed in reverse order (highest to lowest) to ensure
        ///             the first manager begun is the last one ended (FILO).
        /// </summary>
        public int ManagerProcessOrder { get; set; }

        #endregion

        #region Implementation of IRenderableManager

        /// <summary>
        /// The current GraphicsDeviceManager used by this object.
        /// </summary>
        public IGraphicsDeviceService GraphicsDeviceManager
        {
            get { return SunBurnCoreSystem.Instance.GraphicsDeviceManager; }
        }

        /// <summary>
        /// Sets up the object prior to rendering.
        /// </summary>
        /// <param name="scenestate"/>
        public void BeginFrameRendering(ISceneState scenestate)
        {
            if (Space.ForceUpdater.Gravity.Y != -scenestate.Environment.Gravity)
                Space.ForceUpdater.Gravity = new Vector3(0, -scenestate.Environment.Gravity, 0);
        }

        /// <summary>
        /// Finalizes rendering.
        /// </summary>
        public void EndFrameRendering()
        {
        }

        #endregion

        private bool TestRay(ref Vector3 start, ref Vector3 end, out Ray ray, out float length)
        {
            var direction = new Vector3
                                {
                                    X = start.X - end.X,
                                    Y = start.Y - end.Y,
                                    Z = start.Z - end.Z
                                };

            length = direction.Length();

            if (length <= 0f)
            {
                ray = new Ray();
                return false;
            }

            direction.X /= length;
            direction.Y /= length;
            direction.Z /= length;

            ray = new Ray(end, direction);
            return true;
        }

        private void BuildRayRollisionPoint(ref RayCollisionPoint rayCollisionPoint, RayCastResult rayCastResult)
        {
            rayCollisionPoint.ContactTime = rayCastResult.HitData.T;
            rayCollisionPoint.ContactPoint = rayCastResult.HitData.Location;
            rayCollisionPoint.SurfaceNormal = rayCastResult.HitData.Normal;
            if (rayCollisionPoint.SurfaceNormal != Vector3.Zero)
            {
                rayCollisionPoint.SurfaceNormal.Normalize();
            }

            var collisionObject = rayCastResult.HitObject.Tag as ICollisionObject;
            if (collisionObject != null)
            {
                rayCollisionPoint.ContactObject = collisionObject;
                rayCollisionPoint.Material = collisionObject.DefaultCollisionMaterial;
            }
        }

        #region Implementation of ICollisionManager

        /// <summary>
        /// Casts a ray into the scene and returns the first intersected collidable object.
        /// </summary>
        /// <param name="startposition">World space start position of the ray.</param><param name="endposition">World space end position of the ray.</param><param name="firsthit">Output intersection information.</param>
        /// <returns>
        /// Returns true if an intersection occurs.
        /// </returns>
        public bool RayCast(Vector3 startposition, Vector3 endposition, out RayCollisionPoint firsthit)
        {
            Ray ray;
            float length;

            if (!TestRay(ref startposition, ref endposition, out ray, out length))
            {
                firsthit = new RayCollisionPoint();
                return false;
            }

            return RayCast(ray, length, out firsthit);
        }

        /// <summary>
        /// Casts a ray into the scene and returns all intersected collidable object.
        /// </summary>
        /// <param name="hits">Resulting intersection information.</param><param name="startposition">World space start position of the ray.</param><param name="endposition">World space end position of the ray.</param>
        public void RayCast(List<RayCollisionPoint> hits, Vector3 startposition, Vector3 endposition)
        {
            Ray ray;
            float length;

            if (TestRay(ref startposition, ref endposition, out ray, out length))
            {
                RayCast(hits, ray, length);
            }
        }

        /// <summary>
        /// Casts a ray into the scene and returns the first intersected collidable object.
        /// </summary>
        /// <param name="ray">Normalized world space ray.</param><param name="castdistance">Distance to cast ray.</param><param name="firsthit">Output intersection information.</param>
        /// <returns>
        /// Returns true if an intersection occurs.
        /// </returns>
        public bool RayCast(Ray ray, float castdistance, out RayCollisionPoint firsthit)
        {
            RayCastResult result;
            if (!Space.RayCast(ray, castdistance, out result))
            {
                firsthit = new RayCollisionPoint();
                return false;
            }

            firsthit = new RayCollisionPoint();
            BuildRayRollisionPoint(ref firsthit, result);
            return true;
        }

        /// <summary>
        /// Casts a ray into the scene and returns all intersected collidable object.
        /// </summary>
        /// <param name="hits">Resulting intersection information.</param><param name="ray">Normalized world space ray.</param><param name="castdistance">Distance to cast ray.</param>
        public void RayCast(List<RayCollisionPoint> hits, Ray ray, float castdistance)
        {
            var results = new List<RayCastResult>();

            if (Space.RayCast(ray, castdistance, results))
            {
                foreach (var result in results)
                {
                    var collisionPoint = new RayCollisionPoint();
                    BuildRayRollisionPoint(ref collisionPoint, result);
                    hits.Add(collisionPoint);
                }
            }
        }

        #endregion
    }
}