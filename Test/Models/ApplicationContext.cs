using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.IO;


namespace Test.Models
{
    // лучше в database
    public class ApplicationContext : DbContext
    {
        private IConfigurationRoot builder = null;
        public DbSet<Drink> Drinks { get; set; }

        public ApplicationContext() 
        {
            Database.EnsureCreated();
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            System.Diagnostics.Debugger.Launch();
            ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = "Test.dll.config" };
            string config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None).FilePath;
            /*string fileConfig = Path.GetFileName(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
                    .FilePath);*/
            builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddXmlFile(config).Build();
            optionsBuilder.UseNpgsql(builder["connectionStrings:add:connectionString:connectionString"]);
        }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Drink.DrinkConfiguration());
        }*/
    }

    /*public class SampleContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            if (System.Diagnostics.Debugger.IsAttached == false)
            {
                System.Diagnostics.Debugger.Log(1, null, null);

                try
                {
                    builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddXmlFile("App.config").Build();
                    var h = builder.GetChildren();

                    try
                    {
                        var w = ConfigurationManager.ConnectionStrings["connectionStrings:add:connectionString:connectionString"].ConnectionString;
                    }
                    catch { }

                    try
                    {
                        var w = ConfigurationManager.ConnectionStrings["connectionStrings:add:connectionString:connectionString"];
                    }
                    catch { }

                    builder.Providers.GetEnumerator();
                    var p = builder.GetSection("ConnectionStrings").GetConnectionString("connectionStrings:add:connectionString:connectionString");
                    //var w = builder.GetSection("ConnectionString").GetConnectionString;
                    var t = builder.GetSection("connectionString")["connectionString"];
                    var setting1 = builder["connectionStrings:add:connectionString:connectionString"];
                    var setting2 = builder["connectionStrings:add:connectionString:connectionString"];
                    var e = builder["0"];
                    string str = builder.GetConnectionString("connectionStrings:add:connectionString:connectionString");

                    // получаем строку подключения из файла appsettings.json
                    //string name = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].Name;
                    //var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"];
                    //optionsBuilder.UseNpgsql(connectionString, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));

                }

                catch (Exception ex) { string mes = ex.Message; }

            }
            return new ApplicationContext(optionsBuilder.Options);
        }
    }*/
}
