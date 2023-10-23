using DbUp;

namespace WebApplication.Data;

public class DatabaseMigrator
{
    private readonly IConfiguration _configuration;

    public DatabaseMigrator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void MigrateDatabase()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        var migrationScriptsPath = 
            "C:\\Users\\Sergey\\Documents\\Github\\webNET-Hits-backend-aspnet-project-1\\WebApplication\\Scripts\\Migrations";

        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsFromFileSystem(migrationScriptsPath)
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            throw new Exception("Error performing migrations: " + result.Error);
        }
    }
}