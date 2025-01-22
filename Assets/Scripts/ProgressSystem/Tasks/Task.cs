using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace FarmasiaVR.New
{
    [Serializable]
    public class Task
    {
        /// <summary>
        /// This is a struct that represents a task that the player can complete.
        /// </summary>
        [NonSerialized]
        public string name;

        [Tooltip("The key used to refer to this task from other scripts")]
        [field: SerializeField]
        public string key { get; private set; }


        [Tooltip("What the task is. This can be shown to the player when this task is active.")]
        [field: SerializeField]
        public string taskText { get; private set; }


        [Tooltip("The hint related to the current task. Can be used in hintboxes to aid the player.")]
        [field: SerializeField]
        public string hint { get; private set; }

        [Tooltip("How many points should be given to the player when the task is completed.")]
        [field: SerializeField]
        public int points { get; private set; }

        [Tooltip("Whether this task is completed or not")]
        public bool completed { get; private set; }

        [Tooltip("Whether the task is time sensitive and the points awarded should be based on how long it takes for the player to complete the task")]
        [field: SerializeField]
        public bool timed { get; private set; }

        [field: Header("Timed parameters")]
        [Tooltip("How long the task is")]
        [field:SerializeField]
        public float timeToCompleteTask { get; private set; }

        [Tooltip("Whether the player should be penalised for not completing the task on time. OnTaskFailed is invoked when the time runs out")]
        [field: SerializeField]
        public bool failWhenOutOfTime { get; private set; }

        [NonSerialized]
        private float timeTaskStarted;
        [field: NonSerialized]
        public float timeTakenToCompleteTask { get; private set; }

        [field: NonSerialized]
        public int awardedPoints { get; private set; }

        private List<Mistake> mistakeList;

        public void Reset()
        {
            completed = false;
            awardedPoints = 0;
            mistakeList = new List<Mistake>();
        }

        public void MarkAsDone()
        {
            completed= true;

            if ( timed )
            {
                timeTakenToCompleteTask = Time.time - timeTaskStarted;
                float pointsMultiplier = 1 - (timeTakenToCompleteTask / timeToCompleteTask);
                // If the player took longer to complete the task than the time limit allows, then set the multiplier to zero
                if (pointsMultiplier < 0) pointsMultiplier = 0;
                if (pointsMultiplier > 1) pointsMultiplier = 1;
                awardedPoints = (int) (pointsMultiplier * points);
            }
            else
            {
                awardedPoints = points;
            }
        }
        /// <summary>
        /// Adds a mistake to the mistake list of the task
        /// </summary>
        /// <param name="mistake">A mistake object to save to the mistake list</param>
        public void AddMistake(Mistake mistake)
        {
            mistakeList.Add(mistake);
        }
        /// <summary>
        /// Return the mistake list of the task
        /// This method was created for testing
        /// </summary>
        public List<Mistake> ReturnMistakes()
        {
            return mistakeList;
        }

        public void StartTaskTimer()
        {
            timeTaskStarted= Time.time;
        }
    }
}
