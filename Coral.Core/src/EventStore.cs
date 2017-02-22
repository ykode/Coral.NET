using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coral.Core {
  public interface IEventStore<I, S>
    where I: struct 
    where S: struct
  {
    void load(I id, Action<IEnumerable<EventInfo<I,S>>> success, Action<Exception> failure);
    void load(I id, int version, Action<IEnumerable<EventInfo<I,S>>> success, Action<Exception> failure);
    void commit(I id, EventInfo<I,S> eventInfo, Action<I> success, Action<Exception> failure);
  } 
}