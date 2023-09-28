using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public class SayImageRequest : AbstractConversationMessageRequest<byte>
{
    private readonly string[] _images;

    public SayImageRequest(IConversationSpeaker speaker, string[] images) : base(speaker) => _images = images;

    public override ConversationMessageType Type => ConversationMessageType.SayImage;
    
    protected override void WriteData(IPacketWriter writer)
    {
        writer.WriteByte((byte)_images.Length);
        foreach (var image in _images)
            writer.WriteString(image);
    }
}
