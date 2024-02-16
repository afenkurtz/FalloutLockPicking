using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockpickingManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    public GameObject lockPickingObj;

    private LockPicking_Demo lockPickingScript;
    private bool currentLockIsLocked;
    private GameObject doorInUse;

    public void Startup()
    {
        Debug.Log("LockpickingManager starting...");

        currentLockIsLocked = true;

        status = ManagerStatus.Started;
    }

    // TODO: Postprocessing on scene/player camera
    public void StartLockpicking(LockpickingDifficulties diff, GameObject door)
    {
        doorInUse = door;
        currentLockIsLocked = true;
        Managers.Player.PauseInput();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        lockPickingObj.SetActive(true);
        lockPickingObj.GetComponent<LockPicking_Demo>().SetDifficulty(diff);
    }

    // TODO: Fade out transition 
    public void EndLockpicking()
    {
        if (!currentLockIsLocked)
        {
            doorInUse.GetComponent<OpenDoor_Locked>().UnlockDoor();
        }

        StartCoroutine(WaitForSeconds());

        lockPickingObj.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Managers.Player.UnpauseInput();
        currentLockIsLocked = true;
    }

    public void CurrentDoorUnlocked()
    {
        currentLockIsLocked = false;
    }

    private IEnumerator WaitForSeconds()
    {
        // this may be frowned upon... 
        yield return new WaitForSeconds(3.0f);
    }
}
