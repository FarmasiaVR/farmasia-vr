using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace FarmasiaVR.New
{
    [Serializable]
    public struct Task
    {
        /// <summary>
        /// This is a struct that represents a task that the player can complete.
        /// </summary>
        [NonSerialized]
        public string name;

        [Tooltip("The key used to refer to this task from other scripts")]
        public string key;

        [Tooltip("What the task is. This can be shown to the player when this task is active.")]
        public string taskText;

        [Tooltip("The hint related to the current task. Can be used in hintboxes to aid the player.")]
        public string hint;

        [Tooltip("How many points should be given to the player when the task is completed.")]
        public int points;

        [NonSerialized]
        [Tooltip("Whether this task is completed or not")]
        public bool completed;

        [Tooltip("Whether the task is time sensitive and the points awarded should be based on how long it takes for the player to complete the task")]
        [SerializeField]
        public bool timed;

        [Header("Timed parameters")]

        [Tooltip("How long the task is")]
        public float timeToCompleteTask;
        [Tooltip("Whether the player fails and the OnTaskFailed should be invoked when the time runs out")]
        public bool failWhenOutOfTime;

        [NonSerialized]
        public float timeTaskStarted;

        [NonSerialized]
        public int awardedPoints;

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
                float timeTakenToCompleteTask = Time.time - timeTaskStarted;
                float pointsMultiplier = 1 - (timeTakenToCompleteTask / timeTakenToCompleteTask);
                // If the player took longer to complete the task than the time limit allows, then set the multiplier to zero
                if (pointsMultiplier< 0) pointsMultiplier = 0;
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
    }
}
