using Coral.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;

namespace Coral.Core.Test
{
  using InfoList  = IEnumerable<EventInfo<Guid, Person>>;
  using SuccessCB = Action<IEnumerable<EventInfo<Guid, Person>>>;
  using FailureCB = Action<Exception>;
  public class CommandHandlerTest
  {
    private Mock<IEventStore<Guid, Person>> storageMock;
    private Mock<IAggregate<Person>> aggregateMock;
    private Mock<IIDGenerator<Guid>> idGeneratorMock;

    public CommandHandlerTest() {
      storageMock = new Mock<IEventStore<Guid, Person>>();
      aggregateMock = new Mock<IAggregate<Person>>();
      idGeneratorMock = new Mock<IIDGenerator<Guid>>();
    }

    private void resetMocks() {
      storageMock.Reset(); aggregateMock.Reset(); idGeneratorMock.Reset();
    }

    [Fact]
    public void TestCommandHandler()
    {

      //Given
      this. resetMocks();
      var fakeId = Guid.Parse("CB6CA9A0-6DDA-4CDB-82A8-639753740B5F");
      var existId = Guid.Parse("560287C3-3A56-4F3D-9B1C-BF76BF1B0484");
      var createPersonCommand = new CreatePerson("Didiet", 29);
      var personCreatedEvents = new List<IEvent<Person>> { new PersonCreated("Didiet", 29) };
      var personCreatedEventInfo = personCreatedEvents
        .Zip(Enumerable.Range(0, personCreatedEvents.Count), (e, v) =>
          EventInfo<Guid, Person>.NewBuilder(e, fakeId, v).Build()
        );
      idGeneratorMock.Setup(gen => gen.Generate()).Returns(fakeId);
      aggregateMock.Setup(agg => agg.Exec(It.IsAny<Person>(), createPersonCommand))
        .Returns(personCreatedEvents);
 
      storageMock
        .Setup(store => store.load(fakeId, It.IsAny<SuccessCB>(), It.IsAny<FailureCB>()))
        .Callback<Guid, SuccessCB, FailureCB>(
          (guid, success, failure) => success.Invoke(new List<EventInfo<Guid, Person>>())
      );

      storageMock
        .Setup(store => store.load(existId, It.IsAny<SuccessCB>(), It.IsAny<FailureCB>()))
        .Callback<Guid, SuccessCB, FailureCB>(
          (guid, success, failure) => success.Invoke(personCreatedEventInfo)
      );

      var commandHandler = new CommandHandler<Guid, Person>(aggregateMock.Object,
        storageMock.Object, idGeneratorMock.Object);

      //When
      var successReceiver = new Mock<SuccessCB>();
      var failureReceiver = new Mock<FailureCB>();
      commandHandler.handle(null, createPersonCommand,
        successReceiver.Object, failureReceiver.Object);

      //Then
      idGeneratorMock.Verify(mock => mock.Generate(), Times.Once);
      aggregateMock.Verify(agg => agg.Exec(It.IsAny<Person>(), createPersonCommand), Times.Once);
      successReceiver.Verify(cb => cb(It.IsAny<InfoList>()), Times.Once);
      failureReceiver.Verify(cb => cb(It.IsAny<Exception>()), Times.Never); 
    }
  }
}