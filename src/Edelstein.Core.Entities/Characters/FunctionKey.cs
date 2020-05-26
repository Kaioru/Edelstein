namespace Edelstein.Core.Entities.Characters
{
    public class FunctionKey
    {
        public byte Type { get; set; }
        public int Action { get; set; }

        public FunctionKey()
        {
        }

        public FunctionKey(KeyType type, KeyMenu action)
        {
            Type = (byte) type;
            Action = (int) action;
        }
    }
}