using System;
using System.Collections.Generic;
using System.Data;
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
        public MainWindow()
        {
            InitializeComponent();

            _context = new ApplicationContext();
            _service = new DrinkService(_context);
        }

       

        private void dataGrid1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            DataTable changes = ((DataView)dataGrid1.ItemsSource).ToTable().GetChanges();

            foreach (DataRow row in changes.Rows)
            {

            }
            /*dataGrid1.UpdateDB();
            _service.*/
        }

        private async void copyButton_Click(object sender, EventArgs e)
        {
            string drinksEnum = string.Empty;
            List<Drink> drinks = (List<Drink>)dataGrid1.ItemsSource;
            drinks.ForEach(delegate (Drink drink) { drinksEnum += string.Join(",", drink.Id + " " + drink.Name + " " + drink.Quantity + " " + drink.Volume + Environment.NewLine); });
            Clipboard.SetData(DataFormats.Text, drinksEnum);
        }

        private async void insertButton_Click(object sender, EventArgs e)
        { 
            
        }

        private async void updateButton_Click(object sender, RoutedEventArgs e)
        {
            List<Drink> changes = (List<Drink>)dataGrid1.ItemsSource;
            List<Drink> dataFromBase = await _service.GetDrinks();
            bool identicalElem = new HashSet<Drink>(dataFromBase).SetEquals(changes);

            if (!identicalElem)
            {
                changes.ForEach(async delegate(Drink drink) { await _service.UpdateDB(drink);});
                await _context.SaveChangesAsync();
            }
        }

        private async void insertRowButton_Click(object sender, RoutedEventArgs e)
        {

            Drink emptyDrink = new Drink();
            dataGrid1.ClearValue(ItemsControl.ItemsSourceProperty);

            List<Drink> drinks = await _service.GetDrinks();

            foreach (Drink drink in drinks)
            {
                dataGrid1.Items.Add(drink);
            }
            dataGrid1.Items.Add(emptyDrink);
        }

        private async void loadButton_Click(object sender, EventArgs e)
        {
            string path = DialogService.OpenFile("txt");

            if (path != null)
            {
               /* DrinkService.InsertIntoDB(path);
                await DrinkService.SelectDrinks(dataGrid1);*/
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            /*foreach (DataGrid dg in dataGrid1.SelectedCells)
            {
                try
                {
                    var rowDelete = Convert.ToInt32(dg.Cells[0].Value);
                    DrinkService.DeletetDB(myConnectionString, rowDelete);
                    dataGridView1.Rows.Remove(dg);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }*/
        }

        private async void dataGrid1_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid1.Items.Clear();
            dataGrid1.ItemsSource = await _service.GetDrinks();
            dataGrid1.MakeWindowStyle();
        }

        private void dataGrid1_Unloaded(object sender, RoutedEventArgs e)
        {
            _context.Dispose();
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Действие выполнено");
        }

        private void escButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // закрытие окна
        }
    }
}
