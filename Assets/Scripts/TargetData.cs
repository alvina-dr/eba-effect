using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetData : ScriptableObject
{
    public enum targetType
    {
        Fixed,
        Moving
    }
    public enum TargetSide
    {
        left,
        right
    }
    public float spawnTime;
    public float duration;
    public Vector3 startPosition;
    public TargetSide targetSide;
}
