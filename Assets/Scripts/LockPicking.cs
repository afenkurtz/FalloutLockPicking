/*
 * This script is highly derivitive of Zeppelin Games' lockpicking script from the following tutorial: https://www.youtube.com/watch?v=68iYL-rktQ4&list=PLEj1kOxzPTLWX_q_XvjFF9h_3cS4C1jyu&index=12
 * My adaptation is a little bloated, but operates on much the same logic.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPicking : MonoBehaviour
{
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
    [Tooltip("Range in degrees from which the lock can be picked.")]
    [Range(1, 25)] [SerializeField] private float lockRange = 10.0f;
    [Tooltip("The max amount of time a pick can be held down for until it breaks (in seconds).")]
    [Range(1.0f, 5.0f)] [SerializeField] private float maxPickStrength = 2.0f;

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
        NewLock();
        NewPick();
        audioSourceLoop.clip = audioClipLoop;
        attempts = 0;
    }


    private void Update()
    {
        transform.localPosition = pickPosition.position;

        if (attempts == 0)
        {
            anim.Play("Entry");
            attempts++;
        }

        RotatePick();

        UnlockAttemptInput();

        if (keyPressTime == 1)
        {
            attemptDuration += Time.deltaTime;

            if (attemptDuration > pickStrength)
            {
                StartCoroutine(BreakPick());
            }
        }

        RotateLock();

        CheckUnlock();
    }


    private void RotatePick()
    {
        if (movePick)
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
        if (Input.GetKeyDown(KeyCode.W))
        {
            // cannot move bobbypin when trying to pick lock
            movePick = false;
            keyPressTime = 1;

            audioSourceMain.clip = audioClips[0];
            audioSourceMain.Play();

            
        }
        if (Input.GetKeyUp(KeyCode.W))
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
                NewLock();

                movePick = true;
                keyPressTime = 0;
                attemptDuration = 0.0f;

                audioSourceMain.clip = audioClipUnlock;
                audioSourceMain.Play();
            }
            else if (keyPressTime == 1)
            {
                // make lockpick shake if not unlocked
                float randomRotation = Random.insideUnitCircle.x;
                transform.eulerAngles += new Vector3(0, 0, Random.Range(-randomRotation, randomRotation));

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


    private void NewPick()
    {
        pickStrength = Random.Range(0.25f, maxPickStrength);
    }


    [ContextMenu("Generate new lock")]
    private void NewLock()
    {
        // unlocking angle isnt larger than our lock range
        unlockAngle = Random.Range(-maxAngle + lockRange, maxAngle - lockRange);
        // gives a bit of space for moving the pick in to unlock
        // wider you make the unlock range, the easier the lock becomes 
        unlockRange = new Vector2(unlockAngle - lockRange, unlockAngle + lockRange);
        attempts++;
    }
}
