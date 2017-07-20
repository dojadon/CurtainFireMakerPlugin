using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Tasks
{
    public class Task
    {
        private Action<Task> state;
        private Action<Task> task;

        public int Interval { get; set; }
        public int UpdateCount { get; set; }

        public int ExecutionTimes { get; set; }
        public int RunCount { get; set; }

        private int WaitTime { get; }
        private int WaitCount { get; set; }

        public Task(Action<Task> task, int interval, int executionTimes, int waitTime)
        {
            this.task = task;
            this.Interval = this.UpdateCount = interval;
            this.ExecutionTimes = executionTimes;
            this.WaitTime = waitTime;

            this.state = waitTime > 0 ? WAITING : ACTIVE;
        }

        public void Update()
        {
            this.state(this);
        }

        private void Run()
        {
            this.task(this);
        }

        public Boolean IsFinished()
        {
            return this.state == FINISHED;
        }

        private static Action<Task> WAITING = (task) =>
        {
            if (++task.WaitCount > task.WaitTime)
            {
                task.state = ACTIVE;
            }
        };
        private static Action<Task> ACTIVE = (task) =>
        {
            if (++task.UpdateCount >= task.Interval)
            {
                task.UpdateCount = 0;

                if (++task.RunCount > task.ExecutionTimes && task.ExecutionTimes != 0)
                {
                    task.state = FINISHED;
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
