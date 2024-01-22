using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LR10_C121_GornakDmitri
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Stock teams;
        public MainWindow()
        {
            // Инициализация списка заказов
            
            InitializeComponent();
            teams = new Stock();

            // Привязка списка заказов к DataGrid

            TeamsDataGrid.ItemsSource = teams;
            typematchCB.ItemsSource = new Item[]
            {
                new Item {Name="Финал" },
                new Item {Name="Полуфинал" },
                new Item {Name="Четверть финал" },
            };
            stadionCB.ItemsSource = new Item[]
            {
                new Item {Name="Камп Ноу" },
                new Item {Name="Соккер Сити" },
                new Item {Name="Олимпийский" },
            };
            tipstadionCB.ItemsSource = new Item[]
            {
                new Item {Name="Открытый" },
                new Item {Name="Закрытый" },
            };
            resultmatchCB.ItemsSource = new Item[]
            {
                new Item {Name="Победа" },
                new Item {Name="Поражение" },
                new Item {Name="Ничья" },
            };
        }
        public class Item
        {
            public string Name { get; set; }
            public override string ToString() => $"{Name}";
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void JsonSave_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddTeams_Click(object sender, RoutedEventArgs e)
        {
            // Получение выбранных значений из элементов управления
            string teamfirst = frsnameTextBox.Text;

            if (string.IsNullOrWhiteSpace(frsnameTextBox.Text))
            {
                MessageBox.Show("Please enter a product name.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string teamsecond = secnameTextBox.Text;

            if (string.IsNullOrWhiteSpace(secnameTextBox.Text))
            {
                MessageBox.Show("Please enter a product name.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime? dateMatch = datematchDP.SelectedDate;  // Проверяем, что дата выбрана


            string typematch = typematchCB.Text;
            if (string.IsNullOrWhiteSpace(typematchCB.Text))
            {
                MessageBox.Show("Введите тип матча!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string stadion = stadionCB.Text;
            if (string.IsNullOrWhiteSpace(stadionCB.Text))
            {
                MessageBox.Show("Введите тип матча!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string typeStadion = tipstadionCB.Text;
            if (string.IsNullOrWhiteSpace(tipstadionCB.Text))
            {
                MessageBox.Show("Введите тип матча!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string resultmatch = resultmatchCB.Text;
            if (string.IsNullOrWhiteSpace(resultmatchCB.Text))
            {
                MessageBox.Show("Введите тип матча!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            // Создание нового матча
            var order = new Teams(teamfirst, teamsecond, dateMatch, typematch, stadion, typeStadion, resultmatchCB.Text);

            // Добавление заказа в список
            teams.Add(order);

            // Обновление источника данных для DataGrid
            TeamsDataGrid.ItemsSource = null;
            TeamsDataGrid.ItemsSource = teams;

            // Очистка полей ввода
            ClearFields();
        }

        private void ClearFields()
        {
            typematchCB.SelectedItem = null;
            stadionCB.SelectedItem = null;
            frsnameTextBox.Clear();
            tipstadionCB.SelectedItem = null;
            secnameTextBox.Clear();
            datematchDP.SelectedDate = null;
            resultmatchCB.SelectedIndex = -1;
        }
        private void ClearFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            typematchCB.SelectedItem = null;
            stadionCB.SelectedItem = null;
            frsnameTextBox.Clear();
            tipstadionCB.SelectedItem = null;
            secnameTextBox.Clear();
            datematchDP.SelectedDate = null;
            resultmatchCB.SelectedIndex = -1;
        }
        public void SaveToJson(string filename)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            File.WriteAllText(filename, JsonSerializer.Serialize(teams, options));
        }

        public Stock OpenFromJson(string filename)
        {
            return JsonSerializer.Deserialize<Stock>(File.ReadAllText(filename));
        }

        private void XMLSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "json-файл (.json)|.json|Текстовые файлы (.txt)|.txt|Все файлы (.)|.";
            fileDialog.FilterIndex = 0;
            fileDialog.DefaultExt = "json";
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            fileDialog.Title = "Сохранить в JSON";
            fileDialog.OverwritePrompt = true;

            if (fileDialog.ShowDialog() != true) return;
            SaveToJson(fileDialog.FileName);
        }

        private void XMLOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.FilterIndex = 0;
            fileDialog.DefaultExt = "json";
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            fileDialog.Title = "Открытие JSON";

            if (fileDialog.ShowDialog() != true) return;
            OpenFromJson(fileDialog.FileName);
        }
    }
}