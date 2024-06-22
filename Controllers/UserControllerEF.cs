using DotNetAPILearn.Models;
using DotNetAPILearn.Data;
using Microsoft.AspNetCore.Mvc;
using DotNetAPILearn.Dtos;
using AutoMapper;

namespace DotNetAPILearn.Controllers;

[ApiController]
[Route("[controller]")]
public class UserControllerEF(IConfiguration config) : ControllerBase
{
    DataContextEF _entityFramework = new(config);
    IMapper _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserToAddDto, User>();
            cfg.CreateMap<UserSalary, UserSalary>();
            cfg.CreateMap<UserJobInfo, UserJobInfo>();
        }));


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
    public IActionResult AddUser(UserToAddDto user)
    {
        User userToAdd = _mapper.Map<User>(user);


        _entityFramework.Add(userToAdd);
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
            _mapper.Map(user, userDb);

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
