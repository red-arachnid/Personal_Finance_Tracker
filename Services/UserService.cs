using Personal_Finance_Tracker.Data;
using Personal_Finance_Tracker.Models;

namespace Personal_Finance_Tracker.Services
{
    public class UserService
    {
        private readonly Repository _repository;
        private PFTData _data;

        public UserService()
        {
            _repository = new Repository();
            _data = _repository.LoadData();
        }

        /// <summary>Login User</summary>
        /// <returns>Returns User or null if password or username are incorrect</returns>
        public User Login(string username, string password)
        {
            //Find User through username
            var user = _data.Users.SingleOrDefault(u => u.Username.Equals(username, StringComparison.Ordinal));

            if (user == null)
                return null!;

            //Check password
            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                return user;

            return null!;
        }

        /// <summary>Add New User</summary>
        /// <returns>Returns true if successfully added else false</returns>
        public bool AddUser(string username, string password)
        {
            //Check if the given username exist or not
            if (CheckDuplicateUser(username))
                return false;

            string hasedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            User newUser = new User { Username = username, Password = hasedPassword };
            _data.Users.Add(newUser);
            _repository.SaveData(_data);
            return true;
        }

        /// <summary>Checks If a user already exist with the username</summary>
        /// <returns>Returns true if it exists</returns>
        public bool CheckDuplicateUser(string username)
        {
            if (_data.Users.Any(u => u.Username.Equals(username, StringComparison.Ordinal)))
                return true;
            
            return false;
        }
    }
}
