using System.Reflection;

namespace Edelstein.Common.Database;

public record GameDbContextProvider(
    string Key,
    Assembly Assembly
);
