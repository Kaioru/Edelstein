using Edelstein.Common.Gameplay.Game.Objects.User.Messages;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using PowerArgs;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public class DebugCommandArgs : CommandArgs
{
    [ArgPosition(0)]
    [ArgDescription("Output to chat window?")]
    public bool ToChat { get; set; } = false;
}

public class DebugCommand : AbstractCommand<DebugCommandArgs>
{
    public override string Name => "Debug";
    public override string Description => "Testing command for debugging purposes";

    protected override async Task Execute(IFieldUser user, DebugCommandArgs args)
    {
        await user.Message(new IncEXPMessage(1000, true));
        await user.Message(new IncMoneyMessage(1000));

        var answer = await user.Prompt(target => target.AskMenu(
            "What would you like to debug?", new Dictionary<int, string>
            {
                [0] = "Stats",
                [1] = "Temporary Stats Records",
                [2] = "Skill Records",
                [3] = "ExtendedSP Records",
                [4] = "Quest Records",
                [5] = "QuestEx Records",
                [6] = "QuestCompleted Records",
            }), -1);

        if (answer == -1) return;

        switch (answer)
        {
            case 0:
                var userStats = user.Stats.ToString() ?? string.Empty;

                if (args.ToChat)
                {
                    await user.Message(userStats);
                    break;
                }

                await user.Prompt(s => s.Say(userStats), default);
                break;
            case 1:
                await DisplayRecordsToUser(
                    user,
                    "Temporary stats",
                    user.Character.TemporaryStats.Records.ToDictionary(
                        kvp => kvp.Key.ToString(),
                        kvp => kvp.Value.ToString() ?? string.Empty),
                    kvp => $"Type: {kvp.Key} Record: {kvp.Value}",
                    args.ToChat);
                break;
            case 2:
                await DisplayRecordsToUser(
                    user,
                    "Skill",
                    user.Stats.SkillLevels.Records,
                    kvp => $"Id: {kvp.Key} Level: {kvp.Value}",
                    args.ToChat);
                break;
            case 3:
                await DisplayRecordsToUser(
                    user,
                    "ExtendSP",
                    user.Character.ExtendSP.Records,
                    kvp => $"Id: {kvp.Key} Level: {kvp.Value}",
                    args.ToChat);
                break;
            case 4:
                await DisplayRecordsToUser(
                    user,
                    "Quest",
                    user.Character.QuestRecords.Records.ToDictionary(
                        kvp => kvp.Key.ToString(),
                        kvp => kvp.Value.ToString() ?? string.Empty),
                    kvp => $"Id: {kvp.Key} Value: {kvp.Value}",
                    args.ToChat);
                break;
            case 5:
                await DisplayRecordsToUser(
                    user,
                    "QuestEx",
                    user.Character.QuestRecordsEx.Records.ToDictionary(
                        kvp => kvp.Key.ToString(),
                        kvp => kvp.Value.ToString() ?? string.Empty),
                    kvp => $"Id: {kvp.Key} Value: {kvp.Value}",
                    args.ToChat);
                break;
            case 6:
                await DisplayRecordsToUser(
                    user,
                    "QuestCompleted",
                    user.Character.QuestCompletes.Records.ToDictionary(
                        kvp => kvp.Key.ToString(),
                        kvp => kvp.Value.ToString() ?? string.Empty),
                    kvp => $"Id: {kvp.Key} Value: {kvp.Value}",
                    args.ToChat);
                break;
        }
    }

    private static async Task DisplayRecordsToUser<T>(
        IFieldUser user,
        string header,
        IDictionary<T, T> record,
        Func<KeyValuePair<T, T>, string> formatting,
        bool outputToChat = false)
    {
        var characterName = outputToChat ? user.Character.Name : "#e#b#h #";
        var newline = outputToChat ? string.Empty : "\\n";
        var recordText = $"{header} records for player {characterName}: {newline}";

        for (var i = 0; i < record.Count; i++)
        {
            KeyValuePair<T, T> kvp = record.ElementAt(i);
            recordText += $"[{i}] {formatting(kvp)} {newline}";
        }

        if (outputToChat)
            await user.Message(recordText);
        else if (!outputToChat)
            await user.Prompt(s => s.Say(recordText), default);
    }
}
