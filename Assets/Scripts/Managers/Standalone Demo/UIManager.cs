using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    public bool logValues { get; private set; }
    public bool creditsOpened { get; private set; }

    [SerializeField] private DebugConsole logScript;
    [SerializeField] private GameObject popup;
    [SerializeField] private TextMeshProUGUI difficulty;
    [SerializeField] private Toggle logValuesButton;
    [SerializeField] private Color activeColor;

    private Color inactiveColor;


    public void Startup()
    {
        Debug.Log("UIManager starting...");

        difficulty.text = "Novice";

        creditsOpened = false;
        logValues = false;
        inactiveColor = logValuesButton.colors.normalColor;

        status = ManagerStatus.Started;
    }


    public void GenerateNoviceLockButton()
    {
        Managers_Standalone.Lockpicking.GenerateNoviceLock();
        difficulty.text = "Novice";
    }

    public void GenerateAdvancedLockButton()
    {
        Managers_Standalone.Lockpicking.GenerateAdvancedLock();
        difficulty.text = "Advanced";
    }

    public void GenerateExpertLockButton()
    {
        Managers_Standalone.Lockpicking.GenerateExpertLock();
        difficulty.text = "Expert";
    }

    public void GenerateEliteLockButton()
    {
        Managers_Standalone.Lockpicking.GenerateEliteLock();
        difficulty.text = "Elite";
    }


    public void LogValues()
    {
        ColorBlock cb = logValuesButton.colors;

        if (logValuesButton.isOn)
        {
            cb.normalColor = activeColor;
            cb.highlightedColor = inactiveColor;
        }
        else
        {
            cb.normalColor = inactiveColor;
            cb.highlightedColor = activeColor;
        }

        logValuesButton.colors = cb;

        logValues = !logValues;
    }


    public void ZeppelinButton()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=68iYL-rktQ4&list=PLEj1kOxzPTLWX_q_XvjFF9h_3cS4C1jyu&index=12");
        if (Managers_Standalone.UI.logValues) { Managers_Standalone.UI.HandleLog("URL sent to default browser."); }
    }

    public void derHugoButton()
    {
        Application.OpenURL("https://stackoverflow.com/questions/60228993/putting-debug-log-as-a-gui-element-in-unity");
        if (Managers_Standalone.UI.logValues) { Managers_Standalone.UI.HandleLog("URL sent to default browser."); }
    }

    public void BrevicepsButton()
    {
        Application.OpenURL("https://freesound.org/people/Breviceps/sounds/458405/");
        if (Managers_Standalone.UI.logValues) { Managers_Standalone.UI.HandleLog("URL sent to default browser."); }
    }

    public void duncanlewismackinnonButton()
    {
        Application.OpenURL("https://freesound.org/people/duncanlewismackinnon/sounds/159331/");
        if (Managers_Standalone.UI.logValues) { Managers_Standalone.UI.HandleLog("URL sent to default browser."); }
    }

    public void MyButton()
    {
        Application.OpenURL("https://github.com/afenkurtz");
        if (Managers_Standalone.UI.logValues) { Managers_Standalone.UI.HandleLog("URL sent to default browser."); }
    }

    public void OpenCreditsPopup()
    {
        popup.SetActive(true);
        creditsOpened = true;
    }

    public void CloseCreditsPopup()
    {
        popup.SetActive(false);
        creditsOpened = false;
    }

    public void HandleLog(string log)
    {
        logScript.HandleLog(log);
    }
}
