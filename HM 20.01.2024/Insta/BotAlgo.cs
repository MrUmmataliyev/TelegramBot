using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InlineQueryResults;

namespace Insta
{
    public class TelegramBotHandler
    {
        public string Token { get; set; }
        public TelegramBotHandler(string token)
        {
            this.Token = token;
        }

        public async Task BotHandle()
        {
            var botClient = new TelegramBotClient($"{this.Token}");

            using CancellationTokenSource cts = new();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
                
            };
           
            
            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            
            // Send cancellation request to stop bot
            cts.Cancel();

        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            //// Only process text messages
            //if (message.Text is not { } messageText)
            //    return;



            var chatId = update.Message.Chat.Id;
            // var message = update.Message;

            Console.WriteLine($"Received a '{message.Text}' message in chat {chatId}. UserName =>  {message.Chat.Username}");

            //var fileStream = new FileStream(@"C:\Users\dotnetbillioner\Videos\test.mp4", FileMode.Open);
            Console.WriteLine($"Message Type: {message.Type} Username=> {message.Chat.Username} Text => {message.Text} ");

            string replasemessage = update.Message.Text.Replace("www.", "dd");


            if (update.Message.Text.StartsWith("https://www.instagram.com"))
            {
                //Message sentMessage = await botClient.SendVideoAsync(
                //   chatId: chatId,
                //   video: $"{replasemessage}",
                //   supportsStreaming: true,
                //   cancellationToken: cancellationToken);

                Message sentMessage = await botClient.SendPhotoAsync(
                   chatId: chatId,
                   photo: $"{replasemessage}",
                   //  supportsStreaming: true,
                   cancellationToken: cancellationToken);
            }



        }





        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

    }
}
