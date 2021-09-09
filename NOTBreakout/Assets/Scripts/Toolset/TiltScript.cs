using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TiltScript
{
    static Vector2 offset;
    static float sensitivity;

    public static Vector2 Get2DTilt()
    {
        Vector2 tilt = ((Vector2)Input.gyro.gravity - offset) * sensitivity;
        tilt = new Vector2(Mathf.Clamp(tilt.x, -1, 1), Mathf.Clamp(tilt.y, -1, 1));
        return tilt;
    }

    public static void SetTiltOffset(Vector2 _offset) => offset = _offset;
    public static void SetTiltSensitivity(float _sensitivity) => sensitivity = _sensitivity;
}
