using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators
{
    public interface IFieldGenerator
    {
        bool Check(DateTime now, IField field);
        Task Generate(IField field);
    }
}
