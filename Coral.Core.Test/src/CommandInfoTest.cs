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
    }


  }
}
