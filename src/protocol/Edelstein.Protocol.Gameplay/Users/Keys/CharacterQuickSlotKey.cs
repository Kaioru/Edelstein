namespace Edelstein.Protocol.Gameplay.Users.Keys
{
    public record CharacterQuickSlotKey
    {
        public int Key { get; set; }

        public CharacterQuickSlotKey()
        {
        }

        public CharacterQuickSlotKey(int key)
        {
            Key = key;
        }
    }
}
