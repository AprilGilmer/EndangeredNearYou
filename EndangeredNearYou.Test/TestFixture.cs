using Dapper;
using EndangeredNearYou.Domain.Interfaces;
using EndangeredNearYou.Domain.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace EndangeredNearYou.Test
{
    public class TestFixture : IDisposable
    {
        public ServiceProvider ServiceProvider { get; }
        private readonly SqliteConnection _connection;

        public TestFixture()
        {
            var services = new ServiceCollection();

            // Create one shared SQLite in-memory connection
            _connection = new SqliteConnection("Data Source=:memory:;Cache=Shared");
            // Cache=Shared - Allows multiple connections to access the same in-memory DB (important if repositories open new connections).
            // :memory: - Tells SQLite to create a temporary, in-memory database.
            _connection.Open();

            // Create schema and seed data
            InitializeDatabase(_connection);

            // Register dependencies
            // Keeps one open connection for all tests (otherwise your DB disappears after each test).
            services.AddSingleton<IDbConnection>(_connection); // Use the same open connection
            services.AddScoped<ILocationRepository, LocationRepository>();

            ServiceProvider = services.BuildServiceProvider();
        }

        // Runs schema + seed data setup once for all tests.
        private void InitializeDatabase(IDbConnection connection)
        {
            var sql = @"
               CREATE TABLE country_continent_codes (
                    Country_Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Continent_Name TEXT,
                    Continent_Code TEXT,
                    Country_Name TEXT,
                    Two_Letter_Country_Code TEXT,
                    Three_Letter_Country_Code TEXT,
                    Country_Number INTEGER,
                    UNIQUE (Country_Id)
                );

                INSERT INTO country_continent_codes VALUES 
                    (1, 'North America', 'NA', 'United States of America', 'US', 'USA', 840),
                    (2, 'Europe', 'EU', 'Albania, Republic of', 'AL', 'ALB', 8), 
                    (3, 'Oceania',	'OC', 'American Samoa', 'AS', 'ASM', 16), 
                    (4, 'Antarctica', 'AN', 'Antarctica (the territory South of 60 deg S)', 'AQ', 'ATA', 10);


                CREATE TABLE world_cities (
                    City_Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Country_Code TEXT,
                    State TEXT,
                    County TEXT,
                    Name TEXT,
                    Latitude REAL,
                    Longitude REAL,
                    UNIQUE (City_Id)
                );

                INSERT INTO world_cities VALUES 
                    (1, 'US', 'Alabama', 'Baldwin County', 'Orange Beach', 30.294370, -87.573590),
                    (2, 'US', 'Florida', 'Miami-Dade County', 'Bay Harbor Islands', 25.887590, -80.131160),
                    (3, 'US', 'California', 'Los Angeles County', 'Century City', 34.055570, -118.417860),
                    (4, 'US', 'New York', 'Columbia County', 'Chatham', 42.364250, -73.594840),
                    (5, 'US', 'Texas', 'El Paso County', 'El Paso', 31.758720, -106.486930),
                    (6, 'AL', 'Southern Albania', 'Korce County', 'Pirg', 40.785000, 20.706110),
                    (7, 'AL', 'Southern Albania', 'Gjirokastër County', 'Permet', 40.233610, 20.351670),
                    (8, 'AL', 'Central Albania', 'Elbasan County', 'Lenias', 40.766670, 20.391390),
                    (9, 'AL', 'Northern Albania', 'Shkodër County', 'Rrape', 42.044440, 19.970560),
                    (10, 'AS', 'American Samoa', 'Western District', 'Vaitogi', -14.352590, -170.737960), 
                    (11, 'AS', 'American Samoa', 'Eastern District', 'Nu‘uuli', -14.315830, -170.697500),
                    (12, 'AQ', NULL, NULL, 'Amundsen-Scott South Pole Station', -85.100000, 0.000000),
                    (13, 'AQ', NULL, NULL, 'McMurdo Station', -77.845500, 166.669800);
            ";

            connection.Execute(sql);
        }

        // Ensures SQLite connection closes after tests.
        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}