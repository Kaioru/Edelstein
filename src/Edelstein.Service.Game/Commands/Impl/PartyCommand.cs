using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.Objects.User;
using MoreLinq;

namespace Edelstein.Service.Game.Commands.Impl
{
    public class PartyCommand : AbstractCommand<DefaultCommandContext>
    {
        public override string Name => "Party";
        public override string Description => "All party related commands.";

        public PartyCommand(Parser parser) : base(parser)
        {
            Commands.Add(new CommandBuilder(parser)
                .WithName("Info")
                .WithDescription("Prints current party info")
                .Build(async (user, ctx) =>
                {
                    if (user.Party == null)
                    {
                        await user.Message("Failed to retrieve current party info, not in party.");
                        return;
                    }

                    await user.Message($"Party ID: {user.Party.ID}");
                    await user.Message($"Party Boss ID: {user.Party.BossCharacterID}");

                    await user.Message("Party Members:");
                    user.Party.Members.ForEach(async (m, i) =>
                        await user.Message($"\t{i + 1}) {m.CharacterName} ({m.CharacterID})")
                    );
                }));
            Commands.Add(new CommandBuilder(parser)
                .WithName("Create")
                .WithDescription("Create a new party")
                .Build((user, ctx) => user.Service.PartyManager.Create(user.Character))
            );
            Commands.Add(new CommandBuilder(parser)
                .WithName("Join")
                .WithDescription("Create a specified party")
                .Build(async (user, ctx) =>
                {
                    try
                    {
                        var partyID = await user.Prompt<int>(s => s.AskNumber("What Party ID would you like to join?"));
                        var party = await user.Service.PartyManager.Load(partyID);

                        await party.Join(user.Character);
                    }
                    catch
                    {
                        await user.Message("Failed to join party.");
                    }
                })
            );
            Commands.Add(new CommandBuilder(parser)
                .WithName("Disband")
                .WithDescription("Disband the current party")
                .Build(async (user, ctx) =>
                {
                    if (user.Party != null) await user.Party.Disband();
                    else await user.Message("Failed to disband party, not in party.");
                })
            );
            Commands.Add(new CommandBuilder(parser)
                .WithName("Withdraw")
                .WithDescription("Withdraw from the current party")
                .Build(async (user, ctx) =>
                {
                    if (user.Party != null)
                    {
                        var target = user.Party.Members
                            .First(m => m.CharacterID == user.ID);
                        await user.Party.Withdraw(target);
                    }
                    else await user.Message("Failed to withdraw from party, not in party.");
                })
            );
            Commands.Add(new CommandBuilder(parser)
                .WithName("Kick")
                .WithDescription("Kick a member from the current party")
                .Build(async (user, ctx) =>
                {
                    if (user.Party != null)
                    {
                        var targetID = await user.Prompt<int>(s =>
                            s.AskMenu(
                                "Who would you like to kick from the party?",
                                user.Party.Members.ToDictionary(
                                    m => m.CharacterID,
                                    m => m.CharacterName
                                )
                            )
                        );
                        var target = user.Party.Members
                            .First(m => m.CharacterID == targetID);

                        await user.Party.Kick(target);
                    }
                    else await user.Message("Failed to kick from party, not in party.");
                })
            );
            Commands.Add(new CommandBuilder(parser)
                .WithName("ChangeBoss")
                .WithAlias("Boss")
                .WithDescription("Delegate a member the boss from the current party")
                .Build(async (user, ctx) =>
                {
                    if (user.Party != null)
                    {
                        var targetName = await user.Prompt<string>(s =>
                            s.AskText("Who would you like to delegate the boss in the party?")
                        );
                        var target = user.Party.Members
                            .FirstOrDefault(m => m.CharacterName == targetName);

                        if (target == null)
                        {
                            await user.Message("Failed to delegate boss in party, target not in party.");
                            return;
                        }

                        await user.Party.ChangeBoss(target);
                    }
                    else await user.Message("Failed to delegate boss in party, not in party.");
                })
            );
        }

        protected override Task Run(FieldUser sender, DefaultCommandContext ctx)
            => Process(sender, new Queue<string>(new[] {"--help"}));
    }
}