using System.Collections.Generic;
using Edelstein.Core.Distributed.States;

namespace Edelstein.Core.Gameplay.Migrations.States
{
    public class GameNodeState : DefaultServerNodeState
    {
        public byte ChannelID { get; set; }
        public ICollection<byte> Worlds { get; set; }

        public GameNodeState()
            => Scope = "Game";
    }
}