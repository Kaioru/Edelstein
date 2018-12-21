namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskMembershopAvatarMessage : AskAvatarMessage
    {
        public override ScriptMessageType Type => ScriptMessageType.AskMembershopAvatar;
        
        public AskMembershopAvatarMessage(
            ISpeaker speaker,
            string text,
            int[] styles
        ) : base(speaker, text, styles)
        {
        }
    }
}