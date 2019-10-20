using Common;
using DAL.SignalRChat;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;


namespace BL.SignalRChat
{
    public class UserBL
    {
        private readonly DB _db;

        public UserBL()
        {
            _db = new DB();
        }
       
        public async Task<string> Register(User user, Action<string> UserConnected)
        {
            var findUserDb = await _db.Users.SingleOrDefaultAsync(userDB => userDB.Name == user.Name);
            if (findUserDb != null)
                return "User exists";
            else if (user.Password.Count() < 4)
                return "Password length must be 4 or more";
            else
            {
                User userToDB = new User(user.Name, user.Password);
                _db.Users.Add(userToDB);
                await _db.SaveChangesAsync();

                UserConnected(user.Name);

                return string.Empty;
            }
        }

        public async Task<string> Login(User user, Action<string> UserConnected)
        {
            var findUserDb = await _db.Users.SingleOrDefaultAsync(userDB => userDB.Name == user.Name);

            if (findUserDb != null)
            {
                if (findUserDb.Name == user.Name && findUserDb.Password == user.Password) // && !item.Online
                {
                    findUserDb.Online = true;
                    await _db.SaveChangesAsync();
                    UserConnected(user.Name);
                    return string.Empty;
                }

                return "User or Password is in corrent";
            }
            else
                return "User not exists";
        }

        public async Task<string> Login(User user)
        {
            var findUserDb = await _db.Users.SingleOrDefaultAsync(userDB => userDB.Name == user.Name);

            if (findUserDb != null)
            {
                if (findUserDb.Name == user.Name && findUserDb.Password == user.Password) // && !item.Online
                {
                    findUserDb.Online = true;
                    await _db.SaveChangesAsync();
                    return string.Empty;
                }

                return "User or Password is in corrent";
            }
            else
                return "User not exists";
        }


        public async Task<string> Logout(User user, Action<string> UserDisccouncted)
        {
            var userInDB = await _db.Users.SingleOrDefaultAsync(userDb => userDb.Name == user.Name);
            userInDB.Online = false;
            await _db.SaveChangesAsync();
            UserDisccouncted(user.Name);

            return string.Empty;
        }


        public async Task<IEnumerable<string>> GetAllUsers()
        {

            var dictionary = await _db.Users.AsNoTracking().ToDictionaryAsync(user => user.Name, user => user.Password);

            return dictionary.Keys;
        }

        public async Task<IEnumerable<string>> GetAllOnllineUsers()
        {
            var list = await _db.Users.AsNoTracking().Where(x => x.Online).ToListAsync();

            var dictionary = list.ToDictionary
            (user => user.Name, user => user.Password);

            var checkValue = dictionary.Keys;

            return dictionary.Keys;
        }

        public async Task<IEnumerable<string>> GetAllOfflineUsers()
        {
            var dictinory = await _db.Users.AsNoTracking().Where(x => !x.Online).ToDictionaryAsync(user => user.Name, user => user.Password);
            return dictinory.Keys;
        }

        public async Task<User> GetUser(string userName)
        {
            var userInDB = await _db.Users.SingleOrDefaultAsync(userDB => userDB.Name == userName);

            return userInDB ?? userInDB;
        }
    }
}
