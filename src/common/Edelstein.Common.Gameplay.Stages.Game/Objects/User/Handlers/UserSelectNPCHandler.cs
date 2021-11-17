using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Conversations;
using Edelstein.Common.Gameplay.Stages.Game.Conversations.Scripted;
using Edelstein.Common.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class UserSelectNPCHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.UserSelectNpc;

        protected override async Task<bool> Check(GameStageUser stageUser, IFieldObjUser user)
            => await base.Check(stageUser, user) && !user.IsConversing && !user.IsDialoging;

        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            var npc = user.Field.GetPool(FieldObjType.NPC).GetObject<IFieldObjNPC>(packet.ReadInt());

            if (npc == null) return;
            if (!user.Watching.Any(w => w == npc.FieldSplit)) return;

            var scriptName = npc.Info.Script;

            if (string.IsNullOrWhiteSpace(scriptName)) return;

            try
            {
                var script = await stageUser.Stage.ScriptEngine.CreateFromFile(scriptName);
                var context = new ConversationContext(user);
                var conversation = new ScriptedConversation(
                    context,
                    new BasicSpeaker(context, templateID: npc.Info.ID),
                    new BasicSpeaker(context, flags: ConversationSpeakerFlags.NPCReplacedByUser),
                    scriptName,
                    script
                );

                await user.Converse(conversation);
            }
            catch (FileNotFoundException)
            {
                await user.Message($"The '{scriptName}' script does not exist.");
            }
        }
    }
}
