using System;
using System.Collections.Generic;
using System.Linq;

namespace Coral.Core {
  class CommandHandler<I, S>
    where I: struct
    where S: struct
  {
    private Aggregate<S> _aggregate;
    private EventStore<I, S> _eventStore;
    private IDGenerator<I> _generator;

    CommandHandler(Aggregate<S> aggregate, EventStore<I, S> store, 
      IDGenerator<I> generator) 
    {
      _aggregate = aggregate; _eventStore = store; _generator = generator;
    }

    void handle(I? id, ICommand<S> command, Action<IEnumerable<EventInfo<I,S>>> success, 
      Action<Exception> failure)
    {
      if (null == id) {
        try {
          var evts = _aggregate.Exec(_aggregate.Zero, command);
          var newId  = _generator.Generate();
          var newEvents = evts.Zip(Enumerable.Range(0, evts.Count), 
            (ev, v) => EventInfo<I, S>.NewBuilder(ev, newId, v).Build());
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
              (e, v) => EventInfo<I, S>.NewBuilder(e, id.Value, v).Build());
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