namespace Indiefreaks.Xna.Sessions.Lidgren
{
    public enum LidgrenMessages : byte
    {
        SendLocalPlayersToServer = 0,
        SendNewPlayersToClients,
        SendDisconnectedPlayersToClients,
        SendPlayersListToJustConnectedClient,
        SendSessionStateChangedToClients,
        SendCommandsToClients,
        SendSceneEntitiesToClients,
        SendSynchronizationDoneToServer,
        ExecuteCommandOnServerDataExchanged,
        ExecuteCommandOnServerNoDataExchanged,
        ExecuteServerCommandOnClientsDataExchanged,
        ExecuteServerCommandOnClientsNoDataExchanged
    } ;
}