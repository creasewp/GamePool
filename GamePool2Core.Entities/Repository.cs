using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamePool2Core.Entities;
using Microsoft.WindowsAzure.MobileServices;

namespace GamePool2Core.Entities
{
    public class Repository //: IRepository
    {
        const string applicationURL = @"https://gamepool2.azure-mobile.net/";
        const string applicationKey = @"nOyYWFcVzwgFXXvTLNQVaQFQSxBzCM80";

        //Mobile Service Client reference
        private MobileServiceClient m_MobileService = new MobileServiceClient(
            "https://gamepool2.azure-mobile.net/",
            "nOyYWFcVzwgFXXvTLNQVaQFQSxBzCM80"
            );

        public Repository()
        {
            //Item newitem = new Item { Text = "Awesome item2" };
            //Task.Run(() => client.GetTable<Item>().InsertAsync(newitem)).Wait();
        }

        public async Task<List<Setting>> GetSettings()
        {
            IMobileServiceTable<Setting> settingTable = m_MobileService.GetTable<Setting>();

            List<Setting> settings = await settingTable.ToListAsync();

            if (settings.Count == 0)
            {
                Setting setting = new Setting();
                setting.Description = "IsLocked";
                setting.Value = false.ToString();
                await settingTable.InsertAsync(setting);

                settings.Add(setting);
            }

            return settings;
        }

        public async Task<Setting> GetSetting(string description)
        {
            IMobileServiceTable<Setting> settingTable = m_MobileService.GetTable<Setting>();

            List<Setting> match = await settingTable.Where(item => item.Description == description).ToListAsync();

            return match.FirstOrDefault();
        }

        public async Task UpdateSetting(Setting setting)
        {
            IMobileServiceTable<Setting> settingTable = m_MobileService.GetTable<Setting>();
            await settingTable.UpdateAsync(setting);
        }

        public async Task<User> GetUser(string username)
        {
            User result = null;
            IMobileServiceTable<User> userTable = m_MobileService.GetTable<User>();

            List<User> match = await userTable.Where(item => item.UserName == username).ToListAsync();
            result = match.FirstOrDefault();

            return result;
        }

        public async Task<User> CreateUser(string username, string email, string passwordHash)
        {
            User result = null;
            IMobileServiceTable<User> userTable = m_MobileService.GetTable<User>();
            User match = await GetUser(username);

            //if this user doesn't exist, create it
            if (match == null)
            {
                User newUser = new User();
                newUser.UserName = username;
                newUser.Email = email;
                newUser.PasswordHash = passwordHash;
                newUser.IsEligible = false;
                await userTable.InsertAsync(newUser);

                List<User> matches = await userTable.Where(item => item.UserName == username).ToListAsync();
                result = matches.First();
            }
            else
            {
                //user shouldn't exist, return null
                return null;
            }

            List<UserGame> userGames = await m_MobileService.GetTable<UserGame>().Where(item => item.UserId == result.Id).ToListAsync();
            if (userGames.Count == 0)
            {
                //new user, must create UserGames
                int confidence = 1;
                foreach (Game game in await GetGames())
                {
                    UserGame newUserGame = new UserGame();
                    newUserGame.UserId = result.Id;
                    newUserGame.Confidence = confidence;
                    newUserGame.WinnerTeamId = string.Empty;
                    newUserGame.GameId = game.Id;
                    await m_MobileService.GetTable<UserGame>().InsertAsync(newUserGame);

                    confidence++;
                }
            }
            return result;
        }

        public async Task<UserGame> GetUserGame(string userId, string gameId)
        {

            IMobileServiceTable<UserGame> userGameTable = m_MobileService.GetTable<UserGame>();

            List<UserGame> match = await userGameTable.Where(item => item.UserId == userId && item.GameId == gameId).ToListAsync();

            //if this user doesn't exist, create it
            if (match.Count == 0)
            {
                throw new ArgumentException();
                //UserGame newUserGame = new UserGame();
                //newUserGame.Confidence = 0;
                //newUserGame.WinnerTeamId = string.Empty;
                //newUserGame.GameId = gameId;                
                //await userGameTable.InsertAsync(newUserGame);

                //match = await userGameTable.Where(item => item.GameId == gameId).ToListAsync();
            }

            return match.FirstOrDefault();
        }

        public async Task<Team> GetTeam(string teamId)
        {

            IMobileServiceTable<Team> teamTable = m_MobileService.GetTable<Team>();

            List<Team> match = await teamTable.Where(item => item.Id == teamId).ToListAsync();

            return match.FirstOrDefault();
        }

        public async Task<IList<Game>> GetGames()
        {

            IMobileServiceTable<Game> games = m_MobileService.GetTable<Game>();

            List<Game> listGames = await games.Take(100).ToListAsync();
            IList<Team> teams = await GetTeams();


            foreach (Game game in listGames)
            {
                game.HomeTeam = teams.Single(item => item.Id == game.HomeTeamId);
                game.AwayTeam = teams.Single(item => item.Id == game.AwayTeamId);
            }

            listGames.Sort(new GameByDateTimeComparerASC());


            return listGames;
        }

        public async Task<IList<Team>> GetTeams()
        {
            IMobileServiceTable<Team> teams = m_MobileService.GetTable<Team>();

            IList<Team> listTeams = await teams.Take(100).ToListAsync();

            return listTeams;
        }


        /// <summary>
        /// For now, this returns all users
        /// </summary>
        /// <param name="poolId">The pool identifier.</param>
        /// <returns></returns>
        /// <author>Wayne Creasey</author>
        /// <datetime>10/8/2014</datetime>
        public async Task<IList<User>> GetPoolUsers(string poolId)
        {
            IMobileServiceTable<User> users = m_MobileService.GetTable<User>();

            IList<User> listUsers = await users.OrderByDescending(item => item.PoolScore).ToListAsync();

            return listUsers;
        }

        public async Task<IList<UserGame>> GetUserGames(string userId)
        {

            IMobileServiceTable<UserGame> userGames = m_MobileService.GetTable<UserGame>();

            IList<UserGame> listUserGames = await userGames.Where(item => item.UserId == userId).OrderByDescending(item => item.Confidence).ToListAsync();

            return listUserGames;
        }


        public async Task UpdateUserGames(IList<UserGame> userGames)
        {
            IMobileServiceTable<UserGame> userGameTable = m_MobileService.GetTable<UserGame>();
            foreach (UserGame userGame in userGames)
            {
                await userGameTable.UpdateAsync(userGame);
            }
        }

        public async Task DeleteUserGame(UserGame userGame)
        {
            IMobileServiceTable<UserGame> userGameTable = m_MobileService.GetTable<UserGame>();
            await userGameTable.DeleteAsync(userGame);
        }

        public async Task UpdateUser(User user)
        {
            IMobileServiceTable<User> userTable = m_MobileService.GetTable<User>();
            await userTable.UpdateAsync(user);            
        }

        public async Task DeleteUser(User user)
        {
            IMobileServiceTable<User> userTable = m_MobileService.GetTable<User>();
            await userTable.DeleteAsync(user);
        }

        public async Task UpdateTeam(Team team)
        {
            IMobileServiceTable<Team> teamTable = m_MobileService.GetTable<Team>();
            await teamTable.UpdateAsync(team);            
        }

        public async Task DeleteTeam(Team team)
        {
            IMobileServiceTable<Team> teamTable = m_MobileService.GetTable<Team>();
            await teamTable.DeleteAsync(team);
        }

        public async Task InsertTeam(Team team)
        {
            IMobileServiceTable<Team> teamTable = m_MobileService.GetTable<Team>();
            await teamTable.InsertAsync(team);
        }

        public async Task UpdateGame(Game game)
        {
            IMobileServiceTable<Game> gameTable = m_MobileService.GetTable<Game>();
            await gameTable.UpdateAsync(game);
        }

        public async Task DeleteGame(Game game)
        {
            IMobileServiceTable<Game> gameTable = m_MobileService.GetTable<Game>();
            await gameTable.DeleteAsync(game);
        }

        public async Task InsertGame(Game game)
        {
            IMobileServiceTable<Game> gameTable = m_MobileService.GetTable<Game>();
            await gameTable.InsertAsync(game);
        }
    }

}
