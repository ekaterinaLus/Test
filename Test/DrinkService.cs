using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Test.Models;

namespace Test
{
    //1) класс сделать не статическим - сделать его сервисом
    //2) убрать лишние методы
    public class DrinkService
    {
        private readonly ApplicationContext _context;
        public DrinkService(ApplicationContext context)
        {
            _context = context;
        }
        //Task > Task<List<Drink>>
        public async Task<List<Drink>> GetDrinks()
        {
            //минимализирровать создание контекста - прокидывт ь через конструктор
            return await _context.Drinks
                .ToListAsync();
        }

        public async Task DeletetDB(string id)
        {
            try
            {
                Drink drink = _context.Drinks.Where(x => x.Id.ToString().Equals(id)).FirstOrDefault();
                _context.Drinks.Remove(drink);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task InsertIntoDB(Drink drink)
        {
            try
            {
                _context.Drinks.Add(new Drink { Name = drink.Name, Amount = drink.Amount, Quantity = drink.Quantity, Volume = drink.Volume});
                await _context.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task  UpdateDB(Drink drink)
        {
            try
            {
                _context.Drinks.Update(drink);
                await _context.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
