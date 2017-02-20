using System;

namespace Coral.Core {

  public struct CommandInfo<I, S>
    where S: struct
    where I: struct
  {
    public ICommand<S> Command { get; }
    public Nullable<I> EntityId { get; }
    public int?        TargetVersion { get; }

    private CommandInfo(ICommand<S> cmd, I? entityId, int? targetVersion) {
      Command = cmd; EntityId = entityId; TargetVersion = targetVersion;
    }

    public static Builder NewBuilder(ICommand<S> cmd) {
      return new Builder(cmd);
    }

    public Builder CopyBuilder() {
      return new Builder(this.Command, this.EntityId, this.TargetVersion);
    }

    public class Builder {
      private ICommand<S> _cmd;
      private I? _entityId;
      private int? _targetVersion;

      internal Builder(ICommand<S> cmd, I? entityId, int? targetVersion) {
        _cmd = cmd; _entityId = entityId; _targetVersion = targetVersion;
      }

      internal Builder(ICommand<S> cmd): this(cmd, null, null)
      {

      }

      public Builder EntityId(I entityId) {
        _entityId = entityId;
        return this;
      }

      public Builder TargetVersion(int? targetVersion) {
        _targetVersion = targetVersion;
        return this;
      }

      public Builder Command(ICommand<S> command) {
        _cmd = command;
        return this;
      }

      public CommandInfo<I, S> Build() {
        return new CommandInfo<I,S>(_cmd, _entityId, _targetVersion);
      }
    }
  }
}