using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{

    /// <summary>
    /// Arbitrator is an ALA domain abstraction (see AbstractionLayeredArchitecture.md for more details)
    /// Abstraction description follows:
    /// When you have multiple things in your application that may want to use some resource, but that resource must be used by only one at a time,
    /// we exprerss that by wiring all the things, using the arbitration programming apardigm, to an instance of arbitrator which represents the resource.
    /// A cononical example resource is an external hardware device that should have only one thing in the application talking to it at a time, such as an SCP command device.
    /// This programming paradigm supports wiring with the IArbitrator interface, and this domain abstraction called Arbitrator,
    /// one instance of which is used for each different resource.
    /// See IArbitrator for explanation of the abstraction and example usage of the interface by domain abstractions.
    /// The arbitrator programming paradigm works with async/await - so if the resource is already busy, a requester waits without blocking the thread and coding is very easy.
    /// Any incoming requests for the same arbitrator will queue and once the current request is released, it will be given the resource
    /// It contains a configurable timer to timeout after (default) 5 seconds to force release of the resource. (By using cancellationToken)
    /// An application can have multiple resources, in which case it would use multiple instance of this domain abstraction.

    /// [Note that if the number of things using a specific resource is high, you would consider making the arbitration programming paradigm work without wiring, and use symbolic names of resources instead.
    /// You would configure arbitrator with the name of the specific resource.
    /// Each instance of other domain abstractions would also be configured with the same resource name: That name is considered an abstraction in the domain layer.
    /// However if the number of instances using an abstraction is small, we prefer to not make the specific resource name a domain layer abstraction, and simply wire
    /// all the instances to an instance of Arbitrator instead.
    /// This is an example of choosing whether or not to make something a domain abstraction (no wiring, just use the abstraction by name) or not abstraction (explicit anonymous wiring) to a common point.]

    /// ------------------------------------------------------------------------------------------------------------------
    /// Configurations: (configurations are for use by the application when it instantiates a domain abstraction)
    /// None

    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IArbitrator arbitrator: interface for wiring in all the instances of things that want to use a resource represented by this instance of Arbitrator
    /// </summary>
    public class Arbitrator : IArbitrator // arbitrator
    {
        //properties ---------------------------------------------------------------
        public string InstanceName { get; set; } = "default";

        // private fields ---------------------------------------------------------------
        private Queue<Tuple<string, TaskCompletionSource<object>>> waitingQueue = new Queue<Tuple<string, TaskCompletionSource<object>>>();
        private string occupierRequestorName;
        private CancellationTokenSource cancellationTokenSource;
        private int timeout;

        /// <summary>
        /// In charge of allowing only one instance to being using the arbitrator at any one time using an internal queue
        /// <param name="timeout">The timeout for each request. Set less than zero to not cancel.</param>
        /// </summary>
        public Arbitrator(int timeout = 5000)
        {
            this.timeout = timeout;
        }

        // IArbitrator implementation -------------------------------------------------------------------
        Task IArbitrator.Request(string name)
        {
            TaskCompletionSource<object> t = new TaskCompletionSource<object>();

            if (name==null || name=="" || name=="Default")
            {
                System.Diagnostics.Debug.WriteLine($"Arbitrator {this.InstanceName} request without name");
                throw new Exception($"Arbitrator {this.InstanceName}: Client without name is attempting to request the resource");
            }
            // if it is the first request, immediately return back
            if (waitingQueue.Count == 0)
            {
                occupierRequestorName = name;
                waitingQueue.Enqueue(new Tuple<string, TaskCompletionSource<object>>(name, t));
                System.Diagnostics.Debug.WriteLine($"\nArbitrator {this.InstanceName} requested for {occupierRequestorName}");
                
                t.TrySetResult(null);
                StartTimer(t);
            }
            else
            {
                waitingQueue.Enqueue(new Tuple<string, TaskCompletionSource<object>>(name, t));
            }

           
            // System.Diagnostics.Debug.WriteLine($"Arbitrator {this.InstanceName} Waiting Queue Number: ({waitingQueue.Count}) and being served: ({waitingQueue.Peek().Item1})");

            return t.Task;
        }

        void IArbitrator.Release(string requestorName)
        {
            ReleaseSource(requestorName);
        }

        // private methods ------------------------------------------------------------------------------
        private void ReleaseSource(string name)
        {
            if (occupierRequestorName != name && occupierRequestorName != null)
            {
                throw new Exception($"Arbitrator {this.InstanceName}: Client {name} is attempting to release the resource, that is currently locked by client {occupierRequestorName}");
            }

            // if the queue is not empty after releasing resource, start the next task
            if (waitingQueue.Count > 0)
            {
                cancellationTokenSource?.Dispose();
                System.Diagnostics.Debug.WriteLine($"Arbitrator {this.InstanceName} released for {occupierRequestorName}\n");
                waitingQueue.Dequeue(); //take off task that just finished
                occupierRequestorName = null;

                if (waitingQueue.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Arbitrator {this.InstanceName} Waiting Queue Number: ({waitingQueue.Count}) and next served: ({waitingQueue.Peek().Item1})");

                    Tuple<string, TaskCompletionSource<object>> nextTask = waitingQueue.Peek();
                    nextTask.Item2.TrySetResult(null);
                    StartTimer(nextTask.Item2);
                    occupierRequestorName = nextTask.Item1;

                    System.Diagnostics.Debug.WriteLine($"\nArbitrator {this.InstanceName} requested for {occupierRequestorName}");
                }
                else
                {
                    // System.Diagnostics.Debug.WriteLine($"Arbitrator {this.InstanceName} Waiting Queue: (EMPTY) and next served: (NONE)");
                }

            }
        }

        // private methods ------------------------------------------------------------------------------
        private void StartTimer(TaskCompletionSource<object> tcs)
        {
            // timeout less than zero => no timeout
            if (timeout < 0) return;

            cancellationTokenSource = new CancellationTokenSource(timeout);
            var cancellationToken = cancellationTokenSource.Token;

            //after the 4 seconds when token is cancelled, this method is called
            cancellationToken.Register(() =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    System.Diagnostics.Debug.WriteLine($"Arbitrator {InstanceName} 5 second timeout");
                    tcs.TrySetCanceled();
                    ReleaseSource(occupierRequestorName);
                }

                //cancellationTokenSource.Dispose(); //dispose of the cancellation token to free up memory used by the CancellationTokenSource KL:please check if works and does not create deadlock
            });
        }
    }
}
