using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
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

        [Test]
        public void ResetWorks() {

            Events.SubscribeToEvent(callbacks.A, Events.Event.PlayerDied);
            Events.Reset();

            Events.FireEvent(Events.Event.PlayerDied);

            Assert.AreEqual(0, callbacks.a, "Event callback was called");
        }

        [Test]
        public void CallbacksAreCalled() {

            Events.SubscribeToEvent(callbacks.A, Events.Event.PlayerDied);
            Events.SubscribeToEvent(callbacks.B, Events.Event.EnterGoal);
            Events.SubscribeToEvent(callbacks.C, Events.Event.ExitGoal);

            Events.FireEvent(Events.Event.PlayerDied);
            Events.FireEvent(Events.Event.EnterGoal);
            Events.FireEvent(Events.Event.ExitGoal);

            Assert.AreEqual(1, callbacks.a, "Event callback was not called");
            Assert.AreEqual(1, callbacks.b, "Event callback was not called");
            Assert.AreEqual(1, callbacks.c, "Event callback was not called");

            Assert.AreEqual(1, a, "Event callback was not called");
            Assert.AreEqual(1, b, "Event callback was not called");
            Assert.AreEqual(1, c, "Event callback was not called");
        }

        [Test]
        public void CallbackCanBeRemoved() {

            Events.SubscribeToEvent(callbacks.A, Events.Event.PlayerDied);
            Events.SubscribeToEvent(callbacks.B, Events.Event.EnterGoal);
            Events.SubscribeToEvent(callbacks.C, Events.Event.ExitGoal);

            Events.UnsubscribeFromEvent(callbacks.A, Events.Event.PlayerDied);

            Events.FireEvent(Events.Event.PlayerDied);
            Events.FireEvent(Events.Event.EnterGoal);
            Events.FireEvent(Events.Event.ExitGoal);

            Assert.AreEqual(0, callbacks.a, "Event callback was called");
            Assert.AreEqual(1, callbacks.b, "Event callback was not called");
            Assert.AreEqual(1, callbacks.c, "Event callback was not called");
        }
        [Test]
        public void OnlyMatchingCallbacksAreRemoved() {

            Events.SubscribeToEvent(callbacks.A, Events.Event.PlayerDied);
            Events.SubscribeToEvent(callbacks.B, Events.Event.PlayerDied);

            Events.UnsubscribeFromEvent(callbacks.A, Events.Event.PlayerDied);

            Events.FireEvent(Events.Event.PlayerDied);

            Assert.AreEqual(0, callbacks.a, "Event callback was called");
            Assert.AreEqual(1, callbacks.b, "Event callback was not called");
        }
        [Test]
        public void AllMatchingCallbacksAreRemoved() {

            Events.SubscribeToEvent(callbacks.A, Events.Event.PlayerDied);
            Events.SubscribeToEvent(callbacks.A, Events.Event.PlayerDied);
            Events.SubscribeToEvent(callbacks.B, Events.Event.PlayerDied);

            Events.UnsubscribeFromEvent(callbacks.A, Events.Event.PlayerDied);

            Events.FireEvent(Events.Event.PlayerDied);

            Assert.AreEqual(1, callbacks.a, "Event callback was called");
        }

        [Test]
        public void SameCallbackCanBeAddedTwice() {

            Events.SubscribeToEvent(callbacks.A, Events.Event.PlayerDied);
            Events.SubscribeToEvent(callbacks.A, Events.Event.PlayerDied);

            Events.FireEvent(Events.Event.PlayerDied);

            Assert.AreEqual(2, callbacks.a, "Event callback was not called 2 times");
        }

        [Test]
        public void SubscriptionsCanBeOverwritten() {

            Events.SubscribeToEvent(callbacks.A, Events.Event.PlayerDied);
            Events.SubscribeToEvent(callbacks.A, Events.Event.PlayerDied);

            Events.OverrideSubscription(callbacks.B, Events.Event.PlayerDied);

            Events.FireEvent(Events.Event.PlayerDied);

            Assert.AreEqual(0, callbacks.a, "Event callback was not overridden");
            Assert.AreEqual(1, callbacks.b, "Overriding callback was not called");
        }

        [Test]
        public void CallbackSupportMultipleCallbacks() {

            Events.SubscribeToEvent(callbacks.A, Events.Event.PlayerDied);
            Events.SubscribeToEvent(callbacks.B, Events.Event.PlayerDied);

            Events.FireEvent(Events.Event.PlayerDied);

            Assert.AreEqual(1, callbacks.a, "Event callback A was not called");
            Assert.AreEqual(1, callbacks.b, "Event callback B was not called");
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