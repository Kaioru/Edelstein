namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskMembershopAvatarScriptMessage : AskAvatarScriptMessage
    {
        public override ScriptMessageType Type => ScriptMessageType.AskMembershopAvatar;
        
        public AskMembershopAvatarScriptMessage(
            ISpeaker speaker,
            string text,
            int[] styles
        ) : base(speaker, text, styles)
        {
        }
    }
}