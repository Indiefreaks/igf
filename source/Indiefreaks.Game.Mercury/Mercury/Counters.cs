using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectMercury
{
    /// <summary>
    /// Counters to tell you 
    /// </summary>
    public static class Counters
    {
        /// <summary>
        /// How many particles rendered this frame
        /// </summary>
        public static int ParticlesDrawn;

        /// <summary>
        /// How many particles updated this frame
        /// </summary>
        public static int ParticlesUpdated;

        /// <summary>
        /// How many particles triggered this frame
        /// </summary>
        public static int ParticlesTriggered;

        /// <summary>
        /// How many trigger calls were frustum culled this frame
        /// </summary>
        public static int ParticleTriggersCulled;

        /// <summary>
        /// List of particle effects that have >0 active particles
        /// </summary>
        public static List<ParticleEffect> ActiveEffects = new List<ParticleEffect>(20);

        ///<summary>
        /// Resets all Mercury counters - should be called at the start of the frame
        ///</summary>
        public static void StartFrame()
        {
            ParticlesDrawn = 0;
            ParticlesUpdated = 0;
            ParticlesTriggered = 0;
            ParticleTriggersCulled = 0;
            ActiveEffects.Clear();
        }
    }
}
