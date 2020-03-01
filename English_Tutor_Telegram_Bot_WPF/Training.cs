using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace English_Tutor_Telegram_Bot_WPF
{
    /// <summary>
    /// Тренировка слов
    /// </summary>
    public class Training
    {
        /// <summary>
        /// Создание тренировки слов для конкретного пользователя
        /// </summary>
        /// <param name="User">Пользователь</param>
        /// <param name="Counter">Счётчик макс. кол-ва вопросов</param>
        public Training(User User, int Counter)
        {
            _User = User;
            WordObject = null;
            CurrentWord = string.Empty;
            CorrectAnswer = string.Empty;
            WordIndex = -1;
            Words = new List<Word>();
            this.Counter = Counter;
            FirstWord = true;
        }

        /// <summary>
        /// Пользователь
        /// </summary>
        public User _User { get; set; }

        /// <summary>
        /// Объект проверяемого слова для идентификации его в массиве слов конкретного пользователя
        /// </summary>
        public Word WordObject { get; set; }

        /// <summary>
        /// Текущее слово на английском
        /// </summary>
        public string CurrentWord { get; set; }

        /// <summary>
        /// Перевод текущего слова (правильный ответ)
        /// </summary>
        public string CorrectAnswer { get; set; }

        /// <summary>
        /// Индекс текущего слова в рамках тренировки
        /// </summary>
        public int WordIndex { get; set; }

        /// <summary>
        /// Слова, загруженные для тренировки
        /// </summary>
        public List<Word> Words { get; set; }

        /// <summary>
        /// Счётчик кол-ва вопросов
        /// </summary>
        public int Counter { get; set; }

        /// <summary>
        /// Кол-во вопросов по максимуму
        /// </summary>
        public static int AmountOfQuestions { get; set; } = 5;

        /// <summary>
        /// Первое ли слово спрашивается в рамках текующей тренировки
        /// </summary>
        public bool FirstWord { get; set; }

        /// <summary>
        /// Проверка слов. Начало тренировки
        /// </summary>
        public void CheckWords()
        {
            //Мод изучения слов включён
            _User.LearningMode = true;

            //Сообщенаем пользователю о начале тренировки
            if (FirstWord)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i <= 10; i++)
                {
                    sb.Append(".\n");
                }

                Sender.SendTextMessage(_User.ChatId, sb.ToString());
                Sender.SendTextMessage(_User.ChatId, "Start!");
            }

            Random r = new Random();

            try
            {
                //Формируем список слов, которые будут проверятся
                if (FirstWord)
                {
                    int index;

                    var selectedWords = from word in _User.Words where word.LearningProgress < Word.NeededProgress select word;

                    List<Word> temp = selectedWords.ToList();

                    if (temp.Count == 0)
                    {
                        Sender.SendTextMessage(_User.ChatId, "Вы уже выучили все слова!");
                        _User.LearningMode = false;
                        return;
                    }

                    for (int i = 0; i < AmountOfQuestions; i++)
                    {
                        if (temp.Count <= 10)
                        {
                            if (temp.Count < AmountOfQuestions)
                            {
                                AmountOfQuestions = temp.Count;
                                Counter = AmountOfQuestions;
                            }

                            for (int j = 0; j < AmountOfQuestions; j++)
                            {
                                Words.Add(temp[j]);
                            }

                            AmountOfQuestions = 5;

                            break;
                        }

                        index = r.Next(temp.Count - 1);

                        Word w = temp[index];
                        Words.Add(w);
                        temp.RemoveAt(index);
                    }

                    FirstWord = false;
                }
            }
            catch
            {

            }

            try
            {
                WordIndex = r.Next(Words.Count - 1);
                WordObject = Words[WordIndex];

                CurrentWord = WordObject._Word;
                CorrectAnswer = WordObject.Translation;

                Sender.SendTextMessage(_User.ChatId, $"Translate: {CurrentWord}");
            }
            catch
            {
                return;
            }
        }
    }
}
