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

        /*internal class DrinkConfiguration : IEntityTypeConfiguration<Drink>
        {
            public void Configure(EntityTypeBuilder<Drink> builder)
            {
                builder.HasData(new Drink[]
                {
                    new Drink { Name = "Coca-Cola", Amount = 33, Quantity = 1, Volume = 0.5m },
                    new Drink { Name = "Fanta", Amount = 21, Quantity = 13, Volume = 1m },
                    new Drink { Name = "Yupi", Amount = 47, Quantity = 51, Volume = 0.5m },
                    new Drink { Name = "KateDrink", Amount = 47, Quantity = 51, Volume = 0.5m }
                });
            }
        }*/
    }
}
