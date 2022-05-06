
#if NET5_0_OR_GREATER

using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(IsExternalInit))]

#else
using System.ComponentModel;

namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class IsExternalInit { }
}

#endif