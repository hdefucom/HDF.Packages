
#if !NET5_0_OR_GREATER


using System.ComponentModel;

namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public class IsExternalInit { }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
}

#endif