using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers_Standalone : MonoBehaviour
{
    public static UIManager UI { get; private set; }
    public static LockPickingManager_Standalone Lockpicking { get; private set; }

    private List<IGameManager> startSequence;


    private void Awake()
    {
        UI = GetComponent<UIManager>();
        Lockpicking = GetComponent<LockPickingManager_Standalone>();

        startSequence = new List<IGameManager>();
        startSequence.Add(UI);
        startSequence.Add(Lockpicking);

        StartCoroutine(StartupManagers());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
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

        Debug.Log("All managers started.");
    }
}
