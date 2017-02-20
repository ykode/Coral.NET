using System;

namespace Coral.Core {

  struct CommandInfo<I, S>
    where S: struct
    where I: class
  {
    public ICommand<S> Command { get; }
    public I           EntityId { get; }
    public int?        TargetVersion { get; }

    private CommandInfo(ICommand<S> cmd, I entityId, int? targetVersion) {
      Command = cmd; EntityId = entityId; TargetVersion = targetVersion;
    }

    public class Builder {
      private ICommand<S> _cmd;
      private I _entityId;
      private int? _targetVersion;

      internal Builder(ICommand<S> cmd, I entityId, int? targetVersion) {
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

      public CommandInfo<I, S> Build() {
        return new CommandInfo<I,S>(_cmd, _entityId, _targetVersion);
      }
    }
  }
}