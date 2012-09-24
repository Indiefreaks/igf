using System;
using System.Collections.Generic;
using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// This class provides a structure to create Steering Behaviors that can make decisions based on the surrounding World
    /// </summary>
    public abstract class ContextualSteeringBehavior : SteeringBehavior
    {
        private readonly List<SceneEntity> _sceneEntitiesLookUp;
        private readonly Dictionary<Type, float> _consideredAgentTypes;
        private readonly Dictionary<int, float> _consideredAgentUniqueIds;
        private readonly Dictionary<SceneEntity,float> _context;
        private readonly List<Type> _ignoredAgentTypes;
        private readonly List<int> _ignoredAgentUniqueIds;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        protected ContextualSteeringBehavior()
        {
            _consideredAgentUniqueIds = new Dictionary<int, float>();
            _consideredAgentTypes = new Dictionary<Type, float>();
            _ignoredAgentUniqueIds = new List<int>();
            _ignoredAgentTypes = new List<Type>();

            _sceneEntitiesLookUp = new List<SceneEntity>();
            _context = new Dictionary<SceneEntity, float>();

            MaximumLookUps = -1;
            FilterDistance = 100f;
        }

        /// <summary>
        /// Returns the World Context
        /// </summary>
        public Dictionary<SceneEntity, float> Context
        {
            get { return _context; }
        }

        /// <summary>
        /// Gets or sets either this Steering Behavior should filter its context by distance or not
        /// </summary>
        public bool FilterByDistance { get; set; }

        /// <summary>
        /// Gets or sets the distance from other agents should this Steering Behavior consider to define its context
        /// </summary>
        public float FilterDistance { get; set; }

        /// <summary>
        /// Gets or sets the maximum lookups this Steering Behavior is autorized to perform before ending
        /// </summary>
        /// <remarks>Helper to let the developer optimize context computation for AI decisions to improve performances in case of really high number of agents in the world</remarks>
        public int MaximumLookUps { get; set; }

        /// <summary>
        /// Adds a unique agent to the list that should be considered to define the agent's context.
        /// </summary>
        /// <param name="agentUniqueId">The SceneEntity or SceneObject unique Id of the agent to be considered</param>
        /// <param name="weight">A value for the provided agent that will be used as a factor in the Steering Behaivior computation</param>
        /// <remarks>The higher the Wehight, the more inclined to this agent, the Steering Behavior will generate its force</remarks>
        public void Consider(int agentUniqueId, float weight)
        {
            if(AutonomousAgent.ParentObject.UniqueId == agentUniqueId)
                return;

            if (_ignoredAgentUniqueIds.Contains(agentUniqueId))
                _ignoredAgentUniqueIds.Remove(agentUniqueId);
            else
                _consideredAgentUniqueIds.Add(agentUniqueId, weight);
        }

        /// <summary>
        /// Adds a unique agent to the list that should be considered to define the agent's context.
        /// </summary>
        /// <param name="agentUniqueId">The SceneEntity or SceneObject unique Id of the agent to be considered</param>
        public void Consider(int agentUniqueId)
        {
            Consider(agentUniqueId, 1.0f);
        }
        
        /// <summary>
        /// Adds a agent type to the list that should be considered to define the agent's context.
        /// </summary>
        /// <param name="weight">A value for the provided Type of SceneEntity or SceneObject that will be used as a factor in the Steering Behaivior computation</param>
        /// <remarks>
        /// This is extremelly useful if you want to consider specific types of agents in your world. For instance, if you want to avoid Asteroids, you can set call this method using
        /// the Asteroid type as long as you created all your asteroids using this inherited SceneEntity or SceneObject class.
        /// 
        /// The higher the Wehight, the more inclined to this agent, the Steering Behavior will generate its force
        /// </remarks>
        public void Consider<T>(float weight) where T : SceneEntity
        {
            Type type = typeof (T);

            if (_ignoredAgentTypes.Contains(type))
                _ignoredAgentTypes.Remove(type);
            else
                _consideredAgentTypes.Add(type, weight);
        }

        /// <summary>
        /// Adds a agent type to the list that should be considered to define the agent's context.
        /// </summary>
        /// <remarks>
        /// This is extremelly useful if you want to consider specific types of agents in your world. For instance, if you want to avoid Asteroids, you can set call this method using
        /// the Asteroid type as long as you created all your asteroids using this inherited SceneEntity or SceneObject class.
        /// </remarks>
        public void Consider<T>() where T:SceneEntity
        {
            Consider<T>(1.0f);
        }

        /// <summary>
        /// Adds a unique agent to the list that should be ignored to define the agent's context
        /// </summary>
        /// <param name="agentUniqueId">The SceneEntity or SceneObject unique Id of the agent to be ignored</param>
        public void Ignore(int agentUniqueId)
        {
            if(AutonomousAgent.ParentObject.UniqueId == agentUniqueId)
                return;

            if (_consideredAgentUniqueIds.ContainsKey(agentUniqueId))
                _consideredAgentUniqueIds.Remove(agentUniqueId);
            else
                _ignoredAgentUniqueIds.Add(agentUniqueId);
        }

        /// <summary>
        /// Adds an agent type to the list that should be ignored to define the agent's context
        /// </summary>
        /// <remarks>
        /// This is extremelly useful if you want to ignore specific types of agents in your world. For instance, if you want to avoid everything but bonus items, you can call this method using
        /// the BonusItem type as long as you created all your bonus items using this inherited SceneEntity or SceneObject class.
        /// </remarks>
        public void Ignore<T>() where T : SceneEntity
        {
            Type type = typeof (T);

            if (_consideredAgentTypes.ContainsKey(type))
                _consideredAgentTypes.Remove(type);
            else
                _ignoredAgentTypes.Add(type);
        }

        /// <summary>
        /// Defines if the current steering behavior can execute or not.
        /// </summary>
        /// <returns>Returns true if it can, false otherwise</returns>
        /// <remarks>Override this method to add a global condition to this behavior</remarks>
        public override bool CanCompute()
        {
            if (base.CanCompute())
            {
                GetContext();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets the list of agents to be considered or ignored to define the agent's context
        /// </summary>
        public void ClearAgentFilters()
        {
            _consideredAgentTypes.Clear();
            _consideredAgentUniqueIds.Clear();
            _ignoredAgentTypes.Clear();
            _ignoredAgentUniqueIds.Clear();
        }

        /// <summary>
        /// Fills the list of SceneEntity and SceneObject instances that should be considered for this agent's context
        /// </summary>
        private void GetContext()
        {
            _sceneEntitiesLookUp.Clear();
            _context.Clear();

            if (FilterByDistance && FilterDistance > 0f)
            {
                var bounds = BoundingBox.CreateFromSphere(new BoundingSphere(AutonomousAgent.Position, FilterDistance));
                Application.SunBurn.ObjectManager.Find(_sceneEntitiesLookUp, bounds, ObjectFilter.All);
            }
            else
            {
                Application.SunBurn.ObjectManager.FindFast(_sceneEntitiesLookUp);
            }
            
            var lookUps = 0;

            if (_consideredAgentTypes.Count == 0 && _consideredAgentUniqueIds.Count == 0)
            {
                if (_ignoredAgentTypes.Count == 0 && _ignoredAgentUniqueIds.Count == 0)
                {
                    foreach (var sceneEntity in _sceneEntitiesLookUp)
                    {
                        if(sceneEntity != AutonomousAgent.ParentObject)
                            _context.Add(sceneEntity, 1.0f);
                    }
                }
                else
                {
                    foreach (var sceneEntity in _sceneEntitiesLookUp)
                    {
                        if (MaximumLookUps != -1)
                        {
                            if (lookUps >= MaximumLookUps)
                                break;

                            lookUps += 1;
                        }

                        if (sceneEntity != AutonomousAgent.ParentObject && !_ignoredAgentTypes.Contains(sceneEntity.GetType()) && !_ignoredAgentUniqueIds.Contains(sceneEntity.UniqueId))
                            _context.Add(sceneEntity, 1.0f);
                    }
                }
            }
            else
            {
                foreach (var sceneEntity in _sceneEntitiesLookUp)
                {
                    if (MaximumLookUps != -1)
                    {
                        if (lookUps >= MaximumLookUps)
                            break;

                        lookUps += 1;
                    }

                    if (sceneEntity == AutonomousAgent.ParentObject || _ignoredAgentTypes.Contains(sceneEntity.GetType()) || _ignoredAgentUniqueIds.Contains(sceneEntity.UniqueId))
                        continue;

                    if (_consideredAgentTypes.ContainsKey(sceneEntity.GetType()))
                        _context.Add(sceneEntity,_consideredAgentTypes[sceneEntity.GetType()]);
                    else if (_consideredAgentUniqueIds.ContainsKey(sceneEntity.UniqueId))
                        _context.Add(sceneEntity, _consideredAgentUniqueIds[sceneEntity.UniqueId]);
                }
            }
        }
    }
}