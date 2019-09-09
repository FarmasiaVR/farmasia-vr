using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests {

    // NEED TO TEST COMPONENT CALLBACKS AS WELL
    public class EventsTest {

        private CallbackContainer callbacks;

        public int testValue;

        public static int a, b, c;

        [SetUp]
        public void SetUp() {
            a = 0;
            b = 0;
            c = 0;
            callbacks = new CallbackContainer();
            testValue = 0;
            Events.Reset();
        }

        #region Regular

        [Test]
        public void ResetWorks() {

            Events.SubscribeToEvent(callbacks.A, EventType.A);
            Events.Reset();

            Events.FireEvent(EventType.A);

            Assert.AreEqual(0, callbacks.a, "Event callback was called");
        }

        [Test]
        public void CallbacksAreCalled() {

            Events.SubscribeToEvent(callbacks.A, EventType.A);
            Events.SubscribeToEvent(callbacks.B, EventType.B);
            Events.SubscribeToEvent(callbacks.C, EventType.C);

            Events.FireEvent(EventType.A);
            Events.FireEvent(EventType.B);
            Events.FireEvent(EventType.C);

            Assert.AreEqual(1, callbacks.a, "Event callback was not called");
            Assert.AreEqual(1, callbacks.b, "Event callback was not called");
            Assert.AreEqual(1, callbacks.c, "Event callback was not called");

            Assert.AreEqual(1, a, "Event callback was not called");
            Assert.AreEqual(1, b, "Event callback was not called");
            Assert.AreEqual(1, c, "Event callback was not called");
        }

        [Test]
        public void CallbackCanBeRemoved() {

            Events.SubscribeToEvent(callbacks.A, EventType.A);
            Events.SubscribeToEvent(callbacks.B, EventType.B);
            Events.SubscribeToEvent(callbacks.C, EventType.C);

            Events.UnsubscribeFromEvent(callbacks.A, EventType.A);

            Events.FireEvent(EventType.A);
            Events.FireEvent(EventType.B);
            Events.FireEvent(EventType.C);

            Assert.AreEqual(0, callbacks.a, "Event callback was called");
            Assert.AreEqual(1, callbacks.b, "Event callback was not called");
            Assert.AreEqual(1, callbacks.c, "Event callback was not called");
        }
        [Test]
        public void OnlyMatchingCallbacksAreRemoved() {

            Events.SubscribeToEvent(callbacks.A, EventType.A);
            Events.SubscribeToEvent(callbacks.B, EventType.A);

            Events.UnsubscribeFromEvent(callbacks.A, EventType.A);

            Events.FireEvent(EventType.A);

            Assert.AreEqual(0, callbacks.a, "Event callback was called");
            Assert.AreEqual(1, callbacks.b, "Event callback was not called");
        }
        [Test]
        public void AllMatchingCallbacksAreRemoved() {

            Events.SubscribeToEvent(callbacks.A, EventType.A);
            Events.SubscribeToEvent(callbacks.A, EventType.A);
            Events.SubscribeToEvent(callbacks.B, EventType.A);

            Events.UnsubscribeFromEvent(callbacks.A, EventType.A);

            Events.FireEvent(EventType.A);

            Assert.AreEqual(1, callbacks.a, "Event callback was called");
        }

        [Test]
        public void SameCallbackCanBeAddedTwice() {

            Events.SubscribeToEvent(callbacks.A, EventType.A);
            Events.SubscribeToEvent(callbacks.A, EventType.A);

            Events.FireEvent(EventType.A);

            Assert.AreEqual(2, callbacks.a, "Event callback was not called 2 times");
        }

        [Test]
        public void SubscriptionsCanBeOverwritten() {

            Events.SubscribeToEvent(callbacks.A, EventType.A);
            Events.SubscribeToEvent(callbacks.A, EventType.A);

            Events.OverrideSubscription(callbacks.B, EventType.A);

            Events.FireEvent(EventType.A);

            Assert.AreEqual(0, callbacks.a, "Event callback was not overridden");
            Assert.AreEqual(1, callbacks.b, "Overriding callback was not called");
        }

        [Test]
        public void CallbackSupportMultipleCallbacks() {

            Events.SubscribeToEvent(callbacks.A, EventType.A);
            Events.SubscribeToEvent(callbacks.B, EventType.A);

            Events.FireEvent(EventType.A);

            Assert.AreEqual(1, callbacks.a, "Event callback A was not called");
            Assert.AreEqual(1, callbacks.b, "Event callback B was not called");
        }
        #endregion Component

        [UnityTest]
        public IEnumerator Comp_ResetWorks() {

            SceneManager.CreateScene("testScene");
            SceneManager.LoadScene("testScene");

            yield return new WaitForSeconds(1);

            GameObject g = new GameObject();

            yield return null;

            g.AddComponent<Rigidbody>();

            yield return null;



            Events.SubscribeToEvent(callbacks.A, g.GetComponent<Rigidbody>(), EventType.A);
            Events.Reset();

            Component c = g.GetComponent<Rigidbody>();

            Events.FireEvent(EventType.A);

            Assert.AreEqual(0, callbacks.a, "Event callback was called");
        }
    }

    public class CallbackContainer {

        public int a, b, c;

        public CallbackContainer() {
            int a = 0;
            int b = 0;
            int c = 0;
        }

        public Action TestFunction;

        public void A() {
            EventsTest.a++;
            a++;
        }
        public void B() {
            EventsTest.b++;
            b++;
        }
        public void C() {
            EventsTest.c++;
            c++;
        }
    }
}