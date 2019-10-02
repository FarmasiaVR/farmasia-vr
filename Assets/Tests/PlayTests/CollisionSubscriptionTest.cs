using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {

    public class TestCollisionSubscription {

        private GameObject CreateObject() {
            return CreateObject(Vector3.zero);
        }

        private GameObject CreateObject(Vector3 position) {
            GameObject g = new GameObject();
            g.name = "GameObject";
            g.AddComponent<Rigidbody>();
            g.GetComponent<Rigidbody>().useGravity = false;
            g.AddComponent<BoxCollider>(); // default radius: 0.5
            g.transform.position = position;
            return g;
        }

        private GameObject CreateTrigger() {
            return CreateTrigger(Vector3.zero);
        }

        private GameObject CreateTrigger(Vector3 position) {
            GameObject g = CreateObject(position);
            g.name = "Trigger";
            g.GetComponent<Collider>().isTrigger = true;
            return g;
        }

        [UnityTest]
        public IEnumerator TestSubscription() {
            /* All tests are being done in one function, because these tests
             * cannot be done with co-routines. This is still not the most
             * deterministic solution, since sometimes the collision events
             * do not fire.
             * 
             * KNOWN BUG: Sometimes, collision events are not fired leading
             * to tests failing.
             */

            /*** Collision tests ***/
            bool hasEnteredCollision1 = false;
            bool hasStayedCollision1 = false;
            bool hasExitedCollision1 = false;
            bool hasEnteredCollision2 = false;
            bool hasStayedCollision2 = false;
            bool hasExitedCollision2 = false;
            GameObject g = CreateObject();
            GameObject o = CreateObject(new Vector3(0, 2, 0));
            GameObject t = CreateTrigger(new Vector3(2, 0, 0));

            CollisionSubscription.SubscribeToCollision(g,
                new CollisionListener()
                    .OnEnter(c => hasEnteredCollision1 = true)
                    .OnStay(c => hasStayedCollision1 = true)
                    .OnExit(c => hasExitedCollision1 = true)
            );
            CollisionSubscription.SubscribeToCollision(g,
                new CollisionListener()
                    .OnEnter(c => hasEnteredCollision2 = true)
                    .OnStay(c => hasStayedCollision2 = true)
                    .OnExit(c => hasExitedCollision2 = true)
            );
            yield return null;

            o.transform.position = g.transform.position;
            yield return null;

            Assert.IsTrue(hasEnteredCollision1, "OnCollisionEnter() not fired!");
            Assert.IsTrue(hasEnteredCollision2, "OnCollisionEnter() not fired!");
            yield return null;

            Assert.IsTrue(hasStayedCollision1, "OnCollisionStay() not fired!");
            Assert.IsTrue(hasStayedCollision2, "OnCollisionStay() not fired!");
            o.transform.position = new Vector3(0, 2, 0);
            yield return null;

            Assert.IsTrue(hasExitedCollision1, "OnCollisionExit() not fired!");
            Assert.IsTrue(hasExitedCollision2, "OnCollisionExit() not fired!");
            yield return null;

            CollisionSubscription.Clear(g);
            yield return new WaitForSeconds(1);

            /*** Trigger Tests ***/

            bool hasEnteredTrigger1 = false;
            bool hasStayedTrigger1 = false;
            bool hasExitedTrigger1 = false;
            bool hasEnteredTrigger2 = false;
            bool hasStayedTrigger2 = false;
            bool hasExitedTrigger2 = false;

            CollisionSubscription.SubscribeToTrigger(t,
                new TriggerListener()
                    .OnEnter(c => hasEnteredTrigger1 = true)
                    .OnStay(c => hasStayedTrigger1 = true)
                    .OnExit(c => hasExitedTrigger1 = true)
            );
            CollisionSubscription.SubscribeToTrigger(t,
                new TriggerListener()
                    .OnEnter(c => hasEnteredTrigger2 = true)
                    .OnStay(c => hasStayedTrigger2 = true)
                    .OnExit(c => hasExitedTrigger2 = true)
            );
            yield return null;

            t.transform.position = g.transform.position;
            yield return null;

            Assert.IsTrue(hasEnteredTrigger1, "OnTriggerEnter() not fired!");
            Assert.IsTrue(hasEnteredTrigger2, "OnTriggerEnter() not fired!");
            yield return null;

            Assert.IsTrue(hasStayedTrigger1, "OnTriggerStay() not fired!");
            Assert.IsTrue(hasStayedTrigger2, "OnTriggerStay() not fired!");
            t.transform.position = new Vector3(2, 0, 0);
            yield return null;

            Assert.IsTrue(hasExitedTrigger1, "OnTriggerExit() not fired!");
            Assert.IsTrue(hasExitedTrigger2, "OnTriggerExit() not fired!");
        }
    }
}