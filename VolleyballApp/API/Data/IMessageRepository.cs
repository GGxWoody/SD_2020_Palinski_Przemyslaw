namespace VolleyballApp.API.Data
{
    public interface IMessageRepository
    {
        void SendMatchUpdate(int firstTeamId,int secondTeamId, int matchId);

    }
}