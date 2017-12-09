using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin
{
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
