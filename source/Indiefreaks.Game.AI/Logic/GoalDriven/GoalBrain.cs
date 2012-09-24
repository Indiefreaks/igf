namespace Indiefreaks.Xna.Logic.GoalDriven
{
    /// <summary>
    /// Add this component to your SceneEntity or SceneObject instances to provide him with Goal Driven AI
    /// </summary>
    public class GoalBrain : NonPlayerAgent
    {
        private readonly GoalRoot _root;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public GoalBrain()
        {
            _root = new GoalRoot();
            base.Add(_root);
        }

        /// <summary>
        /// Adds a new Goal to the brain
        /// </summary>
        /// <param name="behavior"></param>
        public override void Add(Behavior behavior)
        {
            if(behavior is Goal)
                _root.Add(behavior as Goal);
            else
                base.Add(behavior);
        }

        /// <summary>
        /// Removes a Goal from the brain
        /// </summary>
        /// <param name="behavior"></param>
        public override void Remove(Behavior behavior)
        {
            if(behavior is Goal)
                _root.Remove(behavior as Goal);
            else
                base.Remove(behavior);
        }
    }
}