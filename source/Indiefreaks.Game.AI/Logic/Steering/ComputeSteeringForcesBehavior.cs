using System;
using System.Collections.Generic;
using System.Linq;
using Indiefreaks.Xna.Core;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Behavior that computes AutonomousAgent steering behaviors force
    /// </summary>
    public class ComputeSteeringForcesBehavior : Behavior
    {
        private readonly List<SteeringBehavior> _steeringBehaviors;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ComputeSteeringForcesBehavior()
        {
            _steeringBehaviors = new List<SteeringBehavior>();
            
            AddLocalCommand(ComputeSteeringBehaviors);
        }

        /// <summary>
        /// Returns the list of Steering Behaviors computed
        /// </summary>
        public IList<SteeringBehavior> Behaviors { get { return _steeringBehaviors; } }

        /// <summary>
        /// Returns if a given type of Steering Behavior is present
        /// </summary>
        /// <param name="steeringBehaviorType"></param>
        /// <returns></returns>
        public bool Contains(Type steeringBehaviorType)
        {
            return _steeringBehaviors.Aggregate(false, (current, steeringBehavior) => current | steeringBehavior.GetType() == steeringBehaviorType);
        }

        /// <summary>
        /// Returns if a given Steering Behavior instance is present
        /// </summary>
        /// <param name="steeringBehavior"></param>
        /// <returns></returns>
        public bool Contains(SteeringBehavior steeringBehavior)
        {
            return _steeringBehaviors.Contains(steeringBehavior);
        }

        /// <summary>
        /// Add a new Steering Behavior
        /// </summary>
        /// <param name="steeringBehavior"></param>
        public void Add(SteeringBehavior steeringBehavior)
        {
            Add(steeringBehavior, true);
        }

        /// <summary>
        /// Add a new Steering Behavior
        /// </summary>
        /// <param name="steeringBehavior"></param>
        /// <param name="unicity">Set to true if we want to have one steering behavior type only</param>
        public void Add(SteeringBehavior steeringBehavior, bool unicity)
        {
            if(unicity && Contains(steeringBehavior.GetType()))
                throw new CoreException(steeringBehavior.GetType().Name + " steering behavior type is already present");

            steeringBehavior.Priority = _steeringBehaviors.Count;
            steeringBehavior.AutonomousAgent = Agent as AutonomousAgent;

            _steeringBehaviors.Add(steeringBehavior);

            _steeringBehaviors.Sort(SortBySteeringBehaviorPriority);
        }
        
        /// <summary>
        /// Remove the provided Steering Behavior instance
        /// </summary>
        /// <param name="steeringBehavior"></param>
        public void Remove(SteeringBehavior steeringBehavior)
        {
            _steeringBehaviors.Remove(steeringBehavior);
        }

        /// <summary>
        /// Remove all Steering Behaviors
        /// </summary>
        public void RemoveAll()
        {
            _steeringBehaviors.Clear();   
        }

        private object ComputeSteeringBehaviors(Command command)
        {
            foreach (var steeringBehavior in _steeringBehaviors)
            {
                if(!steeringBehavior.Enabled)
                    continue;
                
                if(steeringBehavior.CanCompute())
                    steeringBehavior.Compute();
            }

            return null;
        }

        /// <summary>
        /// Sort all Steering Behaviors by their Priority property
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int SortBySteeringBehaviorPriority(SteeringBehavior a, SteeringBehavior b)
        {
            return Comparer<float>.Default.Compare(a.Priority, b.Priority);
        }
    }
}