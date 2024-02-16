/*
 * Majority of this code from Zeppelin Games on YouTube: https://www.youtube.com/watch?v=68iYL-rktQ4&list=PLEj1kOxzPTLWX_q_XvjFF9h_3cS4C1jyu&index=11
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugging : MonoBehaviour
{
    public Camera cam;
    public Transform innerLock;
    public Transform pickPosition;
    public bool drawDebugLines;
    
    // how far we can turn our lock from the middle (half of total)
    public float maxAngle = 90;
    // how fast the lock turns
    public float lockSpeed = 10;

    // difficulty of lock
    // range in degrees that the lock will unlock at
    [Min(1)]
    [Range(1, 25)]
    public float lockRange = 10;
    // code for following attribute from u/zaikman on r/Unity3D: https://www.reddit.com/r/Unity3D/comments/1s6czv/inspectorbutton_add_a_custom_button_to_your/
    [InspectorButton("OnButtonClicked", ButtonWidth = 100)]
    public bool updateRange;


    // keeps track of current angle the lock is at
    public float eulerAngle;
    // keeps track of current angle the lock will unlock at
    public float unlockAngle;
    // keeps track of current unlock range
    public Vector2 unlockRange;

    public float keyPressTime = 0;

    // to turn on/off allowing user to use the pick
    private bool movePick = true;


    public float percentage;
    public float lockRotation;
    public float maxRotation;
    public float lockLerp;
    public bool unlocked;


    void Start()
    {
        newLock();
        drawDebugLines = true;
        unlocked = false;
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


        if (drawDebugLines)
        {
            /*
             * Following 4 lines from "duck" on the unity forum: 
             * https://discussions.unity.com/t/draw-line-to-a-specific-angle/13132
             */
            // vector to base the lines off of, originating from the center and going up 2 units
            var line = pickPosition.transform.position + (Vector3.up * 2);
            Debug.DrawLine(pickPosition.transform.position, line, Color.white);
            // rotate vectors by multiplying the unlockRange angles on the lock's z/forward axis
            var rotatedLineX = Quaternion.AngleAxis(unlockRange.x, transform.forward) * line;
            var rotatedLineY = Quaternion.AngleAxis(unlockRange.y, transform.forward) * line;
            // draw lines from the center to the rotated vectors
            Debug.DrawLine(pickPosition.transform.position, rotatedLineX, Color.green);
            Debug.DrawLine(pickPosition.transform.position, rotatedLineY, Color.green);
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

        percentage = Mathf.Round(100 - Mathf.Abs((eulerAngle - unlockAngle) / 100 /* he says 180 in the video but types 100 */) * 100);
        // for rotating inner lock
        lockRotation = ((percentage / 100) * maxAngle) * keyPressTime;
        maxRotation = (percentage / 100) * maxAngle;

        lockLerp = Mathf.LerpAngle(innerLock.eulerAngles.z, lockRotation, Time.deltaTime * lockSpeed);
        innerLock.eulerAngles = new Vector3(0, 0, lockLerp);

        if (lockLerp >= maxRotation - 1)
        {
            if (eulerAngle < unlockRange.y && eulerAngle > unlockRange.x)
            {
                Debug.Log("Unlocked!");
                unlocked = true;
                //newLock();

                //movePick = true;
                //keyPressTime = 0;
            }
            else if (keyPressTime == 1)
            {
                unlocked = false;
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
        unlocked = false;
    }


    /*
    private void OnDrawGizmos()
    {
        var p = pickPosition.transform.position;
        Gizmos.color = Color.white;
        var 
        Gizmos.DrawRay(p, _sourceVector * LengthMultiplier);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(p, _rotatedVector * LengthMultiplier);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(p, _limitA * LengthMultiplier);
        Gizmos.DrawRay(p, _limitB * LengthMultiplier);
    } */
}
