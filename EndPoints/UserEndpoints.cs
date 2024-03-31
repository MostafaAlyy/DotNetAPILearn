using DotNetAPI.Data;

public static class UserEndPoints
{



    public static void MapUserEndPoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("users").WithTags("Minimart API");
        group.MapGet("/users/{name}", (string name) => GetUsers(name));
        group.MapGet("/testConnection", (IConfiguration config) => TestConnection(config));
    }


    public static DateTime TestConnection(IConfiguration config)
    {
        DataContextDapper dataContextDapper = new DataContextDapper(config);

        return dataContextDapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    private static IEnumerable<UserModel> GetUsers(String name)
    {
        return new List<UserModel>
        {
            new UserModel { Id = 1, Name = "John Doe" },
            new UserModel { Id = 2, Name = name },
            new UserModel { Id = 3, Name = "Mostafa Ali" }
        };
    }
}
