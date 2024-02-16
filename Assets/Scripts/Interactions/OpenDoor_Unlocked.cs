using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpenDoor_Unlocked : Interactable
{
    private bool _DoorIsOpen = false;
    private bool _DoorIsActionable = true;
    private float angle = 90.0f;

    // this function is where we will design our interaction using code
    protected override void Interact()
    {
        if (_DoorIsOpen && _DoorIsActionable)
        {
            StartCoroutine(UseDoor(angle));
        }
        else if (!_DoorIsOpen && _DoorIsActionable)
        {
            StartCoroutine(UseDoor(-angle));
        }
    }


    IEnumerator UseDoor(float angle)
    {
        Tween myTween = transform.DORotate(new Vector3(0, angle, 0), 1, RotateMode.Fast).SetRelative();
        _DoorIsActionable = false;
        yield return myTween.WaitForCompletion();
        _DoorIsOpen = !_DoorIsOpen;
        _DoorIsActionable = true;
    }
}
