using DotNetAPILearn.Dtos;
using DotNetAPILearn.Models;

namespace DotNetAPILearn.Data
{
    public interface IUserRepo
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToAdd);
        public IEnumerable<User> GetUsers();
        public IEnumerable<UserSalary> GetUsersSalary();
        public IEnumerable<UserJobInfo> GetUsersJopInfo();
        public User GetSingleUser(int userId);
        public UserSalary GetSingleUserSalary(int userId);
        public UserJobInfo GetSingleUserJobInfo(int userId);

    }
}