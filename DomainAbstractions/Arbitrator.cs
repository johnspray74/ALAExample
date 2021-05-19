using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Arbitrator class is in charge of allowing only one instance to be using the arbitrator resource at one time therefore allowing a order for e.g. SCP commands
    /// Any incoming requests for the same arbitrator will enqueue at the end and once the current request is complete and released
    /// it will be dequeued and therefore allowing the next following Task in the queue to begin.
    /// It contains a timer to timeout after 5 seconds of no use and to release the resource. (By using cancellationToken)
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IArbitrator arbitrator: interface for managing mutiple asynchronized resource requests
    /// </summary>
    public class Arbitrator : IArbitrator // arbitrator
    {
        //properties ---------------------------------------------------------------
        public string InstanceName;

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

            // if it is the first request, immediately return back
            if (waitingQueue.Count == 0)
            {
                occupierRequestorName = name;
                waitingQueue.Enqueue(new Tuple<string, TaskCompletionSource<object>>(name, t));
                System.Diagnostics.Debug.WriteLine($"\n{occupierRequestorName} requested for {this.InstanceName} ====================================");
                
                t.TrySetResult(null);
                StartTimer(t);
            }
            else
            {
                waitingQueue.Enqueue(new Tuple<string, TaskCompletionSource<object>>(name, t));
            }

           
            System.Diagnostics.Debug.WriteLine($"{this.InstanceName} Waiting Queue Number: ({waitingQueue.Count}) and being served: ({waitingQueue.Peek().Item1}) ====================================");

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
                throw new Exception($"Client {name} is attempting to release the resource, {InstanceName}, that is currently locked by client {occupierRequestorName}");
            }

            // if the queue is not empty after releasing resource, start the next task
            if (waitingQueue.Count > 0)
            {
                cancellationTokenSource?.Dispose();
                System.Diagnostics.Debug.WriteLine($"{occupierRequestorName} released {this.InstanceName} ====================================\n");
                waitingQueue.Dequeue(); //take off task that just finished
                occupierRequestorName = null;

                if (waitingQueue.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"{this.InstanceName} Waiting Queue Number: ({waitingQueue.Count}) and next served: ({waitingQueue.Peek().Item1}) ====================================");

                    Tuple<string, TaskCompletionSource<object>> nextTask = waitingQueue.Peek();
                    nextTask.Item2.TrySetResult(null);
                    StartTimer(nextTask.Item2);
                    occupierRequestorName = nextTask.Item1;

                    System.Diagnostics.Debug.WriteLine($"\n{occupierRequestorName} requested for {this.InstanceName} ====================================");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"{this.InstanceName} Waiting Queue: (EMPTY) and next served: (NONE) ====================================");
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
