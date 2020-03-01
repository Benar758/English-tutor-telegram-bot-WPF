using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace English_Tutor_Telegram_Bot_WPF
{
    /// <summary>
    /// Отправщик сообщений
    /// </summary>
    static class Sender
    {
        /// <summary>
        /// Отправляет текстовое сообщение
        /// </summary>
        /// <param name="chatId">Id</param>
        /// <param name="Text">Текст сообщения</param>
        public async static void SendTextMessage(long chatId, string Text)
        {
            if (!string.IsNullOrEmpty(Text)) await Bot.TelBot.SendTextMessageAsync(chatId, Text);
        }

        /// <summary>
        /// Отправляет инструкцию и устанавливает клавиатуру для управления ботом
        /// </summary>
        /// <param name="chatId">id</param>
        public async static void SendOptionsKeyboard(long chatId)
        {
            await Bot.TelBot.SendTextMessageAsync(chatId, Bot.Options, Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, Bot.Rkm);
        }

        /// <summary>
        /// Отправляет картинку
        /// </summary>
        /// <param name="chatId">Id</param>
        /// <param name="PhotoUrl">Ссылка на картинку</param>
        public async static void SendPhoto(long chatId, string PhotoUrl)
        {
            await Bot.TelBot.SendPhotoAsync(chatId, PhotoUrl);
        }
    }
}
