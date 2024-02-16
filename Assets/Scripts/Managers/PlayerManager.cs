using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    public int lockpicks { get; private set; }
    public bool inputPaused { get; private set; }

    public GameObject player;

    public FPSInput keyInput;
    public MouseLook mouseInput1;
    public MouseLook mouseInput2;

    public void Startup()
    {
        Debug.Log("PlayerManager starting...");

        lockpicks = 15;
        inputPaused = false;

        status = ManagerStatus.Started;
    }



    public void BreakLockpick()
    {
        lockpicks--;
        Debug.Log($"Lockpick broken. {lockpicks} lockpicks left.");
    }

    public void GiveLockpick()
    {
        lockpicks++;
        Debug.Log($"Lockpick given. {lockpicks} lockpicks left.");
    }

    public void GiveLockPicks(int num)
    {
        lockpicks += num;
        Debug.Log($"{num} lockpicks given. {lockpicks} lockpicks left.");
    }

    public void PauseInput()
    {
        inputPaused = true;
        keyInput.enabled = false;
        mouseInput1.enabled = false;
        mouseInput2.enabled = false;
        Debug.Log("Character controller input paused.");
    }

    public void UnpauseInput()
    {
        inputPaused = false;
        keyInput.enabled = true;
        mouseInput1.enabled = true;
        mouseInput2.enabled = true;
        Debug.Log("Character controller input un-paused.");
    }
}
