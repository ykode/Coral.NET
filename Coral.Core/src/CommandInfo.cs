using System;

namespace Coral.Core {

  public struct CommandInfo<TIdentity, TState>
    where TState: struct
    where TIdentity: struct
  {
    public ICommand<TState> Command { get; }
    public Nullable<TIdentity> EntityId { get; }
    public int?        TargetVersion { get; }

    private CommandInfo(ICommand<TState> cmd, TIdentity? entityId, int? targetVersion) {
      Command = cmd; EntityId = entityId; TargetVersion = targetVersion;
    }

    public static Builder NewBuilder(ICommand<TState> cmd) {
      return new Builder(cmd);
    }

    public Builder CopyBuilder() {
      return new Builder(this.Command, this.EntityId, this.TargetVersion);
    }

    public class Builder {
      private ICommand<TState> _cmd;
      private TIdentity? _entityId;
      private int? _targetVersion;

      internal Builder(ICommand<TState> cmd, TIdentity? entityId, int? targetVersion) {
        _cmd = cmd; _entityId = entityId; _targetVersion = targetVersion;
      }

      internal Builder(ICommand<TState> cmd): this(cmd, null, null)
      {

      }

      public Builder EntityId(TIdentity entityId) {
        _entityId = entityId;
        return this;
      }

      public Builder TargetVersion(int? targetVersion) {
        _targetVersion = targetVersion;
        return this;
      }

      public Builder Command(ICommand<TState> command) {
        _cmd = command;
        return this;
      }

      public CommandInfo<TIdentity, TState> Build() {
        return new CommandInfo<TIdentity,TState>(_cmd, _entityId, _targetVersion);
      }
    }
  }
}