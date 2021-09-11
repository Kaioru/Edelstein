namespace Edelstein.Protocol.Gameplay.Users.Keys
{
    public record CharacterFunctionKey
    {
        public byte Type { get; set; }
        public int Action { get; set; }

        public CharacterFunctionKey()
        {
        }

        public CharacterFunctionKey(KeyType type, KeyMenu action)
        {
            Type = (byte)type;
            Action = (int)action;
        }
    }
}
