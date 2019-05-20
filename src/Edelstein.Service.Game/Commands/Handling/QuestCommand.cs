using System.Threading.Tasks;
using CommandLine;
using Edelstein.Provider.Templates.Quest;
using Edelstein.Provider.Templates.String;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class QuestCommand : AbstractTemplateCommand<QuestTemplate, QuestStringTemplate, QuestCommandOption>
    {
        public override string Name => "Quest";
        public override string Description => "Sets the specified quest to the state.";

        public QuestCommand(Parser parser) : base(parser)
        {
        }

        protected override async Task ExecuteAfter(FieldUser sender, QuestTemplate template, QuestCommandOption option)
        {
            await sender.ModifyQuests(q =>
            {
                switch (option.State)
                {
                    default:
                    case QuestState.No:
                        q.Resign((short) template.ID);
                        break;
                    case QuestState.Perform:
                        q.Accept((short) template.ID);
                        break;
                    case QuestState.Complete:
                        q.Complete((short) template.ID);
                        break;
                }
            });
            await sender.Message($"Successfully set quest {template.ID} to {option.State}");
        }
    }

    public class QuestCommandOption : TemplateCommandOption
    {
        [Option("state", HelpText = "The quest state.", Required = true)]
        public QuestState State { get; set; }
    }
}