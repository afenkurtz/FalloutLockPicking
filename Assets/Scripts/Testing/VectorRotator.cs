using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorRotator : MonoBehaviour
{
    private const float LengthMultiplier = 4f;

    [Tooltip("White line")]
    [SerializeField] Vector3 _sourceVector;
    [Tooltip("Green lines")]
    [SerializeField] float _rotationRange;
    [Tooltip("Red line")]
    [SerializeField] Vector3 _rotatedVector;

    Vector3 _limitA;
    Vector3 _limitB;

    private void OnValidate()
    {
        _limitA = Quaternion.AngleAxis(-_rotationRange / 2, Vector3.forward) * _sourceVector;
        _limitB = Quaternion.AngleAxis(_rotationRange / 2, Vector3.forward) * _sourceVector;
    }

    private void OnDrawGizmos()
    {
        var p = transform.position;
        Gizmos.color = Color.white;
        Gizmos.DrawRay(p, _sourceVector * LengthMultiplier);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(p, _rotatedVector * LengthMultiplier);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(p, _limitA * LengthMultiplier);
        Gizmos.DrawRay(p, _limitB * LengthMultiplier);
    }

    [ContextMenu("Rotate")]
    private void RotateVector()
    {
        var randomRotationAngle = Random.Range(-_rotationRange / 2, _rotationRange / 2);
        _rotatedVector = Quaternion.AngleAxis(randomRotationAngle, Vector3.forward) * _sourceVector;
    }
}
