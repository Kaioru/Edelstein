namespace Edelstein.Plugin.Rue.ClientAnalysis.RuntimeFunctionDumping;

public enum RegisterValueKind
{
    Unknown = 0,
    StructBase = 1,
    SingletonPointerAddress = 2
}

public readonly record struct RegisterTypeInfo(string TypeName, int BaseOffset, RegisterValueKind Kind);
