/*
 * Code from Zeppelin Games on YouTube: https://www.youtube.com/watch?v=68iYL-rktQ4&list=PLEj1kOxzPTLWX_q_XvjFF9h_3cS4C1jyu&index=11
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPick : MonoBehaviour
{
    public Camera cam;
    public Transform innerLock;
    public Transform pickPosition;

    // how far we can turn our lock from the middle (half of total)
    public float maxAngle = 90;
    // how fast the lock turns
    public float lockSpeed = 10;

    // difficulty of lock
    // range in degrees that the lock will unlock at
    [Min(1)] [Range(1, 25)]
    public float lockRange = 10;

    // keeps track of current angle the lock is at
    private float eulerAngle;
    // keeps track of current angle the lock will unlock at
    private float unlockAngle;
    // keeps track of current unlock range
    private Vector2 unlockRange;

    private float keyPressTime = 0;

    // to turn on/off allowing user to use the pick
    private bool movePick = true;
    



    void Start()
    {
        newLock();
    }




    void Update()
    {
        transform.localPosition = pickPosition.position;

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

            // rotate around x axis
            Quaternion rotateTo = Quaternion.AngleAxis(eulerAngle, Vector3.forward);
            transform.rotation = rotateTo;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            // cannot move bobbypin when trying to pick lock
            movePick = false;
            keyPressTime = 1;
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            movePick = true;
            keyPressTime = 0;
        }

        // no explanation given for this in video...
        keyPressTime = Mathf.Clamp(keyPressTime, 0, 1);

        float percentage = Mathf.Round(100 - Mathf.Abs((eulerAngle - unlockAngle) / 100 /* he says 180 in the video but types 100 */) * 100);
        // for rotating inner lock
        float lockRotation = ((percentage / 100) * maxAngle) * keyPressTime;
        float maxRotation = (percentage / 100) * maxAngle;

        float lockLerp = Mathf.LerpAngle(innerLock.eulerAngles.z, lockRotation, Time.deltaTime * lockSpeed);
        innerLock.eulerAngles = new Vector3(0, 0, lockLerp);

        if(lockLerp >= maxRotation - 1)
        {
            if (eulerAngle < unlockRange.y && eulerAngle > unlockRange.x)
            {
                Debug.Log("Unlocked!");
                newLock();

                movePick = true;
                keyPressTime = 0;
            }
            else if (keyPressTime == 1)
            {
                // make lockpick shake if not unlocked
                float randomRotation = Random.insideUnitCircle.x;
                transform.eulerAngles += new Vector3(0, 0, Random.Range(-randomRotation, randomRotation));
            }
        }
    }




    void newLock()
    {
        // unlocking angle isnt larger than our lock range
        unlockAngle = Random.Range(-maxAngle + lockRange, maxAngle - lockRange);
        // gives a bit of space for moving the pick in to unlock
        // wider you make the unlock range, the easier the lock becomes 
        unlockRange = new Vector2(unlockAngle - lockRange, unlockAngle + lockRange);
    }
}
