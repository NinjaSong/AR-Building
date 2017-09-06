using UnityEngine;

public static class GyroUtility
{
    public static Quaternion GyroRotation()
    {
        // Handedness conversion

        Quaternion gyro = Input.gyro.attitude;
        gyro.z = -gyro.z;
        gyro.w = -gyro.w;

        // Orientation fix

        return Quaternion.Euler(90, 0, 90) * gyro;
    }
}