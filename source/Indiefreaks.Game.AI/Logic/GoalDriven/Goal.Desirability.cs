using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indiefreaks.Xna.Logic.GoalDriven
{
    public abstract partial class Goal
    {
        private int _mostDesirableSubGoalIndexOnServer = -1;
        private int _mostDesirableSubGoalIndexOnClient = -1;

        public float Desirability { get; private set; }

        private object ComputeGoalsDesirabilityOnServer(Command command, object networkvalue)
        {
            ComputeSubGoalsDesirability();

            return null;
        }

        private void ComputeSubGoalsDesirability()
        {
            var higherDesirabilityInSubGoals = 0f;
            _mostDesirableSubGoalIndexOnServer = -1;

            for (int i = 0; i < SubGoals.Count; i++)
            {
                var subGoal = SubGoals[i];
                
                subGoal.ComputeSubGoalsDesirability();

                subGoal.Desirability = subGoal.EvaluateDesirability();

                if (subGoal.Desirability > higherDesirabilityInSubGoals)
                {
                    higherDesirabilityInSubGoals = subGoal.Desirability;
                    _mostDesirableSubGoalIndexOnServer = i;
                }
            }
        }
        
        protected abstract float EvaluateDesirability();

        private object SendMostDesirableSubGoalIndexToClients(Command command, object networkvalue)
        {
            return _mostDesirableSubGoalIndexOnServer;
        }

        private void ApplyMostDesirableSubGoalIndexOnClients(Command command, object networkvalue)
        {
            _mostDesirableSubGoalIndexOnClient = (int)networkvalue;
        }

    }
}
