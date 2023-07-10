using Sendz_Climbing_Journal.Models;
using Sendz_Climbing_Journal.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Sendz_Climbing_Journal.Services
{
    public static class DatabaseServices
    {
        private static SQLiteAsyncConnection dBConnection;

        //Initializes tables and creates absolute path to database file
        static async Task Init()
        {
            if (dBConnection != null)
            {
                return;
            }

            var dBPath = Path.Combine(FileSystem.AppDataDirectory, "SendzJournal.db");

            dBConnection = new SQLiteAsyncConnection(dBPath);

            await dBConnection.CreateTableAsync<User>();
            await dBConnection.CreateTableAsync<Gym>();
            await dBConnection.CreateTableAsync<Crag>();
        }

        #region User Methods

        public static async Task AddUser(string name, string email, string password)
        {
            await Init();

            var user = new User
            {
                Name = name,
                Email = email,
                Password = password
            };

            await dBConnection.InsertAsync(user);

            var id = user.Id;
        }

        public static async Task RemoveUser(int id)
        {
            await Init();

            await dBConnection.DeleteAsync<User>(id);
        }

        public static async Task<IEnumerable<User>> GetUsers()
        {
            await Init();

            var users = await dBConnection.Table<User>().ToListAsync();

            return users;
        }

        public static async Task<IEnumerable<User>> GetUserById(int id)
        {
            await Init();

            var user = await dBConnection.Table<User>().Where(i => i.Id == id).ToListAsync();

            return user;
        }

        public static async Task<string> GetUserName(int userId)
        {
            string userName = await dBConnection.ExecuteScalarAsync<string>($"SELECT Name " +
                $"FROM User WHERE Id = ?", userId);

            return userName;
        }

        public static async Task<IEnumerable<User>> GetAllUsers()
        {
            await Init();

            var user = await dBConnection.Table<User>().ToListAsync();
            return user;
        }

        public static async Task UpdateUser(int id, string name, string email, string password)
        {
            await Init();

            var userQuery = await dBConnection.Table<User>().Where(i => i.Id == id).FirstOrDefaultAsync();

            if (userQuery != null)
            {
                userQuery.Name = name;
                userQuery.Email = email;
                userQuery.Password = password;

                await dBConnection.UpdateAsync(userQuery);
            }
        }

        #endregion

        #region Crag Methods

        public static async Task AddCragLog(int userId, string state, string cragName, string type, string name,
            string grade, bool sent, string sendType, DateTime sendDate, string notes)
        {
            await Init();

            var crag = new Crag
            {
                UserId = userId,
                State = state,
                CragName = cragName,
                Type = type,
                Name = name,
                Grade = grade,
                Sent = sent,
                SendType = sendType,
                SendDate = sendDate,
                Notes = notes
            };

            await dBConnection.InsertAsync(crag);

            var id = crag.Id;
        }

        public static async Task RemoveCragLog(int id)
        {
            await Init();

            await dBConnection.DeleteAsync<Crag>(id);
        }

        public static async Task<IEnumerable<Crag>> GetCragLogs(int userId)
        {
            await Init();

            var cragLogs = await dBConnection.Table<Crag>().Where(i => i.UserId == userId).ToListAsync();

            return cragLogs;
        }

        public static async Task UpdateCragLog(int id, string state, string cragName, string type, string name,
            string grade, bool sent, string sendType, DateTime sendDate, string notes)
        {
            await Init();

            var cragQuery = await dBConnection.Table<Crag>().Where(i => i.Id == id).FirstOrDefaultAsync();

            if (cragQuery != null)
            {
                cragQuery.State = state;
                cragQuery.CragName = cragName;
                cragQuery.Type = type;
                cragQuery.Name = name;
                cragQuery.Grade = grade;
                cragQuery.Sent = sent;
                cragQuery.SendType = sendType;
                cragQuery.SendDate = sendDate;
                cragQuery.Notes = notes;

                await dBConnection.UpdateAsync(cragQuery);
            }
        }

        public static async Task<int> GetCragLogCount(int userId)
        {
            int cragLogCount = await dBConnection.ExecuteScalarAsync<int>($"SELECT COUNT(*) " +
                $"FROM Crag WHERE UserId = ?", userId);

            return cragLogCount;
        }

        #endregion

        #region Gym Methods

        public static async Task AddGymLog(int userId, string gymName, string type, string grade, bool sent, 
            string sendType, DateTime sendDate, string description, string notes)
        {
            await Init();

            var gym = new Gym
            {
                UserId = userId,
                GymName = gymName,
                Type = type, 
                Grade = grade,
                Sent = sent,
                SendType = sendType,
                SendDate = sendDate,
                Description = description,
                Notes = notes
            };

            await dBConnection.InsertAsync(gym);

            var id = gym.Id;
        }

        public static async Task RemoveGymLog(int id)
        {
            await Init();

            await dBConnection.DeleteAsync<Gym>(id);
        }

        public static async Task UpdateGymLog(int id, string gymName, string type, string grade, bool sent,
            string sendType, DateTime sendDate, string description, string notes)
        {
            await Init();

            var gymQuery = await dBConnection.Table<Gym>().Where(i => i.Id == id).FirstOrDefaultAsync();

            if (gymQuery != null)
            {
                gymQuery.GymName = gymName;
                gymQuery.Type = type;
                gymQuery.Grade = grade;
                gymQuery.Sent = sent;
                gymQuery.SendType = sendType;
                gymQuery.SendDate = sendDate;
                gymQuery.Description = description;
                gymQuery.Notes = notes;

                await dBConnection.UpdateAsync(gymQuery);
            }
        }

        public static async Task<IEnumerable<Gym>> GetGymLogs(int userId)
        {
            await Init();

            var gymLogs = await dBConnection.Table<Gym>().Where(i => i.UserId == userId).ToListAsync();

            return gymLogs;
        }

        public static async Task<int> GetGymLogCount(int userId)
        {
            int gymLogsCount = await dBConnection.ExecuteScalarAsync<int>($"SELECT COUNT(*) " +
                $"FROM Gym WHERE UserId = ?", userId);

            return gymLogsCount;
        }

        #endregion

        #region Sample Data

        public static async void LoadSampleData()
        {
            User user = new User
            {
                Name = "Test",
                Email = "test@email.com",
                Password = "test"
            };

            await dBConnection.InsertAsync(user);

            Crag crag = new Crag
            {
                UserId = user.Id,
                State = "KY",
                CragName = "Funk Rock City",
                Type = "Sport",
                Name = "Orange Juice",
                Grade = "5.12c",
                SendType = "Open Project",
                Sent = false,
                SendDate = DateTime.Today.AddDays(-3),
                Notes = "Ultra classic."
            };

            await dBConnection.InsertAsync(crag);

            Crag crag1 = new Crag
            {
                UserId = user.Id,
                State = "UT",
                CragName = "Turtle Wall",
                Type = "Sport",
                Name = "The Waltz",
                Grade = "5.12d",
                Sent = true,
                SendType = "Redpoint",
                SendDate = DateTime.Today.AddDays(-1),
                Notes = "Hard boulder start."
            };

            await dBConnection.InsertAsync(crag1);

            Crag crag2 = new Crag
            {
                UserId = user.Id,
                State = "TX",
                CragName = "Hueco Tanks",
                Type = "Boulder",
                Name = "Baby Face",
                Grade = "V7",
                SendType = "Projected",
                Sent = true,
                SendDate = DateTime.Today.AddDays(-5),
                Notes = "Technical, powerful, tall."
            };

            await dBConnection.InsertAsync(crag2);

            Gym gym = new Gym
            {
                UserId = user.Id,
                GymName = "The Climbing Gym",
                Type = "Boulder",
                Grade = "V6",
                Sent = true,
                SendType = "Flash",
                SendDate = DateTime.Today,
                Description = "Pink in the corner.",
                Notes = "Harder than it looked."
            };

            await dBConnection.InsertAsync(gym);

            Gym gym1 = new Gym
            {
                UserId = user.Id,
                GymName = "The Other Climbing Gym",
                Type = "Lead",
                Grade = "5.11+",
                Sent = true,
                SendType = "Redpoint",
                SendDate = DateTime.Today,
                Description = "Blue on the overhang.",
                Notes = "Cool roof sequence."
            };

            await dBConnection.InsertAsync(gym1);
        }

        public static async Task ClearSampleData()
        {
            await Init();

            await dBConnection.DropTableAsync<User>();
            await dBConnection.DropTableAsync<Gym>();
            await dBConnection.DropTableAsync<Crag>();

            dBConnection = null;
        }

        #endregion

    }
}
