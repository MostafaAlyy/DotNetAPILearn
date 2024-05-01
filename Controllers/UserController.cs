using DotnetAPI.Models;
using DotNetAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

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
}
