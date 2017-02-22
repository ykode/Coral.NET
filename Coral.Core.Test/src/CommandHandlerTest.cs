using Coral.Core;
using System;
using System.Collections.Generic;
using Xunit;
using Moq;

namespace Coral.Core.Test {
  class CommandHandlerTest {
    [Fact]
    public void TestCommandHandler()
    {
    //Given
    var storageMock = new Mock<IEventStore<Guid, Person>>();
    var aggregateMock = new Mock<IAggregate<Person>>();
    var idGeneratorMock = new Mock<IIDGenerator<Guid>>();
    
    var createPersonCommand = new CreatePerson("Didiet", 29);
    var personCreatedEvents = new List<IEvent<Person>>{new PersonCreated("Didiet", 29)};
    var fakeId = Guid.Parse("CB6CA9A0-6DDA-4CDB-82A8-639753740B5F");

    idGeneratorMock.Setup(gen => gen.Generate()).Returns(fakeId);
    aggregateMock.Setup(agg => agg.Zero).Returns(new Person("", 0));
    aggregateMock.Setup(agg => agg.Exec(agg.Zero, createPersonCommand))
      .Returns(personCreatedEvents);

    var commandHandler = new CommandHandler<Guid, Person>(aggregateMock.Object, 
      storageMock.Object, idGeneratorMock.Object);

    //When
    
    //Then
    }
  }
}