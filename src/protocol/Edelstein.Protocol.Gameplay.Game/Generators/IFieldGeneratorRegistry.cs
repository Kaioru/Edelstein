using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Gameplay.Game.Generators;

public interface IFieldGeneratorRegistry :
    IRepositoryMethodInsert<string, IFieldGenerator>,
    IRepositoryMethodDelete<string, IFieldGenerator>,
    IRepositoryMethodRetrieveAll<string, IFieldGenerator>;
