using DotNetAPILearn.Models;
using DotNetAPILearn.Data;

public static class UserEndPoints
{



    public static void MapUserEndPoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("users").WithTags("Minimal API");
        group.MapGet("/users", (IConfiguration config) => GetUsers(config));
        group.MapGet("/user", (IConfiguration config, int userId) => GetSingleUsers(config, userId));
        group.MapPut("/updateUser", (IConfiguration config, User user) => UpdateUser(config, user));
        group.MapGet("/testConnection", (IConfiguration config) => TestConnection(config));
    }


    public static DateTime TestConnection(IConfiguration config)
    {
        DataContextDapper dataContextDapper = new DataContextDapper(config);

        return dataContextDapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    public static IEnumerable<User> GetUsers(IConfiguration config)
    {
        DataContextDapper dataContextDapper = new DataContextDapper(config);
        string sql = @"
        SELECT [UserId]
            ,[FirstName]
            ,[LastName]
            ,[Email]
            ,[Gender]
            ,[Active]
        FROM [TutorialAppSchema].[Users]";
        return dataContextDapper.LoadData<User>(sql);

    }

    public static User GetSingleUsers(IConfiguration config, int userId)
    {
        DataContextDapper dataContextDapper = new DataContextDapper(config);

        string sql = @"
            SELECT [UserId]
                ,[FirstName]
                ,[LastName]
                ,[Email]
                ,[Gender]
                ,[Active]
            FROM [TutorialAppSchema].[Users]
            WHERE UserId = " + userId.ToString();
        return dataContextDapper.LoadDataSingle<User>(sql);
    }

    public static void UpdateUser(IConfiguration config, User user)
    {
        DataContextDapper dataContextDapper = new DataContextDapper(config);
        string sql = $@"
            UPDATE  TutorialAppSchema.Users 
            SET FirstName='{user.FirstName}',
                LastName='{user.LastName}',
                Email='{user.Email}',
                Gender='{user.Gender}',
                Active={user.Active}
            WHERE UserId ={user.UserId}";

        if (!dataContextDapper.ExecuteSql(sql))
            throw new Exception("Failed to add user");
    }
}
