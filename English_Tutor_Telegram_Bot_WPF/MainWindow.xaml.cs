using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace English_Tutor_Telegram_Bot_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Клиент бота
        /// </summary>
        public static Bot Bot { get; set; }

        /// <summary>
        /// Создание главного окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Bot = new Bot("Your token", this);
            Bot.Start();
            ListOfUsers.ItemsSource = Bot.Users; //Определяем ресурс для списка пользователей
        }

        /// <summary>
        /// Показать последние сообщения выбранного пользователя
        /// </summary>
        public void ShowMessages()
        {
            AdditionalInfoStack.Visibility = Visibility.Visible;
            WorkWithUserStack.Visibility = Visibility.Visible;
            ObservableCollection<string> messages = new ObservableCollection<string>();
            messages.Clear();
            User user = (User)ListOfUsers.SelectedItem;
            ListOfMessages.ItemsSource = messages;

            try
            {
                int count = (user.Messages.Count >= 5) ? 5 : user.Messages.Count;

                for (int i = user.Messages.Count - 1; i > user.Messages.Count - 1 - count; i--)
                {
                    if (user.Messages[i].MessageType != "Text")
                    {
                        messages.Add($"***{user.Messages[i].MessageType}***");
                    }
                    else
                    {
                        messages.Add(user.Messages[i].Text);
                    }
                }
            }
            catch { }
        }

        private void ListOfUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowMessages();
        }

        /// <summary>
        /// Очищает форму написания сообщения при нажатии на неё и меняет цвет текста набора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageToSend_GotMouseCapture(object sender, MouseEventArgs e)
        {
            if (MessageToSend.Foreground == Brushes.Black) return;
            MessageToSend.Text = string.Empty;
            MessageToSend.Foreground = Brushes.Black;
        }

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string message = $"🔔🔔🔔🔔🔔🔔🔔🔔🔔\nСообщение от администратора:\n{MessageToSend.Text}";

            Sender.SendTextMessage(long.Parse(IdTextBlock.Text), message);
            MessageBox.Show("Сообщение отправлено", Title, MessageBoxButton.OK, MessageBoxImage.Information);
            MessageToSend.Text = string.Empty;
        }

        /// <summary>
        /// Enter в контексте формы набора сообщения также отправляет его
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageToSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Button_Click(new object(), new RoutedEventArgs());
        }

        /// <summary>
        /// Восстанавливает описательный текст формы набора сообщения при потере фокуса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageToSend_LostFocus(object sender, RoutedEventArgs e)
        {
            MessageToSend.Text = "Тект сообщения";
            MessageToSend.Foreground = Brushes.LightGray;
        }
    }
}
