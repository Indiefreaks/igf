using System;
using System.Collections.Generic;
using Indiefreaks.Xna.Sessions;

namespace Indiefreaks.Xna.Logic.GoalDriven
{
    public abstract partial class Goal : Behavior
    {
        protected readonly List<Goal> SubGoals = new List<Goal>();

        protected Goal()
        {
            AddServerCommand(() => this is GoalRoot, ComputeGoalsDesirabilityOnServer);
            AddServerCommand(() => _mostDesirableSubGoalIndexOnServer != _mostDesirableSubGoalIndexOnClient, SendMostDesirableSubGoalIndexToClients, ApplyMostDesirableSubGoalIndexOnClients, typeof (int), DataTransferOptions.ReliableInOrder);
            AddLocalCommand(() => SubGoals.Count != 0, TerminateAndRemoveCompletedAndFailedGoalsOnClients);
            AddServerCommand(() => this is GoalRoot, ComputeGoalsShouldChangeStatus);
            AddServerCommand(IsStatusDifferentBetweenServerAndClient, SendGoalStatusToClients, ApplyGoalStatusOnClients, typeof (byte), DataTransferOptions.ReliableInOrder);
        }

        private bool IsStatusDifferentBetweenServerAndClient()
        {
            return _serverStatus != _clientStatus;
        }

        public Goal ParentGoal { get; private set; }

        public bool IsRoot { get { return (ParentGoal != null && ParentGoal is GoalRoot); } }

        public void Add(Goal goal)
        {
            goal.Agent = Agent;
            goal.ParentGoal = this;
            goal.Initialize();
            SubGoals.Add(goal);
        }

        public void Remove(Goal goal)
        {
            goal.ParentGoal = null;
            goal.Terminate();
            SubGoals.Remove(goal);
        }

        protected override void Process(float elapsed)
        {
            foreach (var subGoal in SubGoals)
                ((IProcess)subGoal).Process(elapsed);

            base.Process(elapsed);
        }
    }

    public abstract class Goal<T> : Goal where T:GoalBrain
    {
        public T Brain { get; private set; }

        public override void Initialize()
        {
            base.Initialize();

            Brain = Agent as T;
        }
    }
}