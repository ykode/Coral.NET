// Base.cs
// compile with: /doc: Base.xml
using System;
using System.Collections.Generic;

/// <summary>
/// The Core Namespace, all base classes are written here
/// </summary>
namespace Coral.Core
{
  public interface ICommand<S> where S: struct {}

  public interface IEvent<S> where S: struct {}

  public interface IDGenerator<out T> where T: struct {
    T Generate();
  }

  public interface Aggregate<S>
    where S: struct
  {
    S Zero {get;}
    S Apply(S state, IEvent<S> evt);
    List<IEvent<S>> Exec(S state, ICommand<S> cmd);
  } 
}