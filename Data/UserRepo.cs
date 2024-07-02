using DotNetAPILearn.Models;

namespace DotNetAPILearn.Data
{
    public class UserRepo(IConfiguration config) : IUserRepo
    {

        readonly DataContextEF _entityFramework = new(config);

        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
                _entityFramework.Add(entityToAdd);
            else
                throw new Exception("Can't add null entity");

        }

        public User GetSingleUser(int userId)
        {
            User? user = _entityFramework.Users
                    .Where(u => u.UserId == userId).FirstOrDefault();
            if (user != null)
                return user;

            throw new Exception("Failed to get user");
        }

        public UserJobInfo GetSingleUserJobInfo(int userId)
        {
            UserJobInfo? userJobInfo = _entityFramework.UsersJobInfo.Where(u => u.UserId == userId).FirstOrDefault();
            if (userJobInfo != null)
                return userJobInfo;

            throw new Exception("Failed to get user JobInfo");
        }

        public UserSalary GetSingleUserSalary(int userId)
        {
            UserSalary? userSalary = _entityFramework.UsersSalary.Where(u => u.UserId == userId).FirstOrDefault();
            if (userSalary != null)
                return userSalary;

            throw new Exception("Failed to get user salary");
        }

        public IEnumerable<User> GetUsers()
        {
            return _entityFramework.Users;
        }

        public IEnumerable<UserJobInfo> GetUsersJopInfo()
        {
            return _entityFramework.UsersJobInfo;
        }

        public IEnumerable<UserSalary> GetUsersSalary()
        {
            return _entityFramework.UsersSalary;
        }

        public void RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
                _entityFramework.Remove(entityToRemove);
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }
    }
}