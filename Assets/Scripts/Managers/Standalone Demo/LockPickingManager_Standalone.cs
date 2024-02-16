using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPickingManager_Standalone : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    [SerializeField] private LockPicking_Standalone_Demo lockScript;

    public void Startup()
    {
        Debug.Log("LockpickingManager starting...");

        status = ManagerStatus.Started;
    }

    public void GenerateNoviceLock()
    {
        lockScript.SetDifficulty(LockpickingDifficulties.novice);
    }

    public void GenerateAdvancedLock()
    {
        lockScript.SetDifficulty(LockpickingDifficulties.advanced);
    }

    public void GenerateExpertLock()
    {
        lockScript.SetDifficulty(LockpickingDifficulties.expert);
    }

    public void GenerateEliteLock()
    {
        lockScript.SetDifficulty(LockpickingDifficulties.elite);
    }
}
