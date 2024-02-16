using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static PlayerManager Player { get; private set; }
    public static LockpickingManager Lockpicking { get; private set; }

    private List<IGameManager> startSequence;


    private void Awake()
    {
        Player = GetComponent<PlayerManager>();
        Lockpicking = GetComponent<LockpickingManager>();

        startSequence = new List<IGameManager>();
        startSequence.Add(Player);
        startSequence.Add(Lockpicking);

        StartCoroutine(StartupManagers());
    }


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to the center of the screen
        Cursor.visible = false; // Hides the cursor
        // Note: In Unity, press "Esc" to unlock and unhide the cursor
    }


    private IEnumerator StartupManagers()
    {
        foreach (IGameManager manager in startSequence)
        {
            manager.Startup();
        }

        yield return null;

        int numModules = startSequence.Count;
        int numReady = 0;

        while (numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;

            foreach (IGameManager manager in startSequence)
            {
                if (manager.status == ManagerStatus.Started)
                {
                    numReady++;
                }
            }

            if (numReady > lastReady)
            {
                Debug.Log($"Progress: {numReady}/{numModules}");
            }

            yield return null;
        }

        Debug.Log("All managers started");
    }
}
