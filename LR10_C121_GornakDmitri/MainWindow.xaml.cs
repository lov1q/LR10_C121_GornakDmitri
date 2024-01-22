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
            SearchCB.ItemsSource = new Item[]
            {
                new Item {Name="Финал" },
                new Item {Name="Полуфинал" },
                new Item {Name="Четверть финал" },
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
                MessageBox.Show("Введите название 1 команды!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string teamsecond = secnameTextBox.Text;

            if (string.IsNullOrWhiteSpace(secnameTextBox.Text))
            {
                MessageBox.Show("Введите название 2 команды!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Введите какой стадион вы хотите!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string typeStadion = tipstadionCB.Text;
            if (string.IsNullOrWhiteSpace(tipstadionCB.Text))
            {
                MessageBox.Show("Введите тип стадиона!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string resultmatch = resultmatchCB.Text;
            if (string.IsNullOrWhiteSpace(resultmatchCB.Text))
            {
                MessageBox.Show("Введите резульат матча для 1 команды!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            // Создание нового матча
            var order = new Teams(teamfirst, teamsecond, dateMatch, typematch, stadion, typeStadion, resultmatchCB.Text);

            // Добавление матча в список
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
            teams.Clear();
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.FilterIndex = 0;
            fileDialog.DefaultExt = "json";
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            fileDialog.Title = "Открытие JSON";

            if (fileDialog.ShowDialog() != true) return;
            teams.AddRange(OpenFromJson(fileDialog.FileName));
            TeamsDataGrid.ItemsSource = null;
            TeamsDataGrid.ItemsSource = teams;
        }

        private void SearchB_Click(object sender, RoutedEventArgs e)
        {
            string searchCriteria = SearchCB.Text;
            if (string.IsNullOrWhiteSpace(SearchCB.Text))
            {
                MessageBox.Show("Введите значение для поиска!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string searchValue = SearchCB.Text;

            // Произведите поиск и фильтрацию данных
            List<Teams> filteredTeams = teams.Where(teams => teams.TypeMatch.Contains(searchValue)).ToList();

            // Отобразите отфильтрованные данные в DataGrid
            TeamsDataGrid.ItemsSource = filteredTeams;
        }

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
        //    var application = new Microsoft.Office.Interop.Excel.Application();
        //    application.SheetsInNewWorkbook = 2;
        //    Microsoft.Office.Interop.Excel.Workbook workbook = application.Workbooks.Add(Type.Missing);
        //    application.Worksheets.Item[1].Name = "Список тканей";
        //    application.Worksheets.Item[2].Name = "Итоги";
        //    // работа с рабочим листом
        //    Microsoft.Office.Interop.Excel.Worksheet worksheet = application.Worksheets.Item[1];
        //    // заголовок вверку в объединенных ячейка
        //    Microsoft.Office.Interop.Excel.Range header = worksheet.Range[worksheet.Cells[1][1], worksheet.Cells[4][1]];
        //    header.Merge();
        //    header.Value = "Состояние склада на " + DateTime.Today.ToShortDateString();
        //    header.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //    header.Font.Size = 14;
        //    header.Font.Italic = true;
        //    // названия столбцов
        //    worksheet.Cells[3, 1] = "Название";
        //    worksheet.Cells[3, 2] = "Цена за 1 метр";
        //    worksheet.Cells[3, 3] = "Количество";
        //    worksheet.Cells[3, 4] = "Розничная наценка";
        }

        private void Word_Click(object sender, RoutedEventArgs e)
        {
            // Создаем экземпляр приложения Word
            var wordApp = new Microsoft.Office.Interop.Word.Application();
            wordApp.Visible = true;

            // Создаем новый документ Word
            var document = wordApp.Documents.Add();

            // Добавляем параграф с заголовком
            var headerParagraph = document.Content.Paragraphs.Add();
            headerParagraph.Range.Text = "Соревнования";
            headerParagraph.Range.Font.Size = 16;
            headerParagraph.Range.Font.Bold = 1;
            headerParagraph.Range.InsertParagraphAfter();

            // Добавляем таблицу с данными заказов
            var table = document.Tables.Add(document.Content, teams.Count + 1, 7);
            table.Borders.Enable = 1;

            // Заполняем заголовки столбцов
            table.Cell(1, 1).Range.Text = "Название 1 команды";
            table.Cell(1, 2).Range.Text = "Название 2 команды";
            table.Cell(1, 3).Range.Text = "Дата матча";
            table.Cell(1, 4).Range.Text = "Тип матча";
            table.Cell(1, 5).Range.Text = "Стадион";
            table.Cell(1, 6).Range.Text = "Тип стадиона";
            table.Cell(1, 7).Range.Text = "Очки 1 команды";
            table.Cell(1, 8).Range.Text = "Очки 2 команды";

            // Заполняем данные из списка заказов
            for (int row = 2; row <= teams.Count + 1; row++)
            {
                table.Cell(row, 1).Range.Text = teams[row - 2].Teamfirst;
                table.Cell(row, 2).Range.Text = teams[row - 2].Teamsecond;
                table.Cell(row, 3).Range.Text = teams[row - 2].DateMatch.ToString();
                table.Cell(row, 4).Range.Text = teams[row - 2].TypeMatch;
                table.Cell(row, 5).Range.Text = teams[row - 2].Stadion.ToString();
                table.Cell(row, 6).Range.Text = teams[row - 2].TypeStadion.ToString();
                table.Cell(row, 7).Range.Text = teams[row - 2].TeamfirstPoints.ToString();
                table.Cell(row, 8).Range.Text = teams[row - 2].TeamsecondPoints.ToString();
            }
        }
    }
}