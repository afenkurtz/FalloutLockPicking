/*
 * code adapted from Natty GameDev on YouTube: https://www.youtube.com/watch?v=gPPGnpV1Y1c
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// logic to detect interactables and handle player input
public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    // Serialize so its visible in the inspector
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        playerUI = GetComponent<PlayerUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(string.Empty, string.Empty, string.Empty);
        // create a ray at the center of the camera, shooting outwards
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        // variable to store our collision information
        RaycastHit hitInfo;
        // raycast to center of screen
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {   
            // checking if the gameobject has an interactable component
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                // storing that interactable
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();

                var difficultyPrompt = "";

                if (hitInfo.collider.GetComponent<OpenDoor_Unlocked>() != null)
                {
                    // if the door is unlocked, it wont have a difficulty associated to it
                    difficultyPrompt = " ";
                }
                else if (hitInfo.collider.GetComponent<OpenDoor_Locked>() != null)
                {
                    // finding the difficulty of the door being looked at
                    LockpickingDifficulties difficulty = hitInfo.collider.GetComponentInParent<OpenDoor_Locked>().GetDifficulty();
                    difficultyPrompt = $"[ {difficulty} ]";
                }
                
                // updating onscreen text
                playerUI.UpdateText(interactable.namePrompt, interactable.instructionPrompt, difficultyPrompt);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // will run Interact() on the interactable obj
                    interactable.BaseInteract();
                }
            }
        }

    }
}
