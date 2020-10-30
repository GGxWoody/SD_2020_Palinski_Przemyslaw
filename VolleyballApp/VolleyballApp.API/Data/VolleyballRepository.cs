using System.Threading.Tasks;
using VolleyballApp.API.Models;
using Microsoft.EntityFrameworkCore;
using DatingApp.API.Helpers;
using System.Linq;
using System;
using AutoMapper;
using VolleyballApp.API.Dtos;

namespace VolleyballApp.API.Data
{
    public class VolleyballRepository : IVolleyballRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public VolleyballRepository(DataContext context, IMapper mapper)
        {
            this._mapper = mapper;
            this._context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public async Task<Team> CreateTeam(Team team)
        {
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Team> GetTeam(int id)
        {
            var team = await _context.Teams.Include(e => e.Users).Include(c => c.Owner).FirstOrDefaultAsync(u => u.Id == id);
            return team;
        }

        public async Task<PagedList<Team>> GetTeams(UserParams userParams)
        {
            var teams = _context.Teams.OrderByDescending(u => u.DateCreated);
            return await PagedList<Team>.CreateAsync(teams, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<PagedList<Invite>> GetAllUserFriendInvites(UserParams userParams, int userId)
        {
            var invites = _context.Invites.Include(e => e.InviteFrom).Include(e => e.InviteTo)
            .OrderByDescending(x => x.Id).AsQueryable();
            invites = invites.Where(i => i.InviteTo.Id == userId);
            return await PagedList<Invite>.CreateAsync(invites, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(e => e.Teams)
                .Include(e => e.TeamsCreated).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.OrderByDescending(u => u.LastActive).AsQueryable();
            users = users.Where(u => u.Id != userParams.UserID);
            users = users.Where(u => u.Gender == userParams.Gender);

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    case "age":
                        users = users.OrderByDescending(u => u.DateOfBirth);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> saveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> TeamExists(string name)
        {
            if(await _context.Teams.AnyAsync(x => x.TeamName.ToLower() == name.ToLower())) return true;
            return false;
        }

        public async Task<Invite> GetFriendInvite(int userId, int id)
        {
            var invite = await _context.Invites.Include(e => e.InviteFrom).Include(e => e.InviteTo)
            .FirstOrDefaultAsync(x => x.InviteFrom.Id == userId && x.InviteTo.Id == id && x.FriendInvite == true);
            return invite;
        }

        public async Task<Invite> CreateFriendInvite(User sender, User reciver)
        {
            Invite newInvite = new Invite();
            newInvite.InviteFrom = sender;
            newInvite.InviteTo = reciver;
            newInvite.FriendInvite = true;
            await _context.Invites.AddAsync(newInvite);
            await _context.SaveChangesAsync();
            return await GetFriendInvite(sender.Id,reciver.Id);
        }


        public async Task<Friendlist> AcceptFriendInvite(User sender, User reciver)
        {
            Friendlist fl = new Friendlist();
            fl.FirstUser = sender;
            fl.SecoundUser = reciver;
            var InviteToDelete = await GetFriendInvite(sender.Id, reciver.Id);
            _context.Invites.Remove(InviteToDelete);
            await _context.Friendlist.AddAsync(fl);
            await _context.SaveChangesAsync();
            return await _context.Friendlist.Include(e => e.FirstUser).Include(e => e.SecoundUser)
            .FirstOrDefaultAsync(x => x.FirstUser.Id == sender.Id && x.SecoundUser.Id == reciver.Id);
        }

        public async Task<bool> AreFriends(int firstId, int secoundId)
        {
            var friendlistNode = await _context.Friendlist.FirstOrDefaultAsync(x => x.FirstUser.Id == firstId && x.SecoundUser.Id == secoundId
            || x.FirstUser.Id == secoundId && x.SecoundUser.Id == firstId);
            if (friendlistNode == null) return false;
            return true;
        }


        public async Task<bool> IsInivtedToFriends(int userId, int id)
        {
            var invite = await _context.Invites.FirstOrDefaultAsync(x => x.InviteFrom.Id == userId && x.InviteTo.Id == id
            || x.InviteFrom.Id == id && x.InviteTo.Id == userId);
            if (invite == null) return false;
            return true;
        }
    }
}