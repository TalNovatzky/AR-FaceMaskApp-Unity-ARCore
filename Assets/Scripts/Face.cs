using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    public string FaceName = "Face";
    public Sprite FaceIcon;
    public List<IInteraction> Interactions = new List<IInteraction>();

    void Start()
    {
        foreach (IInteraction interaction in transform.GetComponentsInChildren<IInteraction>(true))
        {
            Interactions.Add(interaction);
        }
    }

    public void StartInteraction()
    {
        foreach (IInteraction interaction in Interactions)
        {
            interaction.Interact();
        }
    }

    public void ResetInteraction()
    {
        foreach(IInteraction interaction in Interactions)
        {
            interaction.ResetInteraction();
        }
    }
}
