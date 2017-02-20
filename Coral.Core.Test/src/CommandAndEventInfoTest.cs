using Xunit;
using System;
using Coral.Core;

internal struct Person {
  public string name {get;}
  public int age {get;}

  public Person(string name, int age) {
    this.name = name; this.age = age;
  }
}

internal struct CreatePerson: ICommand<Person> {
  public string name {get; }
  public int age {get;}

  public CreatePerson(string name, int age) {
    this.name = name; this.age = age;
  }
}

internal struct PersonCreated: IEvent<Person> {
  public string name {get; }
  public int age {get; }

  public PersonCreated(string name, int age) {
    this.name = name; this.age = age;
  }
}

internal struct PersonNameChanged: IEvent<Person> {
  public string name {get; }
  public PersonNameChanged(string name) {this.name = name;}
}

namespace Coral.Core.Test
{
  public class CoralCoreTest
  {
    [Fact]
    public void TestCommandInfoCreation()
    {
      var id = Guid.Parse("A9516716-C3D7-40FE-9CC1-065D5111CBBA");
      var cmd = new CreatePerson("Didiet", 20);
      var cmd2 = new CreatePerson("Dina", 23);
      var cmdInfo = CommandInfo<Guid, Person>.NewBuilder(cmd).Build();
      Assert.NotNull(cmdInfo);

      var cmdInfo2 = cmdInfo.CopyBuilder().Command(cmd2).Build();
      Assert.NotNull(cmdInfo2);
      Assert.IsType<CreatePerson>(cmdInfo2.Command);
      Assert.Equal(cmdInfo.EntityId, cmdInfo2.EntityId);
    }

    [Fact]
    public void TestEventInfoCreation()
    {
      var id = Guid.Parse("A9516716-C3D7-40FE-9CC1-065D5111CBBA");
      var evt1 = new PersonCreated("Didiet", 20);
      var evtInfo = EventInfo<Guid, Person>.NewBuilder(evt1, id, 0).Build();
      var evtInfo2 = evtInfo.CopyBuilder().Version(1).Event(new PersonNameChanged("Noor")).Build();

      Assert.NotNull(evtInfo);
      Assert.NotNull(evtInfo2);
    }
  }
}
