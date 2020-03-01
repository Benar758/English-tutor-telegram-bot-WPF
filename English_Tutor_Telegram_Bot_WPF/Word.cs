using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace English_Tutor_Telegram_Bot_WPF
{
    /// <summary>
    /// Слово
    /// </summary>
    public class Word
    {
        /// <summary>
        /// Создание нового слова
        /// </summary>
        /// <param name="Word">Слово</param>
        /// <param name="Translation">Перевод</param>
        public Word(string Word, string Translation)
        {
            this._Word = Word;
            this.Translation = Translation;
            LearningProgress = 0;
        }

        public Word() { }

        /// <summary>
        /// Слово
        /// </summary>
        public string _Word { get; set; }

        /// <summary>
        /// Перевод
        /// </summary>
        public string Translation { get; set; }

        /// <summary>
        /// Прогресс изучения слова
        /// </summary>
        public int LearningProgress { get; set; }

        /// <summary>
        /// Необходимый прогресс слова, чтобы считать его освоенным
        /// </summary>
        public const int NeededProgress = 5;

        /// <summary>
        /// Добавление нового слова для конкретного пользователя
        /// </summary>
        /// <param name="ChatId">id</param>
        /// <param name="_Word">Слово</param>
        public static void Add(long ChatId, Word _Word)
        {
            var selectedUser = from user in MainWindow.Bot.Users where user.ChatId == ChatId select user;

            User _user = selectedUser.FirstOrDefault();

            _user.Words.Add(_Word);
        }
    }
}
