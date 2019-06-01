namespace Edelstein.Service.Game.Interactions.Miniroom.Types
{
    public class TradingRoom : AbstractMiniroom
    {
        public override MiniroomType Type => MiniroomType.TradingRoom;
        public override byte MaxUsers => 2;
    }
}