using System.Linq;
using NUnit.Framework;

namespace Tests {

    /// <summary>
    /// Tests ProgressManager and how it should handle tasks.
    /// </summary>
    public class TestProgressManager {

        private ProgressManager manager;

        [SetUp]
        public void SetUp() {
            manager = new ProgressManager(true);
        }

        /*[Test]
        public void ManagerAddsNewTasks() {
            manager.AddTask(new TestTask());
            Assert.AreEqual(1, manager.activeTasks.Count, "Manager did not add task");
        }

        [Test]
        public void ManagerCanRemoveTasks() {
            manager.AddTask(new TestTask());
            manager.AddTask(new TestTask());
            manager.RemoveTask(manager.activeTasks.Last());
            Assert.AreEqual(1, manager.activeTasks.Count, "Manager did not remove task");
        }

        [Test]
        public void ManagerPreservesNonRemovableTask() {
            manager.AddTask(new TestTask());
            manager.AddTask(new TestTask2());
            manager.activeTasks.Last().FinishTask();
            Assert.AreEqual(2, manager.activeTasks.Count, "Did not preserve Task3 when finishing it.");
        }

        [Test]
        public void ManagerCanAddTaskBefore() {
            TaskBase task = new TestTask();
            manager.AddTask(task);
            manager.AddNewTaskBeforeTask(new TestTask(), task);
            Assert.AreEqual(2, manager.activeTasks.Count, "Didn't add new task");
            Assert.AreEqual(task, manager.activeTasks.Last(), "Didn't put new task in correct order");
        }

        [Test]
        public void ManagerCreatesFinishTask() {
            manager.AddTask(new TestTask());
            manager.activeTasks.Last().FinishTask();
            Assert.AreEqual(1, manager.activeTasks.Count, "Finish task is not added when the last task is finished");
            Assert.IsTrue(manager.activeTasks.Last().GetType() == typeof(Finish), "The added task is not a Finish task");
        }*/
    }

    public class TestTask : Task {

        public TestTask() : base(TaskType.SelectTools, true) { }

        protected override void OnTaskComplete() {
        }
    }

    public class TestTask2 : Task {

        public TestTask2() : base(TaskType.SelectMedicine, true) { }

        protected override void OnTaskComplete() {
        }
    }
}