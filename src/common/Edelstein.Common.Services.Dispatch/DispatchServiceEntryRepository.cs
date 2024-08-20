using Edelstein.Common.Utilities.Repositories;

namespace Edelstein.Common.Services.Dispatch;

public class DispatchServiceEntryRepository<TKey> : Repository<TKey, DispatchServiceEntry<TKey>> 
    where TKey : notnull;
