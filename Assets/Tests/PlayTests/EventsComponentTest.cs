using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests {

    // NEED TO TEST COMPONENT CALLBACKS AS WELL
    public class EventsComponentTest {

        private CallbackContainer callback;

        public int testValue;

        public static int a, b, c;

        [SetUp]
        public void SetUp() {
            a = 0;
            b = 0;
            c = 0;
            callback = new CallbackContainer();
            testValue = 0;
            Events.Reset();
        }

        [UnityTest]
        public IEnumerator Comp_ResetWorks() {
            GameObject g = new GameObject();

            yield return null;

            g.AddComponent<Rigidbody>();

            yield return null;

            Events.SubscribeToEvent(callback.A, g.GetComponent<Rigidbody>(), EventType.A);

            Events.Reset();

            Events.FireEvent(EventType.A);

            Assert.AreEqual(0, callback.a, "Event callback was called");
        }

        [UnityTest]
        public IEnumerator Comp_CallbackIsCalled() {

            GameObject g = new GameObject();

            yield return null;

            g.AddComponent<Rigidbody>();

            yield return null;

            Events.SubscribeToEvent(callback.A, g.GetComponent<Rigidbody>(), EventType.A);
            Events.SubscribeToEvent(callback.B, g.transform, EventType.B);

            Events.FireEvent(EventType.A);
            Events.FireEvent(EventType.B);

            Assert.AreEqual(1, callback.a, "Event callback was called");
            Assert.AreEqual(1, callback.b, "Event callback was called");
        }

        [UnityTest]
        public IEnumerator Comp_CallbackNotCalledAfterDestroy() {

            GameObject g = new GameObject();

            yield return null;

            g.AddComponent<Rigidbody>();

            yield return null;

            Events.SubscribeToEvent(callback.A, g.GetComponent<Rigidbody>(), EventType.A);
            Events.SubscribeToEvent(callback.B, g.transform, EventType.B);

            Events.FireEvent(EventType.A);
            Events.FireEvent(EventType.B);

            Assert.AreEqual(1, callback.a, "Event callback not called");
            Assert.AreEqual(1, callback.b, "Event callback not called");

            MonoBehaviour.Destroy(g);

            yield return null;

            Events.FireEvent(EventType.A);
            Events.FireEvent(EventType.B);

            Assert.AreEqual(1, callback.a, "Event callback was called after destroy");
            Assert.AreEqual(1, callback.b, "Event callback was called after destroy");
        }
    }

    public class CallbackContainer {

        public int a, b, c;

        public CallbackContainer() {
            a = 0;
            b = 0;
            c = 0;
        }

        public Action TestFunction;

        public void A(CallbackData data) {
            EventsComponentTest.a++;
            a++;
        }
        public void B(CallbackData data) {
            EventsComponentTest.b++;
            b++;
        }
        public void C(CallbackData data) {
            EventsComponentTest.c++;
            c++;
        }
    }
}