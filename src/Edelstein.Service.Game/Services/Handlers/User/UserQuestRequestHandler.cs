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

            // TODO: check quest state

            var result = action switch {
                QuestRequest.AcceptQuest => await template.Check(QuestState.None, user),
                QuestRequest.OpeningScript => await template.Check(QuestState.None, user),
                QuestRequest.CompleteQuest => await template.Check(QuestState.Perform, user),
                QuestRequest.CompleteScript => await template.Check(QuestState.Perform, user),
                _ => QuestResult.ActSuccess
                };
            if (result == QuestResult.ActSuccess)
                result = action switch {
                    QuestRequest.AcceptQuest => await template.Act(QuestState.None, user),
                    QuestRequest.CompleteQuest => await template.Act(QuestState.Perform, user),
                    _ => QuestResult.ActSuccess
                    };

            if (result != QuestResult.ActSuccess)
            {
                using (var p = new Packet(SendPacketOperations.UserQuestResult))
                {
                    p.Encode<byte>((byte) result);
                    await user.SendPacket(p);
                }

                return;
            }

            await (action switch {
                QuestRequest.AcceptQuest => user.ModifyQuests(q => q.Accept(templateID)),
                QuestRequest.CompleteQuest => user.ModifyQuests(q => q.Complete(templateID)),
                QuestRequest.ResignQuest => user.ModifyQuests(q => q.Resign(templateID)),
                _ => Task.CompletedTask
                });

            switch (action)
            {
                case QuestRequest.AcceptQuest:
                case QuestRequest.CompleteQuest:
                {
                    var npcTemplateID = packet.Decode<int>();

                    using (var p = new Packet(SendPacketOperations.UserQuestResult))
                    {
                        p.Encode<byte>((byte) QuestResult.ActSuccess);
                        p.Encode<short>(templateID);
                        p.Encode<int>(npcTemplateID);
                        p.Encode<int>(0); // nextQuest

                        await user.SendPacket(p);
                    }

                    break;
                }

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
        }
    }
}