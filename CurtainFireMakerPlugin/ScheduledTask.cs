using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace CurtainFireMakerPlugin
{
    public class ScheduledTask
    {
        private Func<ScheduledTask, dynamic> Run { get; }
        private Func<int, int> GetExecutingInterval { get; }

        public int LatencyTime { get; set; }

        public int ExecutingInterval { get; set; }
        public int UpdatedCount { get; set; }

        public int ExecuteToTime { get; set; }
        public int ExecutedCount { get; set; }

        public ScheduledTask(Func<ScheduledTask, dynamic> runedFunc, Func<int, int> getInterval, int executeToTime, int latencyTime)
        {
            Run = runedFunc;
            GetExecutingInterval = getInterval;
            ExecuteToTime = executeToTime;
            LatencyTime = latencyTime;

            ExecutingInterval = UpdatedCount = GetExecutingInterval(0);
        }

        public void Update()
        {
            if (--LatencyTime > 0)
            {
                return;
            }

            if (++UpdatedCount >= ExecutingInterval)
            {
                UpdatedCount = 0;

                if ((++ExecutedCount < ExecuteToTime || ExecuteToTime == 0) & !(Run(this) is bool flag && flag))
                {
                    ExecutingInterval = GetExecutingInterval(ExecutedCount);
                }
                else
                {
                    ExecutedCount = ExecuteToTime;
                }
            }
        }

        public bool IsCompleted => ExecutedCount >= ExecuteToTime;
    }

    public class TaskScheduler
    {
        private List<ScheduledTask> TaskList { get; } = new List<ScheduledTask>();
        private List<ScheduledTask> AddTaskList { get; } = new List<ScheduledTask>();

        public void AddTask(ScheduledTask task)
        {
            AddTaskList.Add(task);
        }

        public void Frame()
        {
            TaskList.AddRange(AddTaskList);
            AddTaskList.Clear();

            TaskList.ForEach(task => task.Update());
            TaskList.RemoveAll(task => task.IsCompleted);
        }
    }
}
