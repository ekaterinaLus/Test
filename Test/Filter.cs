using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Models;

namespace Test
{
    public static class Filter
    {
        public static List<Drink> GetFiltredResult(this ObservableCollection<Drink> drinkCollection, string drinkItem, string amountItem)
        {
            List<string> drinkSelected = GetFiltredDrinks(drinkCollection);
            List<string> amountSelected = GetFiltredAmount(drinkCollection);

            if (!drinkItem.Equals(drinkSelected[drinkSelected.Count - 1])
                    && amountItem.Equals(amountSelected[amountSelected.Count - 1]))
            {
                return drinkCollection.ToList().FindAll(x => x.Name.Equals(drinkItem));
            }
            else if (drinkItem.Equals(drinkSelected[drinkSelected.Count - 1])
                && !amountItem.Equals(amountSelected[amountSelected.Count - 1]))
            {
                return drinkCollection.ToList().FindAll(x => x.Amount.Equals(Convert.ToDecimal(amountItem)));
            }
            else if (!drinkItem.Equals(drinkSelected[drinkSelected.Count - 1])
                && !amountItem.Equals(amountSelected[amountSelected.Count - 1]))
            {
                return drinkCollection.ToList().FindAll(x => x.Name.Equals(drinkItem) && x.Amount.Equals(Convert.ToDecimal(amountItem)));
            }

            return drinkCollection.ToList();
        }

        public static List<string> GetFiltredDrinks(this ObservableCollection<Drink> drinkCollection)
        {
            List<string> drinkSelected = new List<string>(drinkCollection.Select(x => x.Name).Distinct());
            drinkSelected.Add("Все напитки");
            return drinkSelected;
        }

        public static List<string> GetFiltredAmount(this ObservableCollection<Drink> drinkCollection)
        {
            List<string> amountSelected = new List<string>(drinkCollection.Select(x => x.Amount.ToString()).Distinct());
            amountSelected.Add("Все цены");
            return amountSelected;
        }

        public static List<Drink> GetSearchedDrinks(this ApplicationContext _context, string filter, string search)
        {
            List<Drink> drinkSearched = new List<Drink>();
            bool success = false;
            decimal numberDec;
            int number;
            search = search.Contains(".") ? search.Replace('.', ',') : search;
            success = Decimal.TryParse(search, out numberDec);

            switch (filter)
            {
                case "Name":
                    drinkSearched = _context.Drinks.ToList().Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                    break;
                case "Amount":
                    drinkSearched = _context.Drinks.ToList().Where(x => x.Amount == Convert.ToDecimal(numberDec)).ToList();
                    break;
                case "Quantity":
                    success = Int32.TryParse(search, out number);
                    drinkSearched = _context.Drinks.ToList().Where(x => x.Quantity == number).ToList();
                    break;
                case "Volume":
                    drinkSearched = _context.Drinks.ToList().Where(x => x.Volume == Convert.ToDecimal(numberDec)).ToList();
                    break;
            }

            return drinkSearched;
        }
    }
}
