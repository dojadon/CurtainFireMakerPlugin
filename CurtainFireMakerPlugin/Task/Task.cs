using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Task
{
    class Task
    {
        private Action<Task> state;
        private Action task;

        public int interval;
        public int updateCount;

        public int executionTimes;
        public int runCount;

        private int waitTime;
        private int waitCount;

        public Task(Action task, int interval, int executionTimes, int waitTime)
        {
            this.task = task;
            this.interval = this.updateCount = interval;
            this.executionTimes = executionTimes;
            this.waitTime = waitTime;
        }

        public void Update()
        {
            this.state.Invoke(this);
        }

        private void Run()
        {
            this.task.Invoke();
        }

        public Boolean IsFinished()
        {
            return this.state == FINISHED;
        }

        private static Action<Task> WAITING = (task) =>
        {
            if (++task.waitCount > task.waitTime)
            {
                task.state = ACTIVE;
            }
        };
        private static Action<Task> ACTIVE = (task) =>
        {
            if (++task.updateCount >= task.interval)
            {
                task.updateCount = 0;

                if (++task.runCount > task.executionTimes && task.executionTimes != 0)
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
