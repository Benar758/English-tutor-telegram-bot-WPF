using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace English_Tutor_Telegram_Bot_WPF
{
    /// <summary>
    /// Сообщение
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Новое сообщение
        /// </summary>
        /// <param name="Text">Текст сообщения</param>
        /// <param name="MessageType">Тип сообщения</param>
        /// <param name="ChatId">Id</param>
        public Message(string Text, string MessageType, long ChatId)
        {
            Date = DateTime.Now.ToString();
            this.Text = Text;
            this.MessageType = MessageType;
            this.ChatId = ChatId;
        }

        public Message() { }

        /// <summary>
        /// Дата и время отправки сообщения
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Тип сообщения
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long ChatId { get; set; }

    }
}
