namespace Edelstein.Service.Game.Conversations.Scripts
{
    public interface IScriptConversationManager
    {
        string ScriptPath { get; }

        IScriptConversation Get(
            string name,
            IConversationContext context,
            ISpeaker self,
            ISpeaker target
        );
    }
}