using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Network;

public interface ISocketRepository :
    IRepositoryMethodRetrieve<string, ISocket>,
    IRepositoryMethodRetrieveAll<string, ISocket>;
