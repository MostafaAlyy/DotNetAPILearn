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
    readonly DataContextEF _entityFramework = new(config);
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserToAddDto, User>();
            cfg.CreateMap<User, User>();
            cfg.CreateMap<UserSalary, UserSalary>();
            cfg.CreateMap<UserJobInfo, UserJobInfo>();
        }));


    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        return _entityFramework.Users;
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



    [HttpGet("GetUsersSalary")]
    public IEnumerable<UserSalary> GetUsersSalary()
    {
        return _entityFramework.UsersSalary;
    }

    [HttpGet("GetUserSalary/{userId}")]
    public UserSalary GetUserSalary(int userId)
    {
        UserSalary? userSalary = _entityFramework.UsersSalary.Where(u => u.UserId == userId).FirstOrDefault();
        if (userSalary != null)
            return userSalary;

        throw new Exception("Failed to get user salary");
    }

    [HttpPost("AddUserSalary")]
    public IActionResult AddUserSalary(UserSalary userSalary)
    {

        _entityFramework.Add(userSalary);
        if (_entityFramework.SaveChanges() > 0)
            return Ok();

        throw new Exception("Failed to add user salary");

    }

    [HttpPut("UpdateUserSalary")]
    public IActionResult UpdateUserSalary(UserSalary userSalary)
    {
        UserSalary? userSalaryDb = _entityFramework.UsersSalary.Where(u => u.UserId == userSalary.UserId).FirstOrDefault();

        if (userSalary != null)
        {
            _mapper.Map(userSalary, userSalaryDb);

            if (_entityFramework.SaveChanges() > 0)
                return Ok();

            throw new Exception("Failed to update user salary");
        }

        throw new Exception("Failed to get user salary");

    }

    [HttpDelete("DeleteUserSalary{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        UserSalary? userSalaryDb = _entityFramework.UsersSalary
       .Where(u => u.UserId == userId).FirstOrDefault();

        if (userSalaryDb != null)
        {
            _entityFramework.UsersSalary.Remove(userSalaryDb);

            if (_entityFramework.SaveChanges() > 0)
                return Ok();


            throw new Exception("Failed to delete user salary");
        }


        throw new Exception("Failed to get user salary");
    }

}
