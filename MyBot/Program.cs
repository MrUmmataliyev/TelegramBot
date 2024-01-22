using System.IO;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var botClient = new TelegramBotClient("6892424286:AAHYRBmIt7t7mfaeURblEQBCFuiupLtZZu4");

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

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    var handler = update.Type switch
    {
        UpdateType.Message => HandleMessageAsync(botClient, update, cancellationToken),
        UpdateType.EditedMessage => HandleEditedMessageAsync(botClient, update, cancellationToken),
        _ => HandleUnknownUpdateType(botClient, update, cancellationToken),
    };

    try
    {
        await handler;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error chiqdi:{ex.Message}");
    }
}

async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    var message = update.Message;
    var user = message.Chat.FirstName;
    var handler = message.Type switch
    {
        MessageType.Text => HandleTextMessageAsync(botClient, update, cancellationToken, user),
        MessageType.Video => HandleVideoMessageAsync(botClient, update, cancellationToken, user),
        MessageType.VideoNote => HandleVideoNoteMessageAsync(botClient, update, cancellationToken, user),
        MessageType.Photo => HandlePhotoMessageAsync(botClient, update, cancellationToken, user),
        MessageType.Document => HandleDocumentMessageAsync(botClient, update, cancellationToken, user),
        MessageType.Animation => HandleAnimationMessageAsync(botClient, update, cancellationToken, user),
        MessageType.Sticker => HandleStickerMessageAsync(botClient, update, cancellationToken, user),
        MessageType.Audio => HandleAudioMessageAsync(botClient, update, cancellationToken, user),
        MessageType.Voice => HandleVoiceMessageAsync(botClient, update, cancellationToken, user),
        MessageType.Poll => HandlePollMessageAsync(botClient, update, cancellationToken, user),
        MessageType.Contact => HandleContactMessageAsync(botClient, update, cancellationToken, user),
        
        _ => HandleUnknownMessageTypeAsync(update, update, cancellationToken),
    };
}

async Task HandleVideoMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string user)
{
    
    await botClient.SendPhotoAsync(
        chatId: update.Message.Chat.Id,
        photo: InputFile.FromUri("https://youtu.be/HafC3ayrt3U"),
        cancellationToken: cancellationToken);
    Console.WriteLine($"Recieved Video from {user}");
}



async Task HandleContactMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string user)
{
    Message message = await botClient.SendContactAsync(
    chatId: update.Message.Chat.Id,
    phoneNumber: "+9988888888",
    firstName: "VIP",
    lastName: "",
    cancellationToken: cancellationToken);

    Console.WriteLine($"Recieved Contact from {user}");
}



async Task HandlePollMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string user)
{
    Message pollMessage = await botClient.SendPollAsync(
        chatId:update.Message.Chat.Id,
        question: "Do you like my bot?",
        options: new[]
        {
            "Yes!",
            "Of course!",
            "Certainly!"
        },
        cancellationToken: cancellationToken);

        Console.WriteLine($"Recieved Poll from {user}");
}

async Task HandleVideoNoteMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string user)
{

    await botClient.SendPhotoAsync(
    chatId: update.Message.Chat.Id,
    photo: InputFile.FromUri("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTTBHhDPRuGJuG4MpCuJdxHVSvxtFALS2W40eE0_v20lCabGahsmQHx5GVnparjtBXkaGc&usqp=CAU"),
    cancellationToken: cancellationToken);

    Console.WriteLine($"Recieved VideoNote from {user}");
}

async Task HandlePhotoMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string user)
{
    await botClient.SendPhotoAsync(
       chatId: update.Message.Chat.Id,
       photo: InputFile.FromUri("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS5VvWIuY9JvTM7aPlVwE_VxmuGOTz9Zajpug&usqp=CAU"),
       cancellationToken: cancellationToken);
    Console.WriteLine($"Recieved Photo from {user}");
}


async Task HandleStickerMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string user)
{
    await botClient.SendStickerAsync(
       chatId: update.Message.Chat.Id,
       sticker: InputFile.FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/sticker-fred.webp"),
       cancellationToken: cancellationToken);
    Console.WriteLine($"Recieved Stiker from {user}");
}


async Task HandleDocumentMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string user)
{
    await botClient.SendPhotoAsync(
       chatId: update.Message.Chat.Id,
       photo: InputFile.FromUri("https://www.computerhope.com/jargon/d/doc.png"),
       cancellationToken: cancellationToken);
    Console.WriteLine($"Recieved Document from {user}");
}

async Task HandleAnimationMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string user)
{
    Message message = await botClient.SendAnimationAsync(
        chatId: update.Message.Chat.Id,
        animation: InputFile.FromUri("https://raw.githubusercontent.com/TelegramBots/book/master/src/docs/video-waves.mp4"),

        cancellationToken: cancellationToken);
        Console.WriteLine($"Recieved Animation from {user}");
}
async Task HandleAudioMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string user)
{
    await botClient.SendAudioAsync(
              chatId: update.Message.Chat.Id,
              audio: InputFile.FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/audio-guitar.mp3"),
              cancellationToken: cancellationToken

        );
     
    Console.WriteLine($"Recieved Audio from {user}");
}

async Task HandleVoiceMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string user)
{
    await botClient.SendPhotoAsync(
     chatId: update.Message.Chat.Id,
     photo: InputFile.FromUri("https://images.idgesg.net/images/article/2019/07/alexa_virtual-assistant_echo_amazon-alexa_voice-control-100801663-large.jpg?auto=webp&quality=85,70"),
     cancellationToken: cancellationToken);
    Console.WriteLine($"Recieved Voice from {user}");
}



async Task HandleTextMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string user)
{
    await botClient.SendTextMessageAsync(
        chatId:update.Message.Chat.Id, 
        text: $"You said: \n {update.Message.Text}",
        cancellationToken: cancellationToken
        );
    Console.WriteLine($"Recieved TextMessage from{user}");

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


