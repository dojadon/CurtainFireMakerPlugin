using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Entities
{
    public class Task
    {
        private Action<Task> State { get; set; }
        private Action<Task> Action { get; set; }

        public int Interval { get; set; }
        public int UpdateCount { get; set; }

        public int ExecutionTimes { get; set; }
        public int RunCount { get; set; }

        public int WaitTime { get; }
        public int WaitCount { get; set; }

        public Task(Action<Task> task, int interval, int executionTimes, int waitTime)
        {
            Action = task;
            Interval = UpdateCount = interval;
            ExecutionTimes = executionTimes;
            WaitTime = waitTime;

            State = waitTime > 0 ? WAITING : ACTIVE;
        }

        public void Update() => State(this);

        private void Run() => Action(this);

        public bool IsFinished() => State == FINISHED;

        private static Action<Task> WAITING = (task) =>
        {
            if (++task.WaitCount >= task.WaitTime - 1)
            {
                task.State = ACTIVE;
            }
        };
        private static Action<Task> ACTIVE = (task) =>
        {
            if (++task.UpdateCount >= task.Interval)
            {
                task.UpdateCount = 0;

                if (task.RunCount + 1 > task.ExecutionTimes && task.ExecutionTimes != 0)
                {
                    task.State = FINISHED;
                }
                else
                {
                    task.Run();
                    task.RunCount++;
                }
            }
        };
        private static Action<Task> FINISHED = (task) => { };
    }

    internal class TaskManager
    {
        private List<Task> TaskList { get; } = new List<Task>();
        private List<Task> AddTaskList { get; } = new List<Task>();

        public void AddTask(Task task)
        {
            AddTaskList.Add(task);
        }

        public void Frame()
        {
            TaskList.AddRange(AddTaskList);
            AddTaskList.Clear();

            TaskList.ForEach(task => task.Update());
            TaskList.RemoveAll(task => task.IsFinished());
        }

        public bool IsEmpty() => TaskList.Count == 0 && AddTaskList.Count == 0;
    }
}
