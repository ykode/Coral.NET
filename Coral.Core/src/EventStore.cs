using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coral.Core {
  public interface IEventStore<TIdentity, TState>
    where TIdentity: struct 
    where TState: struct
  {
    void load(TIdentity id, Action<IEnumerable<EventInfo<TIdentity,TState>>> success, Action<Exception> failure);
    void load(TIdentity id, int version, Action<IEnumerable<EventInfo<TIdentity,TState>>> success, Action<Exception> failure);
    void commit(TIdentity id, EventInfo<TIdentity,TState> eventInfo, Action<TIdentity> success, Action<Exception> failure);
  } 
}