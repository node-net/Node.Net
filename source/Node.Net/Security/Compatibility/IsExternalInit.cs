#if IS_FRAMEWORK || NETSTANDARD2_0
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Compatibility shim for IsExternalInit attribute required by record types in .NET Framework 4.8 and .NET Standard 2.0.
    /// </summary>
    internal static class IsExternalInit
    {
    }
}
#endif
