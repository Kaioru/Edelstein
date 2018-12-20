namespace Edelstein.Service.Game.Conversations.Scripts
{
    public interface IScriptConversation : IConversation
    {
        string ScriptPath { get; }
    }
}