using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraFadeController : MonoBehaviour
{
    ///<summary>
    ///This a script that is used with the camera fade animator to control the fade animation and pass through animator events to different objects.
    ///This makes it possible to call the camera fade animations from different scripts and call appropriate events when the fade is complete.
    /// </summary>
    /// 

    private Animator fadeAnimator;
    [Header("Fade Out")]
    public UnityEvent onFadeOutStart;
    public UnityEvent onFadeOutComplete;

    [Header("Fade In")]
    public UnityEvent onFadeInStart;
    public UnityEvent onFadeInComplete;

    // Take the states of the above events and revert to these states when the fades are completed.
    private UnityEvent defaultOnFadeOutStart;
    private UnityEvent defaultOnFadeOutComplete;
    private UnityEvent defaultOnFadeInStart;
    private UnityEvent defaultOnFadeInComplete;

    private void Start() {
        fadeAnimator = GetComponent<Animator>();
        defaultOnFadeInStart = onFadeInStart;
        defaultOnFadeInComplete = onFadeInComplete;
        defaultOnFadeOutStart = onFadeOutStart;
        defaultOnFadeOutComplete = onFadeOutComplete;

    }

    public void BeginFadeOut() {
        ///<summary>
        /// This starts the fade out animation which turns the screen black
        /// </summary>
        /// 

        fadeAnimator.SetTrigger("FadeOut");
        onFadeOutStart.Invoke();
        onFadeOutStart = defaultOnFadeOutStart;
    }

    public void BeginFadeIn() {
        ///<summary>
        ///This begins the fade in animation, which will transition the screen from black to being able to see around you.
        /// </summary>
        /// 

        fadeAnimator.SetTrigger("FadeIn");
        onFadeInStart.Invoke();
        onFadeInStart = defaultOnFadeInStart;
    }

    public void FadeOutComplete() {
        ///<summary>
        ///This is called by the fade out animation when it has been completed.
        ///All of the listeners attached by other scripts are also removed
        /// </summary>
        /// 
        onFadeOutComplete.Invoke();
        onFadeOutComplete = defaultOnFadeOutComplete;
    }

    public void FadeInComplete() {
        ///<summary>
        ///This is called by the fade in animation when it has been completed.
        /// </summary>
        /// 
        onFadeInComplete.Invoke();
        onFadeInComplete = defaultOnFadeInComplete;
    }
}
