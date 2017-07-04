using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Tasks
{
    public class TaskManager
    {
        private List<Task> taskList = new List<Task>();
        private List<Task> addTaskList = new List<Task>();

        public void AddTask(Task task)
        {
            this.addTaskList.Add(task);
        }

        public void Frame()
        {
            this.taskList.AddRange(this.addTaskList);
            this.addTaskList.Clear();

            this.taskList.ForEach(task => task.Update());
            this.taskList.RemoveAll(task => task.IsFinished());
        }
    }
}
