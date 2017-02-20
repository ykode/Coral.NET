using System;

namespace Coral.Core
{
 public struct EventInfo<I, S>
    where S: struct
    where I: class
  {
    public I           EntityId { get; }
    public IEvent<S>   Event { get; }
    public int         Version { get; }
    public DateTime    TimeStamp { get; }    

    private EventInfo( IEvent<S> evt, I entityId,
                      int version, 
                      DateTime timestamp) 
    {
      EntityId = entityId; Event = evt;
      Version  = version; TimeStamp = timestamp;
    }

    public Builder newBuilder(IEvent<S> evt, I entityId, int version) {
      return new Builder(evt, entityId, version);
    }

    public class Builder 
    {
      private IEvent<S> _evt;
      private I _entityId;
      private int _version;
      private DateTime _timestamp;

      internal EventInfo<I, S> Build() {
        return new EventInfo<I,S>(_evt, _entityId, _version, _timestamp);
      }

      internal Builder(IEvent<S> evt, I entityId, int version): 
        this(evt, entityId, version, DateTime.UtcNow)
      {
      }

      internal Builder(IEvent<S> evt, I entityId, int version, 
        DateTime timestamp) 
      {
        _evt = evt; _entityId = entityId; _version = version;
        _timestamp = timestamp;
      }

      public EventInfo<I, S>.Builder Timestamp(DateTime newTimestamp) {
        _timestamp = newTimestamp;
        return this;
      }
    }
  } 
}