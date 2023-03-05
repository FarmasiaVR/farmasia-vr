using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSimpleInteractable))]
public class XRMenuButton : MonoBehaviour
{

    private SceneLoader levelChanger;

    public MenuInterface menu;
    public SceneTypes scene;
    public bool isCloseButton = false;
    private XRBaseInteractable interactable;

    protected void Start()
    {
        levelChanger = GameObject.FindGameObjectWithTag("LevelChanger").GetComponent<SceneLoader>();
        interactable = GetComponent<XRBaseInteractable>();

        interactable.selectEntered.AddListener(eventArgs => Interact());

    }

    public void Interact()
    {
        if (gameObject.activeInHierarchy)
        {
            if (isCloseButton)
            {
                menu.Close();
            }
            else
            {
                levelChanger.SwapScene(scene);
                levelChanger.FadeOutScene();
            }
        }
    }
}
