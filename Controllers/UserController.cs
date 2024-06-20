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
    public IActionResult AddUser(UserDto user)
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
            WHERE UserId ={user.Id}";
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

        throw new Exception("Failed to add user");
    }
}
