namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskAcceptMessage : AskYesNoMessage
    {
        public override ConversationMessageType Type => ConversationMessageType.AskAccept;

        public AskAcceptMessage(
            ISpeaker speaker,
            string text
        ) : base(speaker, text)
        {
        }
    }
}