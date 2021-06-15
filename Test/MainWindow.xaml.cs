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
                Regex reg = new Regex(@"^[0-9][0-9\.]+\d$");
                Match match = reg.Match(((TextBox)e.EditingElement).Text);
                if (string.IsNullOrEmpty(match.Value) && !((TextBox)e.EditingElement).Text.Equals("0"))
                {
                    e.Cancel = true;
                    ((TextBox)e.EditingElement).Text = "0";
                    MessageBox.Show(@"Неверный формат ячейки. Необходимо поставить '.'");
                }
            }
            if (e.Column.Header.Equals("Количество"))
            {
                Regex reg = new Regex(@"(?<![-.])\b[0-9]+\b(?!\.[0-9])");
                Match match = reg.Match(((TextBox)e.EditingElement).Text);
                if (string.IsNullOrEmpty(match.Value))
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
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool saveSuccess = await _service.SaveDbAsync(drinkCollection);
            if (saveSuccess)
            {
                MessageBox.Show("Данные сохранены");
                List<string> drinkSelected = _service.GetFiltredDrinks(drinkCollection);
                drinkFilter.ItemsSource = drinkSelected;
                drinkFilter.SelectedItem = drinkSelected[drinkSelected.Count - 1];
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
            List<string> drinkSelected = _service.GetFiltredDrinks(drinkCollection);
            drinkFilter.ItemsSource = drinkSelected;
            drinkFilter.SelectedItem = drinkSelected[drinkSelected.Count - 1];
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
            dataGrid1.ItemsSource = _service.GetSearchedDrinks(searchFilter.Text);
        }

        private async void ActiveFilter(object sender, RoutedEventArgs e)
        {
            drinkCollection = await _service.GetDrinksAsync();

            if (drinkFilter.SelectedItem != null)
            {
                dataGrid1.ItemsSource = drinkFilter.SelectedItem.Equals("Все") ?  drinkCollection.ToList() : drinkCollection.ToList().FindAll(x => x.Name.Equals(drinkFilter.SelectedItem));
            }
        }

        private async void dataGrid1_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid1.Items.Clear();
            drinkFilter.Items.Clear();
            drinkCollection = await _service.GetDrinksAsync();
            dataGrid1.ItemsSource = drinkCollection;
            dataGrid1.MakeWindowStyle();
            List<string> drinkSelected = _service.GetFiltredDrinks(drinkCollection);
            drinkFilter.ItemsSource = drinkSelected;
            drinkFilter.SelectedItem = drinkSelected[drinkSelected.Count - 1];
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
