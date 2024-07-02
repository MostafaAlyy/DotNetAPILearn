using DotNetAPILearn.Models;
using DotNetAPILearn.Data;
using Microsoft.AspNetCore.Mvc;
using DotNetAPILearn.Dtos;
using AutoMapper;

namespace DotNetAPILearn.Controllers;

[ApiController]
[Route("[controller]")]
public class UserControllerEF(IConfiguration config, IUserRepo userRepo) : ControllerBase
{
    // readonly DataContextEF _entityFramework = new(config);

    readonly IUserRepo _userRepository = userRepo;
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
        return _userRepository.GetUsers();
    }


    [HttpGet("GetSingleUsers/{userId}")]
    public User GetSingleUsers(int userId)
    {
        return _userRepository.GetSingleUser(userId);
    }



    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        User userToAdd = _mapper.Map<User>(user);

        _userRepository.AddEntity(userToAdd);
        if (_userRepository.SaveChanges())
            return Ok();

        throw new Exception("Failed to add user");

    }

    [HttpPut("UpdateUser")]
    public IActionResult UpdateUser(User user)
    {
        User? userDb = _userRepository.GetSingleUser(user.UserId);

        if (userDb != null)
        {
            _mapper.Map(user, userDb);

            if (_userRepository.SaveChanges())
                return Ok();

            throw new Exception("Failed to update user");
        }
        throw new Exception("Failed to get user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _userRepository.GetSingleUser(userId);

        if (userDb != null)
        {
            _userRepository.RemoveEntity(userDb);
            if (_userRepository.SaveChanges())
                return Ok();

            throw new Exception("Failed to delete user");
        }


        throw new Exception("Failed to get user");
    }






    [HttpGet("GetUsersSalary")]
    public IEnumerable<UserSalary> GetUsersSalary()
    {
        return _userRepository.GetUsersSalary();
    }

    [HttpGet("GetUserSalary/{userId}")]
    public UserSalary GetUserSalary(int userId)
    {
        return _userRepository.GetSingleUserSalary(userId);

    }

    [HttpPost("AddUserSalary")]
    public IActionResult AddUserSalary(UserSalary userSalary)
    {

        _userRepository.AddEntity(userSalary);
        if (_userRepository.SaveChanges())
            return Ok();

        throw new Exception("Failed to add user salary");

    }

    [HttpPut("UpdateUserSalary")]
    public IActionResult UpdateUserSalary(UserSalary userSalary)
    {
        UserSalary? userSalaryDb = _userRepository.GetSingleUserSalary(userSalary.UserId);

        if (userSalary != null)
        {
            _mapper.Map(userSalary, userSalaryDb);

            if (_userRepository.SaveChanges())
                return Ok();

            throw new Exception("Failed to update user salary");
        }

        throw new Exception("Failed to get user salary");

    }

    [HttpDelete("DeleteUserSalary{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        UserSalary? userSalaryDb = _userRepository.GetSingleUserSalary(userId);

        if (userSalaryDb != null)
        {
            _userRepository.RemoveEntity(userSalaryDb);

            if (_userRepository.SaveChanges())
                return Ok();


            throw new Exception("Failed to delete user salary");
        }


        throw new Exception("Failed to get user salary");
    }




    [HttpGet("GetUsersJobInfo")]
    public IEnumerable<UserJobInfo> GetUsersJobInfo()
    {
        return _userRepository.GetUsersJopInfo();
    }

    [HttpGet("GetUserJobInfo/{userId}")]
    public UserJobInfo GetUserJobInfo(int userId)
    {
        return _userRepository.GetSingleUserJobInfo(userId);
    }

    [HttpPost("AddUserJobInfo")]
    public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
    {

        _userRepository.AddEntity(userJobInfo);
        if (_userRepository.SaveChanges())
            return Ok();

        throw new Exception("Failed to add user JobInfo");

    }

    [HttpPut("UpdateUserJobInfo")]
    public IActionResult UpdateUserJobInfo(UserJobInfo userJobInfo)
    {
        UserJobInfo? userJobInfoDb = _userRepository.GetSingleUserJobInfo(userJobInfo.UserId);

        if (userJobInfo != null)
        {
            _mapper.Map(userJobInfo, userJobInfoDb);

            if (_userRepository.SaveChanges())
                return Ok();

            throw new Exception("Failed to update user JobInfo");
        }

        throw new Exception("Failed to get user JobInfo");

    }

    [HttpDelete("DeleteUserJobInfo{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        UserJobInfo? userJobInfoDb = _userRepository.GetSingleUserJobInfo(userId);

        if (userJobInfoDb != null)
        {
            _userRepository.RemoveEntity(userJobInfoDb);

            if (_userRepository.SaveChanges())
                return Ok();


            throw new Exception("Failed to delete user JobInfo");
        }


        throw new Exception("Failed to get user JobInfo");
    }
}
