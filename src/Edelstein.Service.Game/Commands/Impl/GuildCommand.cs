using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.Objects.User;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Commands.Impl
{
    public class GuildCommand : AbstractCommand<DefaultCommandContext>
    {
        public override string Name => "Guild";
        public override string Description => "All guild related commands.";

        public GuildCommand(Parser parser) : base(parser)
        {
            Commands.Add(new CommandBuilder(parser)
                .WithName("Info")
                .WithDescription("Prints current guild info")
                .Build(async (user, ctx) =>
                {
                    if (user.Guild == null)
                    {
                        await user.Message("Failed to retrieve current guild info, not in guild.");
                        return;
                    }

                    await user.Message($"Guild ID: {user.Guild.ID}");
                    await user.Message("Guild Members:");
                    user.Guild.Members.ForEach(async (m, i) =>
                        await user.Message(
                            $"\t{i + 1}) {m.CharacterName} ({m.CharacterID}) [{user.Guild.GradeName[m.Grade - 1]}]"
                        ));
                }));
            Commands.Add(new CommandBuilder(parser)
                .WithName("Create")
                .WithDescription("Create a new guild")
                .Build(async (user, ctx) =>
                {
                    try
                    {
                        var name = await user.Prompt<string>(s => s.AskText("What guild name?"));

                        if (user.Party == null)
                            await user.Message("Failed to create guild, not in party");
                        await user.Service.GuildManager.Create(name, user.Party);
                    }
                    catch
                    {
                        await user.Message("Failed to create guild");
                    }
                })
            );
            Commands.Add(new CommandBuilder(parser)
                .WithName("Join")
                .WithDescription("Create a specified guild")
                .Build(async (user, ctx) =>
                {
                    try
                    {
                        var guildID = await user.Prompt<int>(s => s.AskNumber("What Guild ID would you like to join?"));
                        var guild = await user.Service.GuildManager.Load(guildID);

                        // TODO: shortcut methods
                        await user.Service.GuildManager.Join(guild, user.Character);
                    }
                    catch
                    {
                        await user.Message("Failed to join guild.");
                    }
                })
            );
        }

        protected override Task Run(FieldUser sender, DefaultCommandContext ctx)
            => Process(sender, new Queue<string>(new[] {"--help"}));
    }
}