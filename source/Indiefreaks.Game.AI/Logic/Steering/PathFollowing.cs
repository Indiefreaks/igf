using System;

namespace Indiefreaks.Xna.Logic.Steering
{
    public class PathFollowing : SteeringBehavior
    {
        public PathFollowing()
        {
            Weight = 0.05f;
            Probability = 1.0f;
        }

        #region Overrides of SteeringBehavior
        
        /// <summary>
        /// Computes the current Steering Behavior
        /// </summary>
        /// <returns></returns>
        public override void Compute()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}