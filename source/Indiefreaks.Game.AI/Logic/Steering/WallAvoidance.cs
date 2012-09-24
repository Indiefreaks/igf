using System;

namespace Indiefreaks.Xna.Logic.Steering
{
    public class WallAvoidance : SteeringBehavior
    {
        public WallAvoidance()
        {
            Weight = 10.0f;
            Probability = 0.5f;
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