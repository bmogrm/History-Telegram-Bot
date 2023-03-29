using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;
using Newtonsoft.Json;
using System;

var botClient = new TelegramBotClient("5982499102:AAEIa2j6jBoGVaGemS4WmH0unG-xDYbXv6s");

var dates = System.IO.File.ReadAllText("C:\\Rodion\\testdate4.json");
var json = JsonConvert.DeserializeObject<string[]>(dates);

var info = "Спасибо что запустил меня!" +
           "Ты можешь использовать меня чтобы узнать все возможные даты по истории России." +
           "На данный момент ты можешь нажать на кнопку 'Случайная дата' - чтобы получить рандомную дату, а так же" +
           "выбрать интересующий век и плучить список всех дат за этот век";

using CancellationTokenSource cts = new();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
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


async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
{
    var text = update.Message.Text;
    var id = update.Message.Chat.Id;

    if (text != null)
    {
        if (text == "Начать" || text == "Вернуться на главную" || text == "/start")
        {
            if(text == "/start")
            {
                await botClient.SendTextMessageAsync(chatId: id, text: info, cancellationToken: token);
            }

            ReplyKeyboardMarkup mainButtons = new(new[]
            {
                new KeyboardButton[] {"Случайная дата", "Выбрать век"},
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
                chatId: id,
                text: "Выберите действие: ",
                replyMarkup: mainButtons,
                cancellationToken: token);
            return;
        }

        if (text == "Случайная дата")
        {
            await botClient.SendTextMessageAsync(
                chatId: id,
                text: json[new Random().Next(0, json.Length)],
                cancellationToken: token);
            return;
        }

        if (text == "Выбрать век")
        {
            ReplyKeyboardMarkup dateButtons = new(new[]
        {
                new KeyboardButton[] {"VI-IX века", "XI век", "XII век"},
                new KeyboardButton[] {"XIII век", "XIV век", "XV век"},
                new KeyboardButton[] {"XVI век", "XVII век", "XVIII век"},
                new KeyboardButton[] {"Вернуться на главную" }
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
                chatId: id,
                text: "Выберите действие: ",
                replyMarkup: dateButtons,
                cancellationToken: token);
            return;
        }

        if ("VI-IX века" == text)
        {
            for (int i = 0; i < 27 ; i++)
            {
                await botClient.SendTextMessageAsync(chatId: id, text: json[i], cancellationToken: token);
            }
            return;
        }

        if (text == "XI век")
        {
            for (int i = 27; i < 50; i++)
            {
                await botClient.SendTextMessageAsync(chatId: id, text: json[i], cancellationToken: token);
            }
            return;
        }

        if (text == "XII век")
        {
            for (int i = 50; i < 83; i++)
            {
                await botClient.SendTextMessageAsync(chatId: id, text: json[i], cancellationToken: token);
            }
            return;
        }

        if (text == "XIII век")
        {
            for (int i = 83; i < 121; i++)
            {
                await botClient.SendTextMessageAsync(chatId: id, text: json[i], cancellationToken: token);
            }
            return;
        }

        if (text == "XIV век")
        {
            for (int i = 121; i < 164; i++)
            {
                await botClient.SendTextMessageAsync(chatId: id, text: json[i], cancellationToken: token);
            }
            return;
        }

        if (text == "XV век")
        {
            for (int i = 164; i < 216; i++)
            {
                await botClient.SendTextMessageAsync(chatId: id, text: json[i], cancellationToken: token);
            }
            return;
        }

        if (text == "XVI век")
        {
            for (int i = 216; i < 293; i++)
            {
                await botClient.SendTextMessageAsync(chatId: id, text: json[i], cancellationToken: token);
            }
            return;
        }

        if (text == "XVII век")
        {
            for (int i = 293; i < 386; i++)
            {
                await botClient.SendTextMessageAsync(chatId: id, text: json[i], cancellationToken: token);
            }
            return;
        }

        if (text == "XVIII век")
        {
            for (int i = 386; i < 587; i++)
            {
                await botClient.SendTextMessageAsync(chatId: id, text: json[i], cancellationToken: token);
            }
            return;
        }

        else
        {
            await botClient.SendTextMessageAsync(chatId: id, text: "Наберите Начать, чтобы вызвать окно действий", cancellationToken: token);
        }

    }
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
