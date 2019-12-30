namespace Edelstein.Database.Entities.Characters
{
    public enum KeyType : byte
    {
        None = 0x0,
        Skill = 0x1,
        Item = 0x2,
        Emotion = 0x3,
        Menu = 0x4,
        BasicAction = 0x5,
        BasicEmotion = 0x6,
        Effect = 0x7,
        Macroskill = 0x8
    }
}