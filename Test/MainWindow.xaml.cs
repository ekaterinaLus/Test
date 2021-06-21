using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Test.Models;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationContext _context;
        private readonly DrinkService _service;
        ObservableCollection<Drink> drinkCollection = null;
        public MainWindow()
        {
            InitializeComponent();

            _context = new ApplicationContext();
            _service = new DrinkService(_context);
        }

        private void dataGrid_PreviewCellEditing(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.Equals("Объем") || e.Column.Header.Equals("Цена")) 
            {
                if (!((TextBox)e.EditingElement).Text.CheckForDecimal() && !((TextBox)e.EditingElement).Text.Equals("0"))
                {
                    e.Cancel = true;
                    ((TextBox)e.EditingElement).Text = "0";
                    MessageBox.Show(@"Неверный формат ячейки. Необходимо поставить '.'");
                }
            }
            if (e.Column.Header.Equals("Количество"))
            {
                if (!((TextBox)e.EditingElement).Text.CheckForInt())
                {
                    e.Cancel = true;
                    ((TextBox)e.EditingElement).Text = "0";
                    MessageBox.Show("Неверный формат ячейки. Используется только целое число");
                }
            }
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            dataGrid1.ItemsSource = await _service.GetDrinksAsync();
            InstallFilter();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            drinkCollection = (ObservableCollection<Drink>)dataGrid1.Items.SourceCollection;
            bool saveSuccess = await _service.SaveDbAsync(drinkCollection);

            if (saveSuccess)
            {
                MessageBox.Show("Данные сохранены");
                InstallFilter();
            }
            else
            {
                MessageBox.Show("Данные не сохранены, потому что имеют неверный формат либо пусты. Все поля должны быть заполнены");
            }
        }

        private async void InsertRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (drinkCollection == null)
            {
                drinkCollection = await _service.GetDrinksAsync();
                dataGrid1.ItemsSource = drinkCollection;
            }
            drinkCollection.Add(new Drink());
            dataGrid1.ItemsSource = drinkCollection;
        }

        private void CopyRowButton_Click(object sender, EventArgs e)
        { 
            string drinksEnum = string.Empty;
            var drinks = dataGrid1.SelectedItems;

            foreach (Drink drink in drinks)
            {
                drinksEnum += string.Join(",", drink.Id + " " + drink.Name + " " + drink.Quantity + " " + drink.Volume + Environment.NewLine);
            }
            Clipboard.SetData(DataFormats.Text, drinksEnum);
        }

        private async void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            var drinks = dataGrid1.SelectedItems;
            ObservableCollection<Drink> drinkRemove = new ObservableCollection<Drink>(drinkCollection);

            foreach (Drink drink in drinks)
            {
               await _service.DeleteDrinkAsync(drink);
                drinkRemove.Remove(drink);
            }
            dataGrid1.ItemsSource = drinkRemove;
            drinkCollection = drinkRemove;
            InstallFilter();
        }

        private async void CutRowButton_Click(object sender, RoutedEventArgs e)
        {
            string drinksEnum = string.Empty;
            var drinks = dataGrid1.SelectedItems;
            ObservableCollection <Drink> drinkRemove = new ObservableCollection<Drink>(await _service.GetDrinksAsync());

            foreach (Drink drink in drinks)
            {
                drinkRemove.Remove(drink);
            }

            foreach (Drink drink in drinks)
            {
                drinksEnum += string.Join(",", drink.Id + " " + drink.Name + " " + drink.Quantity + " " + drink.Volume + Environment.NewLine);
            }
            Clipboard.SetData(DataFormats.Text, drinksEnum);
            dataGrid1.ItemsSource = drinkRemove;
        }

        private async void DeleteAllButton_Click(object sender, RoutedEventArgs e)
        {
            string sMessageBoxText = "При выполнении даннйо операции все данные из базы данных удаляются. Продолжить?";
            string sCaption = "Предупреждение";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    await _service.DeleteAllDrinksAsync();
                    break;
                case MessageBoxResult.No:
                    break;
            }

            dataGrid1.ItemsSource = await _service.GetDrinksAsync();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridColumn column = dataGrid1.Columns.Where(x => x.Header.Equals(searchFilter.SelectedItem.ToString())).FirstOrDefault();
            dataGrid1.ItemsSource = _context.GetSearchedDrinks(column.SortMemberPath, textboxSearch.Text);
        }

        private async void ActiveFilter(object sender, RoutedEventArgs e)
        {
            drinkCollection = await _service.GetDrinksAsync();

            if (drinkFilter.SelectedItem != null && amountFilter.SelectedItem != null)
            {
                dataGrid1.ItemsSource = drinkCollection.
                    GetFiltredResult(drinkFilter.SelectedItem.ToString(), amountFilter.SelectedItem.ToString());
            }
        }

        public void InstallFilter()
        {
            List<string> drinkSelected = drinkCollection.GetFiltredDrinks();
            List<string> amountSelected = drinkCollection.GetFiltredAmount();
            List<string> searchSelected = dataGrid1.Columns.Select(x => x.Header.ToString()).ToList();
            drinkFilter.ItemsSource = drinkSelected;
            drinkFilter.SelectedItem = drinkSelected[drinkSelected.Count - 1];
            amountFilter.ItemsSource = amountSelected;
            amountFilter.SelectedItem = amountSelected[amountSelected.Count - 1];
            searchFilter.ItemsSource = searchSelected;
            searchFilter.SelectedItem = searchSelected[0];
        }

        private async void dataGrid1_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid1.Items.Clear();
            drinkFilter.Items.Clear();
            drinkCollection = await _service.GetDrinksAsync();
            dataGrid1.ItemsSource = drinkCollection;
            dataGrid1.MakeWindowStyle();
            InstallFilter();

            /*_context.Drinks.Add(new Drink { Id = 1, Name = "Coca-Cola", Amount = 33.1m, Quantity = 1, Volume = 0.5m });
            _context.Drinks.Add(new Drink { Id = 2, Name = "Fanta", Amount = 1.2m, Quantity = 13, Volume = 1.2m });
            _context.Drinks.Add(new Drink { Id = 3, Name = "Yupi", Amount = 47.6m, Quantity = 51, Volume = 0.5m });
            _context.Drinks.Add(new Drink { Id = 4, Name = "KateDrink", Amount = 47.3m, Quantity = 51, Volume = 0.5m });
            _context.SaveChanges();*/
        }

        private void dataGrid1_Unloaded(object sender, RoutedEventArgs e)
        {
            _context.Dispose();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
