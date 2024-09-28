using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Gameplay.Game;

public interface IFieldSetManager :
    IRepositoryMethodInsert<string, IFieldSet>,
    IRepositoryMethodRetrieve<string, IFieldSet>,
    IRepositoryMethodRetrieveAll<string, IFieldSet>;
