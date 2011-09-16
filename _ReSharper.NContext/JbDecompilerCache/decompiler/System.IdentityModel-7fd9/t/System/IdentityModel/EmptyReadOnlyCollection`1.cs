// Type: System.IdentityModel.EmptyReadOnlyCollection`1
// Assembly: System.IdentityModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.IdentityModel.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.IdentityModel
{
  internal static class EmptyReadOnlyCollection<T>
  {
    public static ReadOnlyCollection<T> Instance = new ReadOnlyCollection<T>((IList<T>) new List<T>());

    static EmptyReadOnlyCollection()
    {
    }
  }
}
