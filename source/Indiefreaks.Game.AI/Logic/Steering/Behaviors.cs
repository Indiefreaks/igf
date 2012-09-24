namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Simple class storing all Steering Behaviors to ease their access in code
    /// </summary>
    public class Behaviors
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public Behaviors()
        {
            Alignment = new Alignment();
            Arrive = new Arrive();
            Cohesion = new Cohesion();
            Evade = new Evade();
            Flee = new Flee();
            Hide = new Hide();
            Interpose = new Interpose();
            ObstacleAvoidance = new ObstacleAvoidance();
            OffsetPursuit = new OffsetPursuit();
            PathFollowing = new PathFollowing();
            Pursuit = new Pursuit();
            Seek = new Seek();
            Separation = new Separation();
            WallAvoidance = new WallAvoidance();
            Wander = new Wander();
        }

        public Alignment Alignment { get; private set; }
        public Arrive Arrive { get; private set; }
        public Cohesion Cohesion { get; private set; }
        public Evade Evade { get; private set; }
        public Flee Flee { get; private set; }
        public Hide Hide { get; private set; }
        public Interpose Interpose { get; private set; }
        public ObstacleAvoidance ObstacleAvoidance { get; private set; }
        public OffsetPursuit OffsetPursuit { get; private set; }
        public PathFollowing PathFollowing { get; private set; }
        public Pursuit Pursuit { get; private set; }
        public Seek Seek { get; private set; }
        public Separation Separation { get; private set; }
        public WallAvoidance WallAvoidance { get; private set; }
        public Wander Wander { get; private set; }
    }
}