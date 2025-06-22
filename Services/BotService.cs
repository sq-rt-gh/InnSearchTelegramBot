using InnSearchTelegramBot.Interfaces;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InnSearchTelegramBot.Services;

public class BotService(IEnumerable<IBotCommand> Commands, ITelegramBotClient BotClient, ILogger<BotService> Logger)
{
    private readonly Dictionary<long, (IBotCommand command, string[]? args)> _lastCommands = new();

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { Text: var text } || string.IsNullOrWhiteSpace(text)) 
            return;

        Logger.LogInformation("Сообщение от {User}: {Text}",
            update.Message.From?.Username ?? "unknown", update.Message.Text ?? "<empty text>");

        var parts = text.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var commandName = parts?[0].ToLower();
        var args = parts?.Skip(1).ToArray();

        var chatId = update.Message.Chat.Id;

        if (commandName == "/last")
        {
            if (_lastCommands.TryGetValue(chatId, out var last))
            {
                await last.command.ExecuteAsync(update.Message, last.args, cancellationToken);
            }
            else
            {
                await BotClient.SendMessage(chatId, "Нет предыдущей команды.", cancellationToken: cancellationToken);
            }
            return;
        }

        var command = Commands.FirstOrDefault(c => c.Name == commandName);
        if (command != null)
        {
            _lastCommands[chatId] = (command, args);
            await command.ExecuteAsync(update.Message, args, cancellationToken);
        }
        else
        {
            await BotClient.SendMessage(chatId, 
                "Неизвестная команда. Используйте /help для просмотра доступных команд.", 
                cancellationToken: cancellationToken);
        }
    }
}
