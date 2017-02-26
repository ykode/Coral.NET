using System;

namespace Coral.Core {
  public interface Serialiser<TObject, TSerialisedObject>
  {
    TSerialisedObject serialise(TObject obj);
    TObject deserialise(TSerialisedObject data);

  }
}