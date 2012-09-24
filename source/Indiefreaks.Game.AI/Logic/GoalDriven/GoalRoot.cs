using System;

namespace Indiefreaks.Xna.Logic.GoalDriven
{
    public sealed class GoalRoot : Goal
    {
        #region Overrides of Goal

        protected override float EvaluateDesirability()
        {
            return 1.0f;
        }

        protected override void Activate()
        {
            
        }

        protected override GoalStatus ShouldStatusChange(GoalStatus subGoalStatus)
        {
            return GoalStatus.Active;
        }

        protected override void Terminate()
        {
            
        }

        protected override void OnCompleted()
        {

        }

        protected override void OnFailed()
        {

        }

        #endregion
    }
}