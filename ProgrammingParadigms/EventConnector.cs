using System;
using System.Collections.Generic;

namespace ProgrammingParadigms
{
    /// <summary>
    /// Events or observer pattern (publish/subscibe without data.
    /// Can be asynchronous or synchronous
    /// No data, can be bidirectional.
    /// Analogous to Reactive Extensions without the duality with Iteration - the flow only uses hot observables, and never completes.
    /// </summary>
    public interface IEvent
    {
        void Execute();
    }

    public delegate void CallBackDelegate();

    /// <summary>
    /// A reversed IEvent. The IEvent pushes event to the destination whereas the IEventB pulls data from source.
    /// However, the destination will be notified when event happens at source.
    /// </summary>
    public interface IEventB
    {
        event CallBackDelegate EventHappened;
    }

    /// <summary>
    /// It fans out IEvent by creating a list and assign the event to the element in the list.
    /// Moreover, any IEvent and IEventB can be transferred bidirectionally.
    /// ----------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent IEvent: incoming event which wants to be fanned out
    /// 2. IEventB IEvent_B: the 'b port' which listens for when event happens again within this EventConnector or acts as an IEvent_B adapter
    /// 3. List<IEvent> fanoutList: output port that fans out to every abstraction connected in order of wiring
    /// 4. IEvent last: output port that will execute after the fanoutList and IEvent_B data changed event. This enables chaining of events and explicit execution of last to execute the event.
    /// </summary>
    public class EventConnector : IEvent, IEventB // IEvent, IEvent_B
    {
        // Properties
        public string InstanceName;

        // ports
        private List<IEvent> fanoutList = new List<IEvent>();
        private IEvent last;

        /// <summary>
        /// Fans out an IEvent to mutiple IEvents, or connect IEvent and IEventB
        /// </summary>
        public EventConnector() { }

        // IEvent implementation ------------------------------------
        void IEvent.Execute()
        {
            foreach (var fanout in fanoutList) fanout.Execute();
            EventHappened?.Invoke();
            last?.Execute();
        }

        // IEventB implementation --------------------------------------
        public event CallBackDelegate EventHappened;
    }
}
