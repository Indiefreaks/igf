namespace Indiefreaks.Xna.Logic
{
    public enum ExecutionFrequency : int
    {
        /// <summary>
        /// 60 times per second
        /// </summary>
        FullUpdate60Hz = 60,
        /// <summary>
        /// 30 times per second
        /// </summary>
        HalfUpdate30Hz = 30,
        /// <summary>
        /// 15 times per second
        /// </summary>
        PartialUpdate15Hz = 15,
        /// <summary>
        /// 10 times per second
        /// </summary>
        PartialUpdate10Hz = 10,
        /// <summary>
        /// 5 times per second
        /// </summary>
        PartialUpdate5Hz = 5,
        /// <summary>
        /// Once per second
        /// </summary>
        PartialUpdate1Hz = 1,
    }
}