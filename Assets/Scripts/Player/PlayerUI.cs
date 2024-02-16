/*
 * code by Natty GameDev on YouTube: https://www.youtube.com/watch?v=gPPGnpV1Y1c
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactibleName;
    [SerializeField] private TextMeshProUGUI instruction;
    [SerializeField] private TextMeshProUGUI difficulty;

    public void UpdateText(string namePrompt, string instructionPrompt, string difficultyPrompt)
    {
        interactibleName.text = namePrompt;
        instruction.text = instructionPrompt;
        difficulty.text = difficultyPrompt;
    }
}
