using System;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Quest;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Speakers.Fields;
using Edelstein.Service.Game.Conversations.Speakers.Fields.Quests;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Fields.Objects.User.Quests;

namespace Edelstein.Service.Game.Services.Handlers.User
{
    public class UserQuestRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var action = (QuestRequest) packet.Decode<byte>();
            var templateID = packet.Decode<short>();
            var template = user.Service.TemplateManager.Get<QuestTemplate>(templateID);

            if (template == null) return;

            var check = action switch {
                QuestRequest.AcceptQuest => template.Check(QuestState.None, user),
                QuestRequest.OpeningScript => template.Check(QuestState.None, user),
                QuestRequest.CompleteQuest => template.Check(QuestState.Perform, user),
                QuestRequest.CompleteScript => template.Check(QuestState.Perform, user),
                _ => true
                };

            switch (action)
            {
                case QuestRequest.LostItem:
                    break;
                case QuestRequest.AcceptQuest:
                {
                    var npcTemplateID = packet.Decode<int>();

                    await user.ModifyQuests(q => q.Accept(templateID));
                    break;
                }

                case QuestRequest.CompleteQuest:
                    await user.ModifyQuests(q => q.Complete(templateID));
                    break;
                case QuestRequest.ResignQuest:
                    await user.ModifyQuests(q => q.Resign(templateID));
                    break;
                case QuestRequest.OpeningScript:
                case QuestRequest.CompleteScript:
                {
                    var npcTemplateID = packet.Decode<int>();
                    var script = action == QuestRequest.OpeningScript
                        ? template.Check[QuestState.None].StartScript
                        : template.Check[QuestState.Perform].EndScript;

                    if (script == null) return;

                    var context = new ConversationContext(user.Socket);
                    var conversation = await user.Service.ConversationManager.Build(
                        script,
                        context,
                        new FieldSpeaker(context, user.Field),
                        new QuestSpeaker(context, user, templateID, npcTemplateID)
                    );

                    await user.Converse(conversation);
                    break;
                }
            }

            await (action switch {
                QuestRequest.AcceptQuest => template.Act(QuestState.None, user),
                QuestRequest.OpeningScript => template.Act(QuestState.None, user),
                _ => Task.CompletedTask
                });

            await user.Message($"{action} : {templateID}");
            await user.ModifyStats(exclRequest: true);
        }
    }
}