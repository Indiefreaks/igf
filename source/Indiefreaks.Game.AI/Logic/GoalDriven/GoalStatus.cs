namespace Indiefreaks.Xna.Logic.GoalDriven
{
    /// <summary>
    /// Goal states
    /// </summary>
    public enum GoalStatus : byte
    {
        /// <summary>
        /// Inactive
        /// </summary>
        Inactive = 0,

        /// <summary>
        /// Active
        /// </summary>
        Active = 1,

        /// <summary>
        /// Has Failed
        /// </summary>
        Failed = 2,

        /// <summary>
        /// Has completed
        /// </summary>
        Completed = 3,
    } ;
}