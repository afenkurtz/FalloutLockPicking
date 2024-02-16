/*
 * code by Natty GameDev on YouTube: https://www.youtube.com/watch?v=gPPGnpV1Y1c
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// abstract because this will serve as a template for other subclasses
public abstract class Interactable : MonoBehaviour
{
    // message displayed to player when looking at interactable
    [SerializeField] public string namePrompt;
    [SerializeField] public string instructionPrompt;

    // function called from our player
    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        // template function to be overridden by our subclasses
    }
}
