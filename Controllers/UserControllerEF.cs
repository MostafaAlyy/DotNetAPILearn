using DotNetAPILearn.Models;
using DotNetAPILearn.Data;
using Microsoft.AspNetCore.Mvc;
using DotNetAPILearn.Dtos;

namespace DotNetAPILearn.Controllers;

[ApiController]
[Route("[controller]")]
public class UserControllerEF : ControllerBase
{
    DataContextEF _entityFramework;
    public UserControllerEF(IConfiguration config)
    {
        _entityFramework = new DataContextEF(config);
    }



    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        return _entityFramework.Users.ToList();
    }


    [HttpGet("GetSingleUsers/{userId}")]
    public User GetSingleUsers(int userId)
    {
        User? user = _entityFramework.Users
                    .Where(u => u.UserId == userId).FirstOrDefault();
        if (user != null)
            return user;

        throw new Exception("Failed to get user");
    }



    [HttpPost("AddUser")]
    public IActionResult AddUser(UserDto user)
    {
        User userDb = new User();
        userDb.Active = user.Active;
        userDb.LastName = user.LastName;
        userDb.FirstName = user.FirstName;
        userDb.Gender = user.Gender;
        userDb.Email = user.Email;
        _entityFramework.Add(userDb);
        if (_entityFramework.SaveChanges() > 0)
            return Ok();

        throw new Exception("Failed to add user");

    }

    [HttpPut("UpdateUser")]
    public IActionResult UpdateUser(User user)
    {
        User? userDb = _entityFramework.Users
                .Where(u => u.UserId == user.UserId).FirstOrDefault();
        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.LastName = user.LastName;
            userDb.FirstName = user.FirstName;
            userDb.Gender = user.Gender;
            userDb.Email = user.Email;

            if (_entityFramework.SaveChanges() > 0)
                return Ok();

            throw new Exception("Failed to update user");
        }
        throw new Exception("Failed to get user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _entityFramework.Users
            .Where(u => u.UserId == userId).FirstOrDefault();

        if (userDb != null)
        {
            _entityFramework.Users.Remove(userDb);

            if (_entityFramework.SaveChanges() > 0)
                return Ok();


            throw new Exception("Failed to delete user");
        }


        throw new Exception("Failed to get user");
    }
}
