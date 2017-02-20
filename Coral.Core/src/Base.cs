// Base.cs
// compile with: /doc: Base.xml
using System;

/// <summary>
/// The Core Namespace, all base classes are written here
/// </summary>
namespace Coral.Core
{
  interface ICommand<S> where S: struct {}

  interface IEvent<S> where S: struct {}

  interface IIDGenerator<out T> where T: class {
    T Generate();
  }

  interface Aggregate<S>
    where S: struct
  {
    S Zero {get;}
    S Apply(S state, IEvent<S> evt);
    S Exec(S state, ICommand<S> cmd);
  } 
}