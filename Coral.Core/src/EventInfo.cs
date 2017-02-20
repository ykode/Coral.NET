using System;

namespace Coral.Core
{
 public struct EventInfo<I, S>
    where S: struct
    where I: struct
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

    public static Builder NewBuilder(IEvent<S> evt, I entityId, int version) {
      return new Builder(evt, entityId, version);
    }

    public Builder CopyBuilder() {
      return new Builder(this.Event, this.EntityId, this.Version, this.TimeStamp);
    }

    public class Builder 
    {
      private IEvent<S> _evt;
      private I _entityId;
      private int _version;
      private DateTime _timestamp;

      public EventInfo<I, S> Build() {
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


      public Builder Event(IEvent<S> evt) {
        _evt = evt;
        return this;
      }

      public Builder EntityId(I entityId) {
        _entityId = entityId;
        return this;
      }

      public Builder Version(int version) {
        _version = version;
        return this;
      }
      

      public Builder Timestamp(DateTime newTimestamp) {
        _timestamp = newTimestamp;
        return this;
      }
    }
  } 
}
