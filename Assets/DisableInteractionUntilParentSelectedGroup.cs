using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableInteractionUntilParentSelectedGroup : MonoBehaviour
{
    public List<disableInteractionUntilParentSelected> group;
    public void RestoreInteractions()
    {
        group.ForEach(script =>
        {
            if (script)
            {
                script.RestoreInteraction();
            }
        });
    }

    public void DisableInteractions()
    {
        group.ForEach(script =>
        {
            if (script)
            {
                script.DisableInteraction();
            }
        });
    }
    public void FirstDisableInteractions(SelectEnterEventArgs args)
    {
        group.ForEach(script =>
        {
            if (script)
            {
                script.FirstDisableInteraction(args);
            }
        });
    }
}
