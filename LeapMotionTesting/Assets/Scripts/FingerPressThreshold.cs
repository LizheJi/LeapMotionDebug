using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FingerPressThreshold
{
    [Range(0, 90)]
    public float Down;
    [Range(0, 90)]
    public float Up;
}
