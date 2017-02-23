using System;

namespace Coral.Core {
  public interface Serialiser<T, U>
  {
    U serialise(T obj);
    T deserialise(U data);

  }
}