using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Test.Models;

namespace Test
{
    public class DrinkService
    {
        private readonly ApplicationContext _context;
        public DrinkService(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<ObservableCollection<Drink>> GetDrinksAsync()
        {
            return new ObservableCollection<Drink>(await _context.Drinks.ToListAsync());
        }

        public async Task DeleteDrinkAsync(Drink drink)
        {
            try
            {
                if (_context.Drinks.Contains(drink))
                {
                    _context.Drinks.Remove(drink);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task DeleteAllDrinksAsync()
        {
            try
            {
                List<Drink> drinksRemove = await _context.Drinks.ToListAsync();
                _context.Drinks.RemoveRange(drinksRemove);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task  UpdateDbAsync(Drink drink)
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

        public async Task<bool> SaveDbAsync(ObservableCollection<Drink> drinkCollection)
        {
            try 
            {
                foreach (Drink drink in drinkCollection)
                {
                    if (_context.Drinks.Find(drink.Id) == null && !string.IsNullOrEmpty(drink.Name) && drink.Quantity != 0
                        && drink.Volume != 0m && drink.Amount != 0m)
                    {
                        _context.Drinks.Add(drink);
                    }
                    else if (string.IsNullOrEmpty(drink.Name) || drink.Quantity == 0
                        || drink.Volume == Decimal.Zero || drink.Amount == Decimal.Zero)
                    {
                        return false;
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
