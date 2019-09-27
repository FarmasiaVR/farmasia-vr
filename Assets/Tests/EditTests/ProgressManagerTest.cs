using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Tests {

    /// <summary>
    /// Tests ProgressManager and how it should handle tasks.
    /// </summary>
    public class TestProgressManager {
        ProgressManager manager;
        [SetUp]
        public void SetUp() {
            manager = new ProgressManager(true);
        }

        [Test]
        public void ManagerAddsNewTasks() {
            int count = manager.ActiveTasks.Count;
            manager.AddTask(new TestTask());
            Assert.IsTrue(manager.ActiveTasks.Count == count + 1, "Manager did not add task");
        }

        [Test]
        public void ManagerCanRemoveTasks() {
            int count = manager.ActiveTasks.Count;
            List<ITask> tasks = manager.ActiveTasks;
            manager.RemoveTask(tasks.Last());
            Assert.IsTrue(manager.ActiveTasks.Count == (count - 1), "Manager did not remove task");
        }

        [Test]
        public void TasksCanGenerateNewTasks() {
            manager.ResetTasks(false);
            TestTask2 task2 = new TestTask2();
            manager.AddTask(task2);
            int count = manager.ActiveTasks.Count;
            task2.FinishTask();
            Assert.IsTrue(manager.ActiveTasks.Count == count, "Didn't add new task when finishing TestTask2");
            manager.ListActiveTasks();
        }

        [Test]
        public void ManagerCreatesFinishTask() {
            manager.ResetTasks(false);
            TestTask task = new TestTask();
            manager.AddTask(task);
            int count = manager.ActiveTasks.Count;
            manager.ActiveTasks.First().FinishTask();
            Assert.IsTrue(manager.ActiveTasks.First().GetType() == typeof(Finish), "Finish task doesnt exist, although it should!");
            manager.ListActiveTasks();
        }
    }
}

public class TestTask : TaskBase {

    public TestTask() : base(TaskType.SelectTools, true, true) {

    }
    public override void FinishTask() {
        base.FinishTask();
    }

    public override string GetDescription() {
        return base.GetDescription();
    }

    public override string GetHint() {
        return base.GetHint();
    }

    public override void Subscribe() {
        base.Subscribe();
    }
}

public class TestTask2 : TaskBase {

    public TestTask2() : base(TaskType.SelectMedicine, true, true) {

    }
    public override void FinishTask() {
        manager.AddNewTaskBeforeTask(new TestTask(), this);
        manager.ListActiveTasks();
        base.FinishTask();
    }

    public override string GetDescription() {
        return base.GetDescription();
    }

    public override string GetHint() {
        return base.GetHint();
    }

    public override void Subscribe() {
        base.Subscribe();
    }
}
