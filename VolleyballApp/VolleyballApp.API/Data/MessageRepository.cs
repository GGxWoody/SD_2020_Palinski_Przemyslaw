using Microsoft.EntityFrameworkCore;

namespace VolleyballApp.API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;

        public MessageRepository(DataContext context)
        {
            this._context = context;
        }
        public async void SendMatchUpdate(int firstTeamId, int secondTeamId, int matchId)
        {
            var firstTeam = await _context.Teams.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == firstTeamId);
            var firstTeamPlayers = firstTeam.Users;
            foreach (var user in firstTeamPlayers)
            {
                if(user.Mail !=null) Helpers.MailSender.sendMatchUpdate(matchId, user.Mail);
            }
            var secondTeam = await _context.Teams.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == secondTeamId);
            var secondTeamPlayers = secondTeam.Users;
            foreach (var user in secondTeamPlayers)
            {
                if(user.Mail !=null) Helpers.MailSender.sendMatchUpdate(matchId, user.Mail);
            }
        }
    }
}