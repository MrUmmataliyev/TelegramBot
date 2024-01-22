using Newtonsoft.Json;
using System.IO.Compression;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


var botClient = new TelegramBotClient("6363331015:AAFwvagGZppYQ9Mi3j1azW2xw31a1pppFYE");

using CancellationTokenSource cts = new();

List<ChatId> users = [];
string? fileName = null;
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

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message

    var handler = update.Type switch
    {
        UpdateType.Message => HandleMessageAsync(botClient, update, cancellationToken),
        UpdateType.CallbackQuery => HandleCallBackQueryAsync(botClient, update, cancellationToken),
        UpdateType.EditedMessage => HandleEditedMessageAsync(botClient, update, cancellationToken),
        //UpdateType.CallbackQuery =>HandleMessageAsync(botClient, update, cancellationToken),
        _ => HandleUnknownUpdateType(botClient, update, cancellationToken),
    };






}

async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    var message = update.Message;
    var user = message.Chat.FirstName;
    var handler = message.Type switch
    {
        MessageType.Text => HandleTextMessageAsync(botClient, update, cancellationToken, user),

        _ => HandleUnknownMessageTypeAsync(update, update, cancellationToken),
    };
}




async Task HandleTextMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string user)
{
    if (update.Message.Text == "/start")
    {
        users.Add(update.Message.Chat.Id);
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
            new KeyboardButton[] { "Path kiriting"}
        })
        {
            ResizeKeyboard = true
        };

        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Choose a response",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);

    }

    else if (update.Message.Text == "Path kiriting")
    {


       await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: @"Masalan: D:\Najot ta'lim\N11\Telegram_bot_Project\ValutaBot",
            cancellationToken: cancellationToken
            );
        Console.WriteLine("text");

    }

    else if (update.Message.Text.StartsWith("D:") || update.Message.Text.StartsWith("C:") || update.Message.Text.StartsWith("E:"))
    {
        string path = update.Message.Text;
        if (System.IO.Directory.Exists(path)==true)
        {
            string startPath = $@"{path}";
            var arr = path.Split(@"\");
            string zipPath = $@"D:\Najot ta'lim\N11\Telegram_bot_Project\{arr[arr.Length-1]}.zip";

            if (System.IO.File.Exists(zipPath) == false)
            {

                ZipFile.CreateFromDirectory(startPath, zipPath);
            }

            for (int i = 0; i <= users.Count - 1; i++)
            {
                Console.WriteLine("Forda");
                await using Stream stream = System.IO.File.OpenRead($"{zipPath}");
                Message message = await botClient.SendDocumentAsync(
                    chatId: users[i],
                    document: InputFile.FromStream(stream: stream, fileName: $"{arr[arr.Length - 1]}.zip"),
                    caption: "Gopporaka");

            }
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId:update.Message.Chat.Id,
                text:"Such path not found Error[404]",
                cancellationToken:cancellationToken
                );
        }
    }
 


}
async Task HandleCallBackQueryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{

}


Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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



Task HandleEditedMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    throw new NotImplementedException();
}

async Task HandleUnknownMessageTypeAsync(Update update1, Update update2, CancellationToken cancellationToken)
{
    throw new NotImplementedException();
}



async Task HandleUnknownUpdateType(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    throw new NotImplementedException();
}



