using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Tasks
{
    public class Task
    {
        private Action<Task> State { get; set; }
        private Action<Task> Action { get; set; }

        public int Interval { get; set; }
        public int UpdateCount { get; set; }

        public int ExecutionTimes { get; set; }
        public int RunCount { get; set; }

        private int WaitTime { get; }
        private int WaitCount { get; set; }

        public Task(Action<Task> task, int interval, int executionTimes, int waitTime)
        {
            Action = task;
            Interval = UpdateCount = interval;
            ExecutionTimes = executionTimes;
            WaitTime = waitTime;

            State = waitTime > 0 ? WAITING : ACTIVE;
        }

        public void Update()
        {
            State(this);
        }

        private void Run()
        {
            Action(this);
        }

        public Boolean IsFinished()
        {
            return State == FINISHED;
        }

        private static Action<Task> WAITING = (task) =>
        {
            if (++task.WaitCount >= task.WaitTime)
            {
                task.State = ACTIVE;
            }
        };
        private static Action<Task> ACTIVE = (task) =>
        {
            if (++task.UpdateCount > task.Interval)
            {
                task.UpdateCount = 0;

                if (++task.RunCount > task.ExecutionTimes && task.ExecutionTimes != 0)
                {
                    task.State = FINISHED;
                }
                else
                {
                    task.Run();
                }
            }
        };
        private static Action<Task> FINISHED = (task) => { };
    }
}
