#if IS_FRAMEWORK
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Compatibility shim for IsExternalInit attribute required by record types in .NET Framework 4.8.
    /// </summary>
    internal static class IsExternalInit
    {
    }
}
#endif
