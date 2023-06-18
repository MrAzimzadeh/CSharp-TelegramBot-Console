// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

struct BotUpdate
{
    public string? text;
    public long? id;
    public string? username;
}

class Program
{

    private static TelegramBotClient Bot = new TelegramBotClient("6015012086:AAHKX2ywM94533VE_JcQsMcxvMHs31pAvlE");
    static string fileName = "updates.json";
    static List<BotUpdate> botUpdates = new List<BotUpdate>();

    static void Main(string[] args)
    {
        //Read all saved updates
        try
        {
            var botUpdatesString = System.IO.File.ReadAllText(fileName);

            botUpdates = JsonConvert.DeserializeObject<List<BotUpdate>>(botUpdatesString) ?? botUpdates;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading or deserializing {ex}");
        }

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new UpdateType[]
            {
                UpdateType.Message,
                UpdateType.EditedMessage,
            }
        };

        Bot.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions);

        Console.ReadLine();
    }

    private static Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        throw new NotImplementedException();
    }

    private static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken arg3)
    
    {
        if (update.Type == UpdateType.Message)
        {
            if (update.Message.Type == MessageType.Text)
            {
                //write an update
                var _botUpdate = new BotUpdate
                {
                    text = update.Message.Text,
                    id = update.Message.Chat.Id,
                    username = update.Message.Chat.Username
                };

                botUpdates.Add(_botUpdate);

                var botUpdatesString = JsonConvert.SerializeObject(botUpdates);

                System.IO.File.WriteAllText(fileName, botUpdatesString);

                // Yanıt gönderme işlemini gerçekleştir
                var responseText = " Response Text ";
                var chatId = update.Message.Chat.Id;

                await bot.SendTextMessageAsync(chatId, responseText);
            }
        }
    }
}