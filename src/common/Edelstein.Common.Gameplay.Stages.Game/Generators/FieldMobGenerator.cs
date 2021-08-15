using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators
{
    public class FieldMobGenerator : IFieldGenerator
    {
        public bool Check(DateTime now, IField field)
            => false;

        public Task Generate(IField field)
        {
            // TODO;
            return Task.CompletedTask;
        }
    }
}
