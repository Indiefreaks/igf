using System;
using Indiefreaks.Xna.Sessions;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Replicates the current AutonomousAgent Seed value to all
    /// </summary>
    public class AutonomousAgentSeedInitializationBehavior : Behavior
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public AutonomousAgentSeedInitializationBehavior()
        {
            AddCondition(() => ((AutonomousAgent)Agent).Seed == 0);

            AddServerCommand(InitializeSeedOnServer, ReplicateSeedOnClients, typeof (int), DataTransferOptions.ReliableInOrder);
        }

        /// <summary>
        /// Initialize the AutonoumousAgent seed
        /// </summary>
        /// <param name="command"></param>
        /// <param name="networkvalue"></param>
        /// <returns></returns>
        private object InitializeSeedOnServer(Command command, object networkvalue)
        {
            // TODO: Add a cached Seed on the server to allow JoinInProgress as well as the number of calls made to the Random instance to synchronize it
            return new Random().Next();
        }

        /// <summary>
        /// Replicates the Seed value to all clients
        /// </summary>
        /// <param name="command"></param>
        /// <param name="networkvalue"></param>
        private void ReplicateSeedOnClients(Command command, object networkvalue)
        {
            var seed = (int) networkvalue;

            ((AutonomousAgent)Agent).Seed = seed;
            ((AutonomousAgent) Agent).Dice = new Random(seed);
        }
    }
}