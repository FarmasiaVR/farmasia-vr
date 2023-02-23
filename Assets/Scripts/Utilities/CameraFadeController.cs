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
    public UnityEvent onFadeOutComplete;
    public UnityEvent onFadeInComplete;

    private void Start() {
        fadeAnimator = GetComponent<Animator>();
    }

    public void BeginFadeOut() {
        ///<summary>
        /// This starts the fade out animation which turns the screen black
        /// </summary>
        /// 

        fadeAnimator.SetTrigger("FadeOut");
    }

    public void BeginFadeIn() {
        ///<summary>
        ///This begins the fade in animation, which will transition the screen from black to being able to see around you.
        /// </summary>
        /// 

        fadeAnimator.SetTrigger("FadeIn");
    }

    public void FadeOutComplete() {
        ///<summary>
        ///This is called by the fade out animation when it has been completed.
        ///All of the listeners attached are also removed
        /// </summary>
        /// 
        onFadeOutComplete.Invoke();
        onFadeOutComplete.RemoveAllListeners();
    }

    public void FadeInComplete() {
        ///<summary>
        ///This is called by the fade in animation when it has been completed.
        /// </summary>
        /// 
        onFadeInComplete.Invoke();
        onFadeOutComplete.RemoveAllListeners();
    }
}
