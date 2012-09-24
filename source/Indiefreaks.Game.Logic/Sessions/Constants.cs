namespace Indiefreaks.Xna.Sessions
{
    /// <summary>
    /// Constants used by the Session
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Message Id used by the Session host to execute a command on all clients (only comand orders)
        /// </summary>
        public const byte ExecuteServerCommandOnClientsNoDataExchanged = 0;

        /// <summary>
        /// Message Id used by the Session host to execute a command on all clients (with data exchanged)
        /// </summary>
        public const byte ExecuteServerCommandOnClientsDataExchanged = 1;

        /// <summary>
        /// Message Id used to send a request on session host to execute a command (only command orders)
        /// </summary>
        public const byte ExecuteCommandOnServerNoDataExchanged = 2;

        /// <summary>
        /// Message Id used to send a request on session host to execute a command (with data exchanged)
        /// </summary>
        public const byte ExecuteCommandOnServerDataExchanged = 3;

        /// <summary>
        /// Message Id used to send a request to clients to create locally a command
        /// </summary>
        public const byte SynchronizeCommandOnClient = 4;

        /// <summary>
        /// Message Id used to send a request to clients to assign a similar SceneEntity Unique Id locally as on server
        /// </summary>
        public const byte SynchronizeSceneEntitiesOnClient = 5;

        /// <summary>
        /// Message Id used to notify the server the client has finished synchronizing commands & scene entities
        /// </summary>
        public const byte SynchronizationDoneOnClient = 6;
    }
}