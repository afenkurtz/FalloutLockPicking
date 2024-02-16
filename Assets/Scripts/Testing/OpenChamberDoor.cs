using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpenChamberDoor : Interactable
{
    [SerializeField] float angle = -43.0f;

    private bool _DoorIsOpen;

    // this function is where we will design our interaction using code
    protected override void Interact()
    {
        if (_DoorIsOpen)
        {
            transform.DORotate(new Vector3(-90, 0, 0), 1, RotateMode.Fast);
        }
        else
        {
            transform.DORotate(new Vector3(-90, 0, angle), 1, RotateMode.Fast);
        }
        _DoorIsOpen = !_DoorIsOpen;
    }

}
