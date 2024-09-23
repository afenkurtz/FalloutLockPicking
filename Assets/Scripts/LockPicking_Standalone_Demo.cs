/*
 * This script is highly derivitive of Zeppelin Games' lockpicking script from the following tutorial: https://www.youtube.com/watch?v=68iYL-rktQ4&list=PLEj1kOxzPTLWX_q_XvjFF9h_3cS4C1jyu&index=12
 * My adaptation is a little bloated, but operates on much the same logic.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPicking_Standalone_Demo : MonoBehaviour
{
    public LockpickingDifficulties LocalDifficulty { get; private set; }

    [Header("General")]
    [Tooltip("The lockpick's own camera.")]
    [SerializeField] private Camera cam;
    [Tooltip("The inner-lock object.")]
    [SerializeField] private Transform innerLock;
    [Tooltip("The empty that the pick will follow and rotate around")]
    [SerializeField] private Transform pickPosition;
    [Tooltip("How far its possible to rotate the lock and pick from the middle (half of total rotation range).")]
    [SerializeField] private float maxAngle = 90.0f;
    [Tooltip("How fast the lock and pick turn.")]
    [SerializeField] private float lockSpeed = 10.0f;

    [Header("Difficulty")]
    [Tooltip("You have to generate a new lock after changing difficulty.")]
    [SerializeField] private LockpickingDifficulties localDifficulty;
    [Tooltip("The minimum range in degrees from which the lock can be picked (before multipliers).")]
    [SerializeField] private float maxLockRange = 15.0f;
    [Tooltip("The max amount of time a pick can be held down for until it breaks.")]
    [SerializeField] private float maxPickStrength = 3.0f;

    [Header("Animation")]
    [Tooltip("Animator controller.")]
    [SerializeField] private Animator anim;

    [Header("Audio")]
    [Tooltip("The main audio source.")]
    [SerializeField] private AudioSource audioSourceMain;
    [Tooltip("The looping audio source.")]
    [SerializeField] private AudioSource audioSourceLoop;
    [Tooltip("The unlock audio clip.")]
    [SerializeField] private AudioClip audioClipUnlock;
    [Tooltip("The audio clip for picking a lock in the wrong position.")]
    [SerializeField] private AudioClip audioClipLoop;
    [Tooltip("The audio clip for breaking a pick.")]
    [SerializeField] private AudioClip audioClipBreak;
    [Tooltip("The audio clips to be used. The first one is the initial movement sound.")]
    [SerializeField] private AudioClip[] audioClips;

    // The angle of the pick
    private float eulerAngle;
    // The angle of the pick last frame
    private float eulerAngleLastFrame;
    // The angle to base the unlock range from
    private float unlockAngle;
    // Keeps track of current unlock range
    private Vector2 unlockRange;
    // The range in degrees from which the lock can be picked from 
    private float lockRange;
    // Difficulty multiplier for novice locks
    private float noviceMultiplier = 1.0f;
    // Difficulty multiplier for advanced locks
    private float advancedMultiplier = 0.5f;
    // Difficulty multiplier for expert locks
    private float expertMultiplier = 0.25f;
    // Difficulty multiplier for master locks
    private float eliteMultiplier = 0.125f;
    // Keeps track of if a button is held
    private float keyPressTime = 0;
    // To turn on/off allowing user to use the pick
    private bool movePick = true;
    // To rotate the inner lock
    private float lockLerp;
    // To keep track of how far the inner lock can rotate
    private float maxRotation;
    // Used to play intro animation only on first attempt
    private int attempts;
    // To keep track of the current pick's strength (time in seconds it can be held before breaking)
    private float pickStrength;
    // To keep track of how long an pick is held down for
    private float attemptDuration;



    void Start()
    {
        localDifficulty = GetDifficulty();
        NewLock();
        NewPick();
        audioSourceLoop.clip = audioClipLoop;
        attempts = 0;
    }


    private void Update()
    {
        transform.position = pickPosition.position;

        if (attempts == 0)
        {
            anim.Play("Entry");
            attempts++;
        }

        RotatePick();

        UnlockAttemptInput();

        RotateLock();

        CheckUnlock();
    }
    
    
    private void RotatePick()
    {
        if (movePick && !Managers_Standalone.UI.creditsOpened)
        {
            // creates direction from mouse to current position
            Vector3 dir = Input.mousePosition - cam.WorldToScreenPoint(transform.position);

            // axis to rotate around
            eulerAngle = Vector3.Angle(dir, Vector3.up);

            Vector3 cross = Vector3.Cross(Vector3.up, dir);
            // to get both neg and pos range
            if (cross.z < 0)
                eulerAngle = -eulerAngle;

            eulerAngle = Mathf.Clamp(eulerAngle, -maxAngle, maxAngle);

            // if the pick is moving,
            if (!Mathf.Approximately(eulerAngle, eulerAngleLastFrame))
            {
                if (audioSourceMain.isPlaying)
                {
                    // do nothing (do not play clip)
                }
                else
                {
                    audioSourceMain.clip = audioClips[Random.Range(1, audioClips.Length)];
                    audioSourceMain.Play();
                }
            }

            // rotate around x axis
            Quaternion rotateTo = Quaternion.AngleAxis(eulerAngle, Vector3.forward);
            transform.rotation = rotateTo;

            eulerAngleLastFrame = eulerAngle;
        }
    }

    private void UnlockAttemptInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && !Managers_Standalone.UI.creditsOpened)
        {
            // cannot move bobbypin when trying to pick lock
            movePick = false;
            keyPressTime = 1;

            audioSourceMain.clip = audioClips[0];
            audioSourceMain.Play();

            
        }
        if (Input.GetKeyUp(KeyCode.W) && !Managers_Standalone.UI.creditsOpened)
        {
            movePick = true;
            keyPressTime = 0;
            attemptDuration = 0.0f;

            audioSourceLoop.Stop();
        }
    }

    private void RotateLock()
    {
        keyPressTime = Mathf.Clamp(keyPressTime, 0, 1);

        float percentage = Mathf.Round(100 - Mathf.Abs((eulerAngle - unlockAngle) / 180 /* he says 180 in the video but types 100 */) * 100);   // 100 there instead of 180 would allow you to turn the inner lock clockwise in some instances
        // for rotating inner lock
        float lockRotation = ((percentage / 100) * maxAngle) * keyPressTime;
        maxRotation = (percentage / 100) * maxAngle;

        lockLerp = Mathf.LerpAngle(innerLock.eulerAngles.z, lockRotation, Time.deltaTime * lockSpeed);
        innerLock.eulerAngles = new Vector3(0, 0, lockLerp);
    }


    private void CheckUnlock()
    {
        if (lockLerp >= maxRotation - 1)
        {
            if (eulerAngle < unlockRange.y && eulerAngle > unlockRange.x)
            {
                StartCoroutine(UnlockDoor());
                NewLock();
                NewPick();

                movePick = true;
                keyPressTime = 0;
                attemptDuration = 0.0f;
            }
            else if (keyPressTime == 1)
            {
                // make lockpick shake if not unlocked
                float randomRotation = Random.insideUnitCircle.x;
                transform.eulerAngles += new Vector3(0, 0, Random.Range(-randomRotation, randomRotation));

                // keep track of how long a pick has been held for
                attemptDuration += Time.deltaTime / 60;
                // pick gets weaker the longer it is held regardless of attempt duration
                pickStrength -= attemptDuration;

                if (pickStrength < 0.0f)
                {
                    StartCoroutine(BreakPick());
                }

                if (audioSourceLoop.isPlaying)
                {
                    // do nothing (do not play clip)
                }
                else
                {
                    audioSourceLoop.Play();
                }
            }
        }
    }


    private IEnumerator BreakPick()
    {
        audioSourceLoop.Stop();
        anim.Play("BreakPick");
        movePick = false;
        keyPressTime = 0;
        attemptDuration = 0.0f;

        audioSourceMain.clip = audioClipBreak;
        audioSourceMain.Play();

        NewPick();
        attempts++;

        yield return new WaitForSeconds(3.0f);
    }


    private IEnumerator UnlockDoor()
    {
        Managers_Standalone.UI.HandleLog("Lock successfully picked!");
        audioSourceMain.clip = audioClipUnlock;
        audioSourceMain.Play();

        yield return new WaitForSeconds(3.0f);
    }


    [ContextMenu("Generate new pick")]
    private void NewPick()
    {
        switch (localDifficulty)
        {
            case LockpickingDifficulties.novice:
                pickStrength = maxPickStrength * noviceMultiplier;
                break;
            case LockpickingDifficulties.advanced:
                pickStrength = maxPickStrength * advancedMultiplier;
                break;
            case LockpickingDifficulties.expert:
                pickStrength = maxPickStrength * expertMultiplier;
                break;
            case LockpickingDifficulties.elite:
                pickStrength = maxPickStrength * eliteMultiplier;
                break;
        }
    }

    

    [ContextMenu("Generate new lock")]
    private void NewLock()
    {
        switch (localDifficulty)
        {
            case LockpickingDifficulties.novice:
                lockRange = maxLockRange * noviceMultiplier;
                break;
            case LockpickingDifficulties.advanced:
                lockRange = maxLockRange * advancedMultiplier;
                break;
            case LockpickingDifficulties.expert:
                lockRange = maxLockRange * expertMultiplier;
                break;
            case LockpickingDifficulties.elite:
                lockRange = maxLockRange * eliteMultiplier;
                break;
        }
        
        // unlocking angle isnt larger than our lock range
        unlockAngle = Random.Range(-maxAngle + lockRange, maxAngle - lockRange);
        // gives a bit of space for moving the pick in to unlock
        // wider you make the unlock range, the easier the lock becomes 
        unlockRange = new Vector2(unlockAngle - lockRange, unlockAngle + lockRange);
        attempts++;

        Managers_Standalone.UI.HandleLog($"-new {localDifficulty} lock generated");

        if (Managers_Standalone.UI.logValues)
        {
            Managers_Standalone.UI.HandleLog($"--unlock angle = {-unlockAngle}° from center");
        }
    }


    public LockpickingDifficulties GetDifficulty()
    {
        return localDifficulty;
    }


    public void SetDifficulty(LockpickingDifficulties difficulty)
    {
        localDifficulty = difficulty;

        Managers_Standalone.UI.HandleLog($"Difficulty set to {localDifficulty}.");

        // need to generate a new lock and pick to update any difficulty changes
        NewLock();
        NewPick();

        if (Managers_Standalone.UI.logValues)
        {
            Managers_Standalone.UI.HandleLog($"--unlock range = {lockRange}°");
            Managers_Standalone.UI.HandleLog($"--pickStrength = {pickStrength} seconds");
        }
        else
        {
            Managers_Standalone.UI.HandleLog("-pick strength adjusted");
        }
    }
}
