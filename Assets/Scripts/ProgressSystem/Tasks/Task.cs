using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace FarmasiaVR.New
{
    [Serializable]
    public class TimedTaskParameters
    {
        [Tooltip("How long the task is")]
        public float timeToCompleteTask;
        [Tooltip("Whether the player fails and the OnTaskFailed should be invoked when the time runs out")]
        public bool failWhenOutOfTime;
    }

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
        public float points;

        [NonSerialized]
        [Tooltip("Whether this task is completed or not")]
        public bool completed;

        [Tooltip("Whether the task is time sensitive and the points awarded should be based on how long it takes for the player to complete the task")]
        [SerializeField]
        public bool timed;

        public TimedTaskParameters timedParameters;

        private float timeTaskStarted;
        private int awardedPoints;

        public void Reset()
        {
            completed = false;
            awardedPoints = 0;
        }
    }
}
