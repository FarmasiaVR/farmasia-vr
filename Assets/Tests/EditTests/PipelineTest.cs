using System;
using NUnit.Framework;

namespace Tests {

    public class TestPipeline {

        [Test]
        public void TestEmpty() {
            Pipeline subject = new Pipeline();
            Assert.IsTrue(subject.IsDone, "Empty pipeline is not done");
        }

        [Test]
        public void TestNullVoidFunctor() {
            try {
                Pipeline subject = new Pipeline().Func(null);
                subject.Update(0);
            } catch (NullReferenceException) {
                Assert.Fail("VoidFunctorAction unable to handle null functor");
            }
        }

        [Test]
        public void TestOneVoidFunctor() {
            bool hasExecuted = false;
            Pipeline subject = new Pipeline().Func(() => hasExecuted = true);
            subject.Update(0);
            Assert.IsTrue(hasExecuted, "Pipeline functor not executed");
        }

        [Test]
        public void TestTwoVoidFunctors() {
            bool hasExecuted = false;
            Pipeline subject = new Pipeline().Func(() => {}).Func(() => hasExecuted = true);
            subject.Update(0);
            Assert.IsFalse(hasExecuted, "Second functor executed with one Update call");
            subject.Update(0);
            Assert.IsTrue(hasExecuted, "Second functor not executed with two Update calls");
        }

        [Test]
        public void TestVoidFunctorIsDone() {
            Pipeline subject = new Pipeline().Func(() => {});
            Assert.IsFalse(subject.IsDone, "Non-empty Pipeline is done before any Update calls");
            subject.Update(0);
            Assert.IsTrue(subject.IsDone, "One-action Pipeline is not done after one Update call");
        }

        [Test]
        public void TestNullFunctorIntFunctor() {
            try {
                Pipeline subject = new Pipeline().TFunc<int>(null, () => 0);
                subject.Update(0);
            } catch (NullReferenceException) {
                Assert.Fail("TFunctorAction unable to handle null getter functor");
            }
        }

        [Test]
        public void TestNullGetterIntFunctor() {
            try {
                Pipeline subject = new Pipeline().TFunc<int>(o => {}, null);
                subject.Update(0);
            } catch (NullReferenceException) {
                Assert.Fail("TFunctorAction unable to handle null getter functor");
            }
        }

        [Test]
        public void TestOneIntFunctor() {
            int value = 0;
            int expected = 3;
            Pipeline subject = new Pipeline().TFunc<int>(i => value = i, () => expected);
            subject.Update(0);
            Assert.AreEqual(expected, value, "Pipeline functor not executed");
        }

        [Test]
        public void TestTwoIntFunctors() {
            int value = 0;
            int arg1 = 1;
            int arg2 = 2;
            Pipeline subject = new Pipeline()
                                .TFunc<int>(i => value += i, () => arg1)
                                .TFunc<int>(i => value += i, () => arg2);
            subject.Update(0);
            Assert.AreEqual(arg1, value, "First Pipeline functor not executed");
            subject.Update(0);
            Assert.AreEqual(arg1 + arg2, value, "Second Pipeline functor not executed");
        }

        [Test]
        public void TestObjectFunctor() {
            PipelineObjectMockup mockup = new PipelineObjectMockup();
            Pipeline subject = new Pipeline().TFunc<PipelineObjectMockup>(o => o.Inc(), () => mockup);
            subject.Update(0);
            Assert.AreEqual(1, mockup.Value, "Object functor was not called");
        }

        [Test]
        public void TestOneDelay() {
            Pipeline subject = new Pipeline().Delay(1);
            subject.Update(0.9f);
            Assert.IsFalse(subject.IsDone, "Pipeline delay action completed too early");
            subject.Update(0.1f);
            Assert.IsTrue(subject.IsDone, "Pipeline delay action not completed on time");
        }

        [Test]
        public void TestTwoDelays() {
            Pipeline subject = new Pipeline().Delay(1).Delay(0.5f);
            subject.Update(1.1f);
            Assert.AreEqual(1, subject.Count, "Completed delay action not removed");
            subject.Update(1.1f);
            Assert.AreEqual(0, subject.Count, "Completed delay action not removed");
        }

        [Test]
        public void TestDelayVoidFunctor() {
            bool hasExecuted = false;
            Pipeline subject = new Pipeline().Delay(1).Func(() => hasExecuted = true);
            subject.Update(0.5f);
            Assert.IsFalse(hasExecuted, "Delay not applied before functor");
            subject.Update(1);
            Assert.IsFalse(hasExecuted, "Functor called immediately when delay completes");
            subject.Update(0);
            Assert.IsTrue(hasExecuted, "Functor not executed after delay");
        }

        [Test]
        public void TestFuncPreFunc() {
            bool hasExecuted = false;
            Pipeline subject = new Pipeline().Func(() => hasExecuted = true).PreFunc(() => {});
            subject.Update(0);
            Assert.IsFalse(hasExecuted, "PreFunc does not prepend functor in Pipeline");
            subject.Update(0);
            Assert.IsTrue(hasExecuted, "Reordered functor was not called");
        }

        [Test]
        public void TestFuncPreIntFunc() {
            int value = 0;
            Pipeline subject = new Pipeline()
                                .Func(() => value = 2)
                                .PreTFunc<int>(i => value -= i, () => 3);
            subject.Update(0);
            Assert.AreEqual(-3, value, "PreTFunc does not prepend functor in Pipeline");
            subject.Update(0);
            Assert.AreEqual(2, value, "Reordered functor was not called");
        }

        [Test]
        public void TestFuncPreDelay() {
            bool hasExecuted = false;
            Pipeline subject = new Pipeline().Func(() => { hasExecuted = true; }).PreDelay(1);
            subject.Update(1);
            Assert.IsFalse(hasExecuted, "PreDelay does not prepend delay in Pipeline");
            subject.Update(0);
            Assert.IsTrue(hasExecuted, "Reordered functor was not called");
        }
    }

    public class PipelineObjectMockup {
        public int Value { get; private set; }

        public void Inc() {
            Value++;
        }
    }
}