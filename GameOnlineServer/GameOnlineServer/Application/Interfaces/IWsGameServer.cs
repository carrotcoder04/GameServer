namespace GameOnlineServer.Application.Interfaces
{
    public interface IWsGameServer
    {
        void StartServer();
        void StopServer();
        void RestartServer();
    }
}
