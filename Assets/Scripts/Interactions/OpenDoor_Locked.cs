using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpenDoor_Locked : Interactable
{
    public LockpickingDifficulties localDifficulty;
    
    private bool _DoorIsOpen = false;
    private bool _DoorIsActionable = true;
    private bool _DoorIsLocked = true;
    private float angle = 90.0f;

    // function for the abstract Interactable.cs template class
    protected override void Interact()
    {
        if (_DoorIsActionable)
        {
            if (!_DoorIsOpen && _DoorIsLocked)
            {
                if (Managers.Player.lockpicks > 0)
                {
                    Managers.Lockpicking.StartLockpicking(localDifficulty, this.gameObject);
                }
            }
            else if (!_DoorIsOpen)
            {
                StartCoroutine(UseDoor(-angle));
            }
            else
            {
                StartCoroutine(UseDoor(angle));
            }
        }
    }

    public IEnumerator UseDoor(float angle)
    {
        Tween myTween = transform.DORotate(new Vector3(0, angle, 0), 1, RotateMode.Fast).SetRelative();
        _DoorIsActionable = false;
        yield return myTween.WaitForCompletion();
        _DoorIsOpen = !_DoorIsOpen;
        _DoorIsActionable = true;
    }

    public LockpickingDifficulties GetDifficulty()
    {
        return localDifficulty;
    }


    public void UnlockDoor()
    {
        _DoorIsLocked = false;
        StartCoroutine(UseDoor(-angle));
    }
}
