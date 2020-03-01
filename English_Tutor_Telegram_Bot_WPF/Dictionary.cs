using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace English_Tutor_Telegram_Bot_WPF
{
    /// <summary>
    /// Класс для работы с базой иностранных слов для рандомной отправки пользователю по запросу
    /// </summary>
    static class Dictionary
    {
        /// <summary>
        /// Набор иностранных слов с их переводом 
        /// </summary>
        public static Dictionary<string, string> Words { get; set; }
    }
}
