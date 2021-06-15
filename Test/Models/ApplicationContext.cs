using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.IO;


namespace Test.Models
{
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
            ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = "Test.dll.config" };
            string config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None).FilePath;
            builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddXmlFile(config).Build();
            optionsBuilder.UseNpgsql(builder["connectionStrings:add:connectionString:connectionString"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Drink.DrinkConfiguration());
        }
    }
}
