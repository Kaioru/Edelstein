using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social.Memo;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Logging;
using MoreLinq;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class MemoRequestHandler : AbstractFieldUserHandler
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var type = (MemoRequestType) packet.Decode<byte>();

            switch (type)
            {
                case MemoRequestType.Delete:
                {
                    var memos = user.Memos.Values;
                    await user.Service.MemoManager.BulkDelete(memos);
                    break;
                }
                default:
                    Logger.Warn($"Unhandled memo request type: {type}");
                    break;
            }
        }
    }
}