using System;
using System.Collections.Generic;
using System.Linq;

namespace Coral.Core {
  public class CommandHandler<TIdentity, TState>
    where TIdentity: struct
    where TState: struct
  {
    private IAggregate<TState> _aggregate;
    private IEventStore<TIdentity, TState> _eventStore;
    private IIDGenerator<TIdentity> _generator;

    public CommandHandler(IAggregate<TState> aggregate, IEventStore<TIdentity, TState> store, 
      IIDGenerator<TIdentity> generator) 
    {
      _aggregate = aggregate; _eventStore = store; _generator = generator;
    }

    public void handle(TIdentity? id, ICommand<TState> command, Action<IEnumerable<EventInfo<TIdentity,TState>>> success, 
      Action<Exception> failure)
    {
      if (null == id) {
        try {
          var evts = _aggregate.Exec(_aggregate.Zero, command);
          var newId  = _generator.Generate();
          var newEvents = evts.Zip(Enumerable.Range(0, evts.Count), 
            (ev, v) => EventInfo<TIdentity, TState>.NewBuilder(ev, newId, v).Build());
          success.Invoke(newEvents);

        }
        catch(Exception e) {
          failure.Invoke(e);
        }
      }
      else {
        try {
          _eventStore.load(id.Value, (evts => {
            var state = evts.OrderBy(x => x.Version)
              .Select( x => x.Event)
              .Aggregate(_aggregate.Zero, (r, e) => _aggregate.Apply(r, e));
          
            var lastVer = evts.Last().Version;
            var results = _aggregate.Exec(state, command);
            var infos = results.Zip(Enumerable.Range(lastVer + 1, lastVer + results.Count),
              (e, v) => EventInfo<TIdentity, TState>.NewBuilder(e, id.Value, v).Build());
            success.Invoke(infos);
          }), failure);
        }
        catch (Exception e) {
          failure.Invoke(e);
        }
      }
    }
  }
}