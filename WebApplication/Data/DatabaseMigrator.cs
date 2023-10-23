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
        var migrationScriptsPath = _configuration.GetConnectionString("MigrationScripsPath");

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