using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Gameplay.Game;

public interface IFieldManager :
    IRepositoryMethodRetrieve<int, IField>,
    IRepositoryMethodRetrieveAll<int, IField>;
