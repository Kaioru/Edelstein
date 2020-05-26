using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Entities.Characters;
using Edelstein.Core.Gameplay;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Core.Gameplay.Extensions.Packets;
using Edelstein.Core.Gameplay.Inventories;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Templates.Etc.MakeCharInfo;
using Edelstein.Core.Templates.Items;
using Edelstein.Core.Utils.Packets;
using Edelstein.Service.Login.Logging;
using Edelstein.Service.Login.Types;

namespace Edelstein.Service.Login.Handlers
{
    public class CreateNewCharacterHandler : AbstractPacketHandler<LoginServiceAdapter>
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        protected override async Task Handle(
            LoginServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            var name = packet.DecodeString();
            var race = (Race) packet.DecodeInt();
            var subJob = packet.DecodeShort();
            var face = packet.DecodeInt();
            var hair = packet.DecodeInt();
            var hairColor = packet.DecodeInt();
            var skin = packet.DecodeInt();
            var coat = packet.DecodeInt();
            var pants = packet.DecodeInt();
            var shoes = packet.DecodeInt();
            var weapon = packet.DecodeInt();
            var gender = packet.DecodeByte();

            if (adapter.Account == null) return;
            if (adapter.AccountWorld == null) return;
            if (adapter.SelectedNode == null) return;

            var token = new CancellationTokenSource();

            token.CancelAfter(TimeSpan.FromSeconds(5));

            var createLock =
                await adapter.Service.LockProvider.AcquireAsync(
                    LoginService.CreateCharLockKey,
                    cancellationToken: token.Token
                );

            if (createLock == null)
            {
                using var p = new OutPacket(SendPacketOperations.CreateNewCharacterResult);

                p.EncodeByte((byte) LoginResultCode.Timeout);
                p.EncodeInt(0);

                await adapter.SendPacket(p);
                return;
            }

            try
            {
                using var p = new OutPacket(SendPacketOperations.CreateNewCharacterResult);
                using var store = adapter.Service.DataStore.StartSession();

                var result = LoginResultCode.Success;
                var templates = adapter.Service.TemplateManager;
                var makeCharInfo = templates.Get<MakeCharInfoTemplate>((int) (race switch
                {
                    Race.Normal => MakeCharInfoType.Normal,
                    Race.Cygnus => MakeCharInfoType.Premium,
                    Race.Aran => MakeCharInfoType.Orient,
                    Race.Evan => MakeCharInfoType.Evan,
                    Race.Resistance => MakeCharInfoType.Resistance,
                    _ => MakeCharInfoType.Normal
                }) * 0x2 + gender);

                if (makeCharInfo.Face.All(i => i != face) ||
                    makeCharInfo.Hair.All(i => i != hair) ||
                    makeCharInfo.HairColor.All(i => i != hairColor) ||
                    makeCharInfo.Skin.All(i => i != skin) ||
                    makeCharInfo.Coat.All(i => i != coat) ||
                    makeCharInfo.Pants.All(i => i != pants) ||
                    makeCharInfo.Shoes.All(i => i != shoes) ||
                    makeCharInfo.Weapon.All(i => i != weapon)
                ) result = LoginResultCode.Unknown;
                if (adapter.Service.State.Worlds
                        .FirstOrDefault(w => w.ID == adapter.AccountWorld.WorldID)
                        ?.BlockCharCreation == true
                ) result = LoginResultCode.NotConnectableWorld;

                p.EncodeByte((byte) result);

                if (result == LoginResultCode.Success)
                {
                    var character = new Character
                    {
                        AccountWorldID = adapter.AccountWorld.ID,
                        Name = name,
                        Job = GameConstants.GetStartJob(race),
                        Face = face,
                        Hair = hair + hairColor,
                        Skin = (byte) skin,
                        Gender = gender,
                        FieldID = GameConstants.GetStartField(race),
                        FieldPortal = 0,
                        SubJob = (short) (race == Race.Normal ? subJob : 0)
                    };
                    var context = new ModifyInventoriesContext(character.Inventories);

                    context.Set(BodyPart.Clothes, templates.Get<ItemTemplate>(coat));
                    context.Set(BodyPart.Shoes, templates.Get<ItemTemplate>(shoes));
                    context.Set(BodyPart.Weapon, templates.Get<ItemTemplate>(weapon));
                    if (pants > 0)
                        context.Set(BodyPart.Pants, templates.Get<ItemTemplate>(pants));

                    await store.InsertAsync(character);

                    Logger.Debug($"Created new {race} character, {name}");

                    character.EncodeStats(p);
                    character.EncodeLook(p);
                    p.EncodeBool(false);
                    p.EncodeBool(false);
                }
                else
                {
                    p.EncodeInt(0);
                }

                await adapter.SendPacket(p);
            }
            catch
            {
                using var p = new OutPacket(SendPacketOperations.CreateNewCharacterResult);

                p.EncodeByte((byte) LoginResultCode.DBFail);
                p.EncodeInt(0);

                await adapter.SendPacket(p);
            }
            finally
            {
                await createLock.ReleaseAsync();
            }
        }
    }
}