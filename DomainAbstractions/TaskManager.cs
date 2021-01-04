using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    public class TaskManager : IEvent, IDataFlow<JObject>, IDataFlow<bool>
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IEvent taskFinished;
        private IDataFlow<string> taskInfo;

        // fields
        private string taskStartTime;
        private string taskEndTime;
        private bool taskExecuting;
        private JObject task;
        private Queue<JObject> msgQueue = new Queue<JObject>();
        // ctor
        public TaskManager() { }


        void IEvent.Execute()
        {
            if (!taskExecuting)
            {
                BeginNewTask();
                ParseMessageQueue();
            }
            else
            {
                StopExistingTask();
            }
        }

        bool IDataFlow<bool>.Data
        {
            set
            {
                // true: request to begin a new task, false: stop the currently executing task
                if (value)
                {
                    BeginNewTask();
                    ParseMessageQueue();
                }
                else
                {
                    StopExistingTask();
                }
            }
        }

        JObject IDataFlow<JObject>.Data
        {
            // Monitor and process information or messages created during the task executing
            // The message JObject format is supposed to be like: {"ERROR", new JValue("There is duplicate column in the session")}, {"Name", new JValue("Upload session: Test Session")}
            set
            {
                if (!taskExecuting)
                {
                    Debug.WriteLine($"There is no task currently running in {InstanceName}, can not process this message.");
                    msgQueue.Enqueue(value);
                }
                else
                {
                    msgQueue.Enqueue(value);

                    ParseMessageQueue();
                }
            }
        }

        private JObject InitialiseTask()
        {
            return new JObject(
                new JProperty("START", taskStartTime),
                new JProperty("END", taskEndTime),
                new JProperty("NAME", string.Empty),
                new JProperty("DESCRIPTION", string.Empty),
                new JProperty("INFORMATION", new JArray()),
                new JProperty("ERROR", new JArray())
                );
        }

        private void BeginNewTask()
        {
            if (taskExecuting)
            {
                Debug.WriteLine($"A task is currently executing in {InstanceName}, can not create a new task.");
            }
            else
            {
                taskStartTime = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss-ffff}";
                task = InitialiseTask();
                taskExecuting = true;
            }
        }

        private void StopExistingTask()
        {
            if (!taskExecuting)
            {
                Debug.WriteLine($"There is no task currently running in {InstanceName}.");
            }
            else
            {
                taskEndTime = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss-ffff}";
                ((JValue)task["END"]).Value = taskEndTime;
                taskExecuting = false;

                if (taskInfo != null) taskInfo.Data = JsonConvert.SerializeObject(task);
                taskFinished?.Execute();
            }
        }

        private void ParseMessageQueue()
        {
            while (msgQueue.Any())
            {
                var msgObj = msgQueue.Dequeue();

                foreach (var msgPair in msgObj)
                {
                    var msgName = msgPair.Key.ToUpper();
                    var msgContent = (JValue)msgPair.Value;

                    if (task.ContainsKey(msgName))
                    {
                        var taskToken = task[msgName];
                        if (taskToken is JValue)
                        {
                            ((JValue)task[msgName]).Value = msgContent;
                        }

                        if (taskToken is JArray)
                        {
                            ((JArray)task[msgName]).Add(msgContent);
                        }
                    }
                    else
                    {
                        task.Add(new JProperty(msgName, msgContent));
                    }
                }
            }
        }
    }
}