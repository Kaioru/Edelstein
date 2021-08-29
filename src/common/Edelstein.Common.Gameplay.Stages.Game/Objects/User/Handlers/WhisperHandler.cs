using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Types;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services.Contracts;
using Google.Protobuf;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class WhisperHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.Whisper;

        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            var stage = stageUser.Stage;
            var flag = (WhisperFlags)packet.ReadByte();

            switch (flag)
            {
                case WhisperFlags.Whisper | WhisperFlags.Request:
                    {
                        _ = packet.ReadInt();
                        var name = packet.ReadString();
                        var text = packet.ReadString();

                        if (name.Equals(user.Character.Name, StringComparison.CurrentCultureIgnoreCase)) return;

                        var allowed = false;
                        var target = await stage.CharacterRepository.RetrieveByName(name);
                        var response = new UnstructuredOutgoingPacket(PacketSendOperations.Whisper);

                        if (target != null)
                        {
                            var sessionRequest = new DescribeSessionByCharacterRequest { Character = target.ID };
                            var sessionResponse = await stage.SessionRegistry.DescribeByCharacter(sessionRequest);

                            if (sessionResponse.Session.State == SessionState.LoggedIn) allowed = true;
                        }

                        response.WriteByte((byte)(WhisperFlags.Whisper | WhisperFlags.Result));
                        response.WriteString(allowed ? target.Name : name);
                        response.WriteBool(allowed);

                        if (allowed)
                        {
                            var whisper = new UnstructuredOutgoingPacket(PacketSendOperations.Whisper)
                                .WriteByte((byte)(WhisperFlags.Whisper | WhisperFlags.Receive))
                                .WriteString(user.Character.Name)
                                .WriteByte((byte)stage.ChannelID)
                                .WriteBool(false) // bFromAdmin
                                .WriteString(text);
                            var whisperRequest = new DispatchToCharactersRequest { Data = ByteString.CopyFrom(whisper.Buffer) };

                            whisperRequest.Characters.Add(target.ID);

                            await stage.DispatchService.DispatchToCharacters(whisperRequest);
                        }

                        await user.Dispatch(response);
                        break;
                    }
                default:
                    stage.Logger.LogWarning($"Unhandled whisper flag: {flag}");
                    break;
            }
        }
    }
}
