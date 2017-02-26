using System;

namespace Coral.Core
{
 public struct EventInfo<TIdentity, TState>
    where TState: struct
    where TIdentity: struct
  {
    public TIdentity           EntityId { get; }
    public IEvent<TState>   Event { get; }
    public int         Version { get; }
    public DateTime    TimeStamp { get; }    

    private EventInfo( IEvent<TState> evt, TIdentity entityId,
                      int version, 
                      DateTime timestamp) 
    {
      EntityId = entityId; Event = evt;
      Version  = version; TimeStamp = timestamp;
    }

    public static Builder NewBuilder(IEvent<TState> evt, TIdentity entityId, int version) {
      return new Builder(evt, entityId, version);
    }

    public Builder CopyBuilder() {
      return new Builder(this.Event, this.EntityId, this.Version, this.TimeStamp);
    }

    public class Builder 
    {
      private IEvent<TState> _evt;
      private TIdentity _entityId;
      private int _version;
      private DateTime _timestamp;

      public EventInfo<TIdentity, TState> Build() {
        return new EventInfo<TIdentity,TState>(_evt, _entityId, _version, _timestamp);
      }

      internal Builder(IEvent<TState> evt, TIdentity entityId, int version): 
        this(evt, entityId, version, DateTime.UtcNow)
      {
      }

      internal Builder(IEvent<TState> evt, TIdentity entityId, int version, 
        DateTime timestamp) 
      {
        _evt = evt; _entityId = entityId; _version = version;
        _timestamp = timestamp;
      }


      public Builder Event(IEvent<TState> evt) {
        _evt = evt;
        return this;
      }

      public Builder EntityId(TIdentity entityId) {
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
