using DotNetAPILearn.Models;
using DotNetAPILearn.Data;
using Microsoft.AspNetCore.Mvc;
using DotNetAPILearn.Dtos;

namespace DotNetAPILearn.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper _dapper;
    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        string sql = @"
        SELECT [UserId]
            ,[FirstName]
            ,[LastName]
            ,[Email]
            ,[Gender]
            ,[Active]
        FROM [TutorialAppSchema].[Users]";
        return _dapper.LoadData<User>(sql);
    }
    [HttpGet("GetSingleUsers/{userId}")]
    public User GetSingleUsers(int userId)
    {
        string sql = @"
            SELECT [UserId]
                ,[FirstName]
                ,[LastName]
                ,[Email]
                ,[Gender]
                ,[Active]
            FROM [TutorialAppSchema].[Users]
            WHERE UserId = " + userId.ToString();
        return _dapper.LoadDataSingle<User>(sql);
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        string sql = $@"
            INSERT INTO TutorialAppSchema.Users (
                    FirstName
                    ,LastName
                    ,Email
                    ,Gender
                    ,Active)
            VALUES (
                    '{user.FirstName}'
                    ,'{user.LastName}'
                    ,'{user.Email}'
                    ,'{user.Gender}'
                    ,{user.Active}
                );";
        if (_dapper.ExecuteSql(sql))
            return Ok();

        throw new Exception("Failed to add user");

    }

    [HttpPut("UpdateUser")]
    public IActionResult UpdateUser(User user)
    {
        string sql = $@"
            UPDATE  TutorialAppSchema.Users 
            SET FirstName='{user.FirstName}',
                LastName='{user.LastName}',
                Email='{user.Email}',
                Gender='{user.Gender}',
                Active={user.Active}
            WHERE UserId ={user.UserId}";
        if (_dapper.ExecuteSql(sql))
            return Ok();

        throw new Exception("Failed to update user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = $@"
            DELETE FROM TutorialAppSchema.Users
            WHERE UserId = {userId};";

        if (_dapper.ExecuteSql(sql))
            return Ok();

        throw new Exception("Failed to delete user");
    }


    [HttpGet("GetUsersSalary")]
    public IEnumerable<UserSalary> GetUsersSalary()
    {
        string sql = @"
        SELECT *
        FROM TutorialAppSchema.UserSalary;";
        return _dapper.LoadData<UserSalary>(sql);
    }

    [HttpGet("GetUserSalary/{userId}")]
    public UserSalary GetUserSalary(int userId)
    {
        string sql = $@"
        SELECT *
        FROM TutorialAppSchema.UserSalary
        WHERE UserId = {userId};";

        return _dapper.LoadDataSingle<UserSalary>(sql);
    }

    [HttpPost("AddUserSalary")]
    public IActionResult AddUserSalary(UserSalary userSalary)
    {
        string sql = $@"
        INSERT INTO TutorialAppSchema.UserSalary( UserId , Salary )
        VALUES({userSalary.UserId},{userSalary.Salary}) ;";

        if (_dapper.ExecuteSql(sql))
            return Ok();

        throw new Exception("Failed to add user salary");

    }

    [HttpPut("UpdateUserSalary")]
    public IActionResult UpdateUserSalary(UserSalary userSalary)
    {
        string sql = $@"
        UPDATE TutorialAppSchema.UserSalary
        SET Salary = {userSalary.Salary}
        WHERE UserId = {userSalary.UserId};";

        if (_dapper.ExecuteSql(sql))
            return Ok();

        throw new Exception("Failed to update user salary");

    }

    [HttpDelete("DeleteUserSalary{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        string sql = $@"
        DELETE FROM TutorialAppSchema.UserSalary
        WHERE UserId = {userId};";

        if (_dapper.ExecuteSql(sql))
            return Ok();

        throw new Exception("Failed to delete user salary");
    }







    [HttpGet("GetUsersJobInfo")]
    public IEnumerable<UserJobInfo> GetUsersJobInfo()
    {
        string sql = @"
        SELECT *
        FROM TutorialAppSchema.UserJobInfo;";
        return _dapper.LoadData<UserJobInfo>(sql);
    }

    [HttpGet("GetUserJobInfo/{userId}")]
    public UserJobInfo GetUserJobInfo(int userId)
    {
        string sql = $@"
        SELECT *
        FROM TutorialAppSchema.UserJobInfo
        WHERE UserId = {userId};";

        return _dapper.LoadDataSingle<UserJobInfo>(sql);
    }

    [HttpPost("AddUserJobInfo")]
    public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
    {
        string sql = $@"
        INSERT INTO TutorialAppSchema.UserJobInfo( UserId , JobTitle ,Department  )
        VALUES({userJobInfo.UserId},{userJobInfo.JobTitle},{userJobInfo.Department}) ;";

        if (_dapper.ExecuteSql(sql))
            return Ok();

        throw new Exception("Failed to add user JobInfo");

    }

    [HttpPut("UpdateUserJobInfo")]
    public IActionResult UpdateUserJobInfo(UserJobInfo userJobInfo)
    {
        string sql = $@"
        UPDATE TutorialAppSchema.UserJobInfo
        SET JobTitle = '{userJobInfo.JobTitle}', Department = '{userJobInfo.Department}'
        WHERE UserId = {userJobInfo.UserId};";

        if (_dapper.ExecuteSql(sql))
            return Ok();

        throw new Exception("Failed to update user JobInfo");

    }

    [HttpDelete("DeleteUserJobInfo{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        string sql = $@"
        DELETE FROM TutorialAppSchema.UserJobInfo
        WHERE UserId = {userId};";

        if (_dapper.ExecuteSql(sql))
            return Ok();

        throw new Exception("Failed to delete user JobInfo");
    }

}
