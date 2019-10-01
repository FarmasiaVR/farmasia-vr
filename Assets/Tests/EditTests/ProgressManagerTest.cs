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
            int count = manager.activeTasks.Count;
            manager.AddTask(new TestTask());
            Assert.IsTrue(manager.activeTasks.Count == count + 1, "Manager did not add task");
        }

        [Test]
        public void ManagerCanRemoveTasks() {
            int count = manager.activeTasks.Count;
            List<ITask> tasks = manager.activeTasks;
            manager.RemoveTask(tasks.Last());
            Assert.IsTrue(manager.activeTasks.Count == (count - 1), "Manager did not remove task");
        }

        [Test]
        public void ManagerPreservesNonRemovableTask() {
            manager.ResetTasks(false);
            TestTask task = new TestTask();
            TestTask3 task3 = new TestTask3();
            manager.AddTask(task3);
            int count = manager.activeTasks.Count;
            task3.FinishTask();
            Assert.IsTrue(manager.activeTasks.Count == count+1, "Did not preserve Task3 when finishing it.");

        }

        [Test]
        public void TasksCanGenerateNewTasks() {
            manager.ResetTasks(false);
            TestTask2 task2 = new TestTask2();
            manager.AddTask(task2);
            int count = manager.activeTasks.Count;
            task2.FinishTask();
            Assert.IsTrue(manager.activeTasks.Count == count, "Didn't add new task when finishing TestTask2");
            manager.ListActiveTasks();
        }

        [Test]
        public void ManagerCreatesFinishTask() {
            manager.ResetTasks(false);
            TestTask task = new TestTask();
            manager.AddTask(task);
            int count = manager.activeTasks.Count;
            manager.activeTasks.First().FinishTask();
            Assert.IsTrue(manager.activeTasks.First().GetType() == typeof(Finish), "Finish task doesnt exist, although it should!");
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


public class TestTask3 : TaskBase {

    public TestTask3() : base(TaskType.SelectMedicine, false, true) {

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
