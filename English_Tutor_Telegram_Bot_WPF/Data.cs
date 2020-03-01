using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace English_Tutor_Telegram_Bot_WPF
{
    /// <summary>
    /// Класс для работы с данными
    /// </summary>
    class Data
    {
        private readonly static string DataPath = @"Data.json";
        private readonly static string WordsDataPath = @"WordsData.txt";

        /// <summary>
        /// Сохранение данных
        /// </summary>
        public static void Save()
        {
            string json = JsonConvert.SerializeObject(MainWindow.Bot.Users);
            File.WriteAllText(DataPath, json);
        }

        /// <summary>
        /// Загрузка данных
        /// </summary>
        public static void Load()
        {
            #region Загрузка данных о пользователях

            if (!File.Exists(DataPath)) File.Create(DataPath).Dispose();
            string json = File.ReadAllText(DataPath);
            if (!string.IsNullOrEmpty(json)) MainWindow.Bot.Users = JsonConvert.DeserializeObject<ObservableCollection<User>>(json);

            #endregion

            #region Загрузка иностранных слов для отправки пользователям

            if (File.Exists(WordsDataPath))
            {
                string[] wordsLines = File.ReadAllLines(WordsDataPath);

                Dictionary<string, string> words = new Dictionary<string, string>();

                for (int i = 0; i < wordsLines.Length; i++)
                {
                    string[] tempLine = wordsLines[i].Split('\t');
                    try
                    {
                        words.Add(tempLine[0], tempLine[2]);
                    }
                    catch
                    {
                        continue;
                    }
                }

                Dictionary.Words = words;
            }

            #endregion
        }
    }
}
