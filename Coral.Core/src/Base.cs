// Base.cs
// compile with: /doc: Base.xml
using System;
using System.Collections.Generic;

/// <summary>
/// The Core Namespace, all base classes are written here
/// </summary>
namespace Coral.Core
{
  public interface ICommand<TState> where TState: struct {}

  public interface IEvent<TState> where TState: struct {}

  public interface IIDGenerator<out TIdentity> where TIdentity: struct {
    TIdentity Generate();
  }

  public interface IAggregate<TState>
    where TState: struct
  {
    TState Zero {get;}
    TState Apply(TState state, IEvent<TState> evt);
    List<IEvent<TState>> Exec(TState state, ICommand<TState> cmd);
  } 
}