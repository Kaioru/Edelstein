using System;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Generators
{
    public interface AbstractFieldMobGenerator
    {
        bool Check(DateTime now, IField field);
        Task Generate(IField field);
    }
}
