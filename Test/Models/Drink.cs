using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace Test.Models
{
    public class Drink
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public decimal Volume { get; set; }

        internal class DrinkConfiguration : IEntityTypeConfiguration<Drink>
        {
            public void Configure(EntityTypeBuilder<Drink> builder)
            {
                builder.HasData(new Drink[]
                {
                    new Drink { Id = 1, Name = "Coca-Cola", Amount = 33.1m, Quantity = 1, Volume = 0.5m },
                    new Drink { Id = 2, Name = "Fanta", Amount = 1.2m, Quantity = 13, Volume = 1.2m },
                    new Drink { Id = 3, Name = "Yupi", Amount = 47.6m, Quantity = 51, Volume = 0.5m },
                    new Drink { Id = 4, Name = "KateDrink", Amount = 47.3m, Quantity = 51, Volume = 0.5m }
                });

                builder.HasIndex(x => new { x.Name, x.Volume}).IsUnique();
                builder.HasIndex(x => new { x.Name });
            }
        }
    }
}
