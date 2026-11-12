using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using RegexSearch.Core.Services; // Подключаем нашу логику из второго проекта

namespace RegexSearch.WPF
{
    public partial class MainWindow : Window
    {
        // Создаем экземпляр нашего сервиса (где лежат алгоритмы поиска)
        private readonly RecognitionService _service = new RecognitionService();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Кнопка "Поиск через Regex"
        private void RegexButton_Click(object sender, RoutedEventArgs e)
        {
            var results = _service.SearchByRegex(InputText.Text);
            ResultsGrid.ItemsSource = results;
            StatusText.Text = $"Режим Regex: найдено {results.Count} совпадений.";
            StatusText.Foreground = System.Windows.Media.Brushes.Blue;
        }

        // Кнопка "Поиск через автомат"
        private void AutomatonButton_Click(object sender, RoutedEventArgs e)
        {
            var results = _service.SearchByAutomaton(InputText.Text);
            ResultsGrid.ItemsSource = results;
            StatusText.Text = $"Режим Автомата: найдено {results.Count} совпадений.";
            StatusText.Foreground = System.Windows.Media.Brushes.DarkGreen;
        }

        // Кнопка "Сравнить алгоритмы"
        private void CompareButton_Click(object sender, RoutedEventArgs e)
        {
            // Запускаем оба алгоритма
            var regexResults = _service.SearchByRegex(InputText.Text);
            var autoResults = _service.SearchByAutomaton(InputText.Text);

            // Показываем в таблице результаты регулярки (для наглядности)
            ResultsGrid.ItemsSource = regexResults;

            // Сравниваем количество найденных совпадений
            if (regexResults.Count != autoResults.Count)
            {
                StatusText.Text = $"ОШИБКА! Разное количество: Regex={regexResults.Count}, Автомат={autoResults.Count}";
                StatusText.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            // Сравниваем каждое совпадение по очереди
            for (int k = 0; k < regexResults.Count; k++)
            {
                if (regexResults[k].Value != autoResults[k].Value ||
                    regexResults[k].Position != autoResults[k].Position)
                {
                    StatusText.Text = $"ОШИБКА! Совпадения №{k + 1} отличаются.";
                    StatusText.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }
            }

            // Если дошли сюда — всё совпало
            StatusText.Text = $"УСПЕХ! Алгоритмы полностью совпали. Найдено {regexResults.Count} совпадений.";
            StatusText.Foreground = System.Windows.Media.Brushes.Purple;
        }

        // Кнопка "Очистить"
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            InputText.Text = "";
            ResultsGrid.ItemsSource = null;
            StatusText.Text = "Поле очищено.";
            StatusText.Foreground = System.Windows.Media.Brushes.Gray;
        }
    }
}