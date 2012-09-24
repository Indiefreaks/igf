using System;

namespace Indiefreaks.Xna.Logic.GoalDriven
{
    public abstract partial class Goal
    {
        private GoalStatus _clientStatus = GoalStatus.Inactive;
        private GoalStatus _serverStatus = GoalStatus.Inactive;

        #region IGoal Members

        private GoalStatus ComputeGoalStatus()
        {
            GoalStatus subGoalStatus;
            
            if (SubGoals.Count != 0 && _mostDesirableSubGoalIndexOnClient != -1)
            {
                Goal mostDesirableSubGoal = SubGoals[_mostDesirableSubGoalIndexOnClient];

                subGoalStatus = mostDesirableSubGoal.ComputeGoalStatus();

                if (subGoalStatus == GoalStatus.Completed && SubGoals.Count > 1)
                    subGoalStatus = GoalStatus.Active;
            }
            else
                subGoalStatus = GoalStatus.Completed;

            if (_clientStatus != GoalStatus.Inactive)
                _serverStatus = ShouldStatusChange(subGoalStatus);
            else _serverStatus = GoalStatus.Active;

            return _serverStatus;
        }

        #endregion

        private object ComputeGoalsShouldChangeStatus(Command command, object networkvalue)
        {
            ComputeGoalStatus();

            return null;
        }

        protected abstract GoalStatus ShouldStatusChange(GoalStatus subGoalStatus);

        protected abstract void Activate();

        protected abstract void Terminate();

        protected abstract void OnCompleted();

        protected abstract void OnFailed();

        private object SendGoalStatusToClients(Command command, object networkvalue)
        {
            return (byte)_serverStatus;
        }

        private void ApplyGoalStatusOnClients(Command command, object networkvalue)
        {
            var serverStatus = (GoalStatus) networkvalue;
            
            switch (serverStatus)
            {
                case GoalStatus.Active:
                    {
                        if (_clientStatus == GoalStatus.Inactive)
                        {
                            _clientStatus = GoalStatus.Active;
                            Activate();
                        }
                        break;
                    }
                case GoalStatus.Completed:
                    {
                        OnCompleted();
                        break;
                    }
                case GoalStatus.Failed:
                    {
                        OnFailed();
                        break;
                    }
                default:
                case GoalStatus.Inactive:
                    {
                        if (_clientStatus == GoalStatus.Active)
                        {
                            _clientStatus = GoalStatus.Inactive;
                            Terminate();
                        }
                        break;
                    }
            }

            _clientStatus = serverStatus;
        }

        private object TerminateAndRemoveCompletedAndFailedGoalsOnClients(Command command)
        {
            for (int i = 0; i < SubGoals.Count;i++)
            {
                var subGoal = SubGoals[i];

                if (subGoal._clientStatus == GoalStatus.Failed || subGoal._clientStatus == GoalStatus.Completed)
                {
                    subGoal.Terminate();
                    SubGoals.Remove(subGoal);
                    
                    if (_mostDesirableSubGoalIndexOnClient == i)
                        _mostDesirableSubGoalIndexOnClient = -1;
                }
            }

            return null;
        }
    }
}