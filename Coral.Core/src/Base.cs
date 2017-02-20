// Base.cs
// compile with: /doc: Base.xml
using System;

/// <summary>
/// The Core Namespace, all base classes are written here
/// </summary>
namespace Coral.Core
{
  public interface ICommand<S> where S: struct {}

  public interface IEvent<S> where S: struct {}

  public interface IIDGenerator<out T> where T: class {
    T Generate();
  }

  public interface Aggregate<S>
    where S: struct
  {
    S Zero {get;}
    S Apply(S state, IEvent<S> evt);
    S Exec(S state, ICommand<S> cmd);
  } 
}