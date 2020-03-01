using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace English_Tutor_Telegram_Bot_WPF
{
    /// <summary>
    /// Непосредственно бот
    /// </summary>
    public class Bot
    {
        /// <summary>
        /// Создание бота
        /// </summary>
        /// <param name="Token">Токен</param>
        public Bot(string Token, MainWindow MW)
        {
            this.Token = Token;
            this.MW = MW;
            Users = new ObservableCollection<User>();
        }

        public Bot() { }

        /// <summary>
        /// Токен
        /// </summary>
        private string Token { get; set; }

        /// <summary>
        /// Клиент телеграм-бота
        /// </summary>
        public static TelegramBotClient TelBot { get; set; }

        public MainWindow MW { get; set; }

        /// <summary>
        /// Клваиатура функций
        /// </summary>
        public static ReplyKeyboardMarkup Rkm { get; set; }

        /// <summary>
        /// Пользователи, которые когда-либо обращались к боту
        /// </summary>
        public ObservableCollection<User> Users { get; set; }

        /// <summary>
        /// Приветствие нового пользователя
        /// </summary>
        private readonly string Greeting = "Приветствую тебя, желающий изучать английский язык!" +
                                  $"{Environment.NewLine}Этот бот предназначен для того, чтобы тебе помочь в этом деле." +
                                  $"{Environment.NewLine}Вот, что он умеет: ";

        /// <summary>
        /// Функции бота
        /// </summary>
        public static readonly string Options = "* Укажите название времени, чтобы получить разъяснения с примерами." +
                                 $"{Environment.NewLine}Для примера: /Present_Simple." +
                                 $"{Environment.NewLine}* /word - Получить рандомное английское слово" +
                                 $"{Environment.NewLine}* $add+word+слово - Добавить новое слово к себе в словарь. Знаки '$' и '-' обязательны." +
                                 $"{Environment.NewLine}* /show_my_words - Показать все сохранённые слова" +
                                 $"{Environment.NewLine}* /check_my_words - Тренировка слов" +
                                 $"{Environment.NewLine}* /help - Вывести это сообщение ещё раз";

        private readonly Dictionary<string, string> RulesPictures = new Dictionary<string, string>
        {
            {"/Present_Simple".ToLower(), @"https://sun9-3.userapi.com/c857732/v857732380/15816b/t5tiWjLpCXA.jpg"},
            {"/Present_Continuous".ToLower(), @"https://sun9-71.userapi.com/c205616/v205616954/4e593/KtwSA_9bWkY.jpg"},
            {"/Future_Simple".ToLower(), @"https://sun9-8.userapi.com/c857324/v857324690/cc0fb/_9XK9wdZ3fU.jpg"}
        };

        /// <summary>
        /// Запускает бот и начинает принмать сообщения
        /// </summary>
        public void Start()
        {
            Data.Load();
            TelBot = new TelegramBotClient(Token);

            foreach (User user in Users)
            {
                user.Training = new Training(user, Training.AmountOfQuestions);
                user.LearningMode = false;
            }

            SetUpKeyboard();
            TelBot.OnMessage += MessageListener;
            TelBot.StartReceiving();
        }

        private void TelBot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Приём сообщений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageListener(object sender, MessageEventArgs e)
        {
            bool newUser = false;

            string FirstName = e.Message.Chat.FirstName;
            string text = e.Message.Text;
            long id = e.Message.Chat.Id;
            string messageType = e.Message.Type.ToString();

            //Проверка на нового пользователя
            var selectedUser = from user in Users where user.ChatId == id select user;
            User tempUser = selectedUser.FirstOrDefault();

            if (tempUser == null)
            {
                MW.Dispatcher.Invoke(() =>
                {
                    Users.Add(new User(FirstName, id));
                });

                tempUser = Users.Last();
                newUser = true;
            }

            User currentUser = tempUser;
            currentUser.Messages.Add(new Message(text, messageType, id));
            currentUser.NumberOfMessages++;

            MW.Dispatcher.Invoke(() =>
            {
                MW.ListOfUsers.Items.Refresh();

                if (currentUser.Messages.Count > 1)
                {
                    if (MW.ListOfUsers.SelectedItem != null)
                    MW.ShowMessages();
                }
            });

            if (currentUser.Training == null) currentUser.Training = new Training(currentUser, Training.AmountOfQuestions);

            if (messageType == "Text")
            {
                if (text.ToLower() == "/start")
                {
                    if (newUser)
                    {
                        Sender.SendTextMessage(id, Greeting);
                        Sender.SendOptionsKeyboard(id);
                    }
                    else
                    {
                        Sender.SendOptionsKeyboard(id);
                    }
                }
                else if (text.ToLower() == "привет" || text.ToLower() == "hello") Sender.SendTextMessage(id, "Hello!");
                else if (text.ToLower() == "/word")
                {
                    Random r = new Random();
                    var keys = Dictionary.Words.Keys;
                    List<string> words = new List<string>();

                    foreach (var key in keys)
                    {
                        words.Add(key);
                    }

                    string word = words[r.Next(words.Count - 1)];

                    Sender.SendTextMessage(id, word);
                    Sender.SendTextMessage(id, Dictionary.Words[word]);

                    Sender.SendTextMessage(id, "Вы можете сохранить данное слово в вашем словаре," +
                                               " используя команду добавления слова: ");

                    Sender.SendTextMessage(id, "$add+word+слово");
                }
                else if (text.ToLower().StartsWith("$"))
                {
                    string[] test = text.ToLower().Split('+');

                    if (test.Length == 3)
                    {
                        if (test[0] == "$add")
                        {
                            Word.Add(id, new Word(test[1], test[2]));
                            Sender.SendTextMessage(id, "Успешно");
                        }
                    }
                }
                else if (text.ToLower() == "/show_my_words")
                {
                    if (currentUser.Words.Count >= 1)
                    {
                        int counter = 1;

                        StringBuilder sb = new StringBuilder();
                        foreach (Word word in currentUser.Words)
                        {
                            sb.Append($"{counter++}. {word._Word} - {word.Translation} [{word.LearningProgress}]\n");
                        }

                        Sender.SendTextMessage(id, sb.ToString());
                    }
                    else
                    {
                        Sender.SendTextMessage(id, "У вас нет слов!");
                    }
                }
                else if (text.ToLower() == "/check_my_words")
                {
                    if (currentUser.Words.Count >= 5) currentUser.Training.CheckWords();
                    else Sender.SendTextMessage(id, $"Для тренировки слов необходимо минимум 5 слов в словаре");
                }
                else if (currentUser.LearningMode)
                {
                    if (text.ToLower() == currentUser.Training.CorrectAnswer)
                    {
                        Sender.SendTextMessage(id, "Correct!");
                        Thread.Sleep(100);
                        currentUser.Words[currentUser.Words.IndexOf(currentUser.Training.WordObject)].LearningProgress++;
                        currentUser.Training.Words.RemoveAt(currentUser.Training.WordIndex);
                        currentUser.Training.Counter--;
                        if (currentUser.Training.Counter == 0)
                        {
                            currentUser.LearningMode = false;
                            currentUser.Training = new Training(currentUser, Training.AmountOfQuestions);
                        }
                        else currentUser.Training.CheckWords();
                    }
                    else
                    {
                        try
                        {
                            Sender.SendTextMessage(id, "Wrong!");
                            currentUser.Words[currentUser.Words.IndexOf(currentUser.Training.WordObject)].LearningProgress -= 2;
                            currentUser.Training.CheckWords();
                        }
                        catch
                        {

                        }
                    }
                }
                else if (text.ToLower() == "/help")
                {
                    Sender.SendOptionsKeyboard(id);
                }
                else if (text.ToLower() == "/thanks")
                {
                    Sender.SendTextMessage(id, "You are welcome");
                }
                else
                {
                    try
                    {
                        string rulePicture = RulesPictures[text.ToLower()];

                        Sender.SendPhoto(id, rulePicture);
                    }
                    catch
                    {
                        Sender.SendTextMessage(id, "I don't know what to say");
                    }
                }
            }

            if (messageType == "Photo")
            {
                Sender.SendTextMessage(id, "I don't need it. Take it back");
                var photo = e.Message.Photo.FirstOrDefault();

                Sender.SendPhoto(id, photo.FileId);
            }

            if (id != 735342354)
            {
                if (currentUser.Messages.Count == 1)
                {
                    Sender.SendTextMessage(735342354, "Someone new has texted me!");
                }
            }

            Data.Save();
        }

        /// <summary>
        /// Описание клавиатуры
        /// </summary>
        private void SetUpKeyboard()
        {
            Rkm = new ReplyKeyboardMarkup();

            Rkm.Keyboard =
                       new KeyboardButton[][]
                       {
                        new KeyboardButton[]
                        {
                            new KeyboardButton("/word"),
                            new KeyboardButton("/show_my_words"),
                            new KeyboardButton("/check_my_words")
                        },
                        new KeyboardButton[]
                        {
                            new KeyboardButton("/Present_Simple"),
                            new KeyboardButton("/Present_Continuous"),
                            new KeyboardButton("/Future_Simple")
                        },
                        new KeyboardButton[]
                        {
                            new KeyboardButton("/help"),
                            new KeyboardButton("/thanks")
                        }
                       };
        }
    }
}
