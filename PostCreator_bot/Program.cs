using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using PostCreator_bot;
internal class Program
{
    static async Task Main(string[] args)
    {
        BotAlgo bot = new BotAlgo();
        await bot.WorkingBot();

    }
}


internal class ControlBottonClass
{
    public static async Task StartButton(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(
            new[]
            {
                new KeyboardButton[] { "create post" },
            }
        )

        {
            ResizeKeyboard = true
        };


        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Yangi Post yaratmoqchi bolsangiz tugmasini Create postni bosing!",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
    public static async Task CreateButton(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var buttons = new List<List<KeyboardButton>>();

        var buttonsgorizontal1 = new List<KeyboardButton>();
        buttonsgorizontal1.Add(new KeyboardButton("Kanal nomini kiritish"));
        buttonsgorizontal1.Add(new KeyboardButton("PostText Kiritish"));
        buttonsgorizontal1.Add(new KeyboardButton("Rasim kiritish"));
        buttonsgorizontal1.Add(new KeyboardButton("linkni kiritish"));

        var buttonsgorizontal2 = new List<KeyboardButton>();
        buttonsgorizontal2.Add(new KeyboardButton("<-"));
        buttonsgorizontal2.Add(new KeyboardButton("Saqlash"));
        buttonsgorizontal2.Add(new KeyboardButton("Kanalga yuborish"));
        buttons.Add(buttonsgorizontal1);
        buttons.Add(buttonsgorizontal2);


        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Tanlovlar",
            replyMarkup: new ReplyKeyboardMarkup(buttons),
            cancellationToken: cancellationToken);
    }
    public static async Task EditButtons(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var buttons = new List<List<KeyboardButton>>();

        var buttonsgorizontal1 = new List<KeyboardButton>();
        buttonsgorizontal1.Add(new KeyboardButton("Kanal nomini yangilash"));
        buttonsgorizontal1.Add(new KeyboardButton("PostTextni yangilah"));
        buttonsgorizontal1.Add(new KeyboardButton("Rasimni yangilash"));
        buttonsgorizontal1.Add(new KeyboardButton("Linkni yangilash"));
        var buttonsgorizontal2 = new List<KeyboardButton>();
        buttonsgorizontal2.Add(new KeyboardButton("<-"));
        buttonsgorizontal2.Add(new KeyboardButton("Yangilanishni saqlash"));
        buttons.Add(buttonsgorizontal1);
        buttons.Add(buttonsgorizontal2);

        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Choose a response",
            replyMarkup: new ReplyKeyboardMarkup(buttons),
            cancellationToken: cancellationToken);
    }
}