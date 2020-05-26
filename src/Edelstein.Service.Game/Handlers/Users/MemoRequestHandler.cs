using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social.Memo;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Utils.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Fields.Objects.User.Messages.Impl;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class MemoRequestHandler : AbstractFieldUserHandler
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            var type = (MemoRequestType) packet.DecodeByte();

            switch (type)
            {
                case MemoRequestType.Delete:
                {
                    var memos = user.Memos.Values;
                    var inc = (short) memos.Count(m => m.Flag > 0);

                    await user.Service.MemoManager.BulkDelete(memos);

                    if (inc > 0)
                    {
                        await user.ModifyStats(s => s.POP += inc);
                        await user.Message(new IncPOPMessage(inc));
                    }

                    break;
                }
                default:
                    Logger.Warn($"Unhandled memo request type: {type}");
                    break;
            }
        }
    }
}