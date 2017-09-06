using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ARCoordinateUtility
{
    // The coordinates of the Lab! This latitude and longitude is the zero point in Unity.

    public static float LAT_ORIGIN = +33.776668f;
    public static float LON_ORIGIN = -84.398290f;

    // This conversion from latitude and longitude to a cartesian mapping of meters
    // is obviously an approximation, but because we are only mapping points within
    // an extremely small area of Tech's campus, any inaccuracies are much smaller
    // than the average accuracy of GPS.

    public static Vector3 LatLonToPosition(float lat, float lon)
    {
        float latMid = Mathf.Deg2Rad * (LAT_ORIGIN + lat) / 2;

        float metersPerDegreeLat = 111132.954f - 559.822f * Mathf.Cos(latMid * 2) + 1.175f * Mathf.Cos(latMid * 4);
        float metersPerDegreeLon = Mathf.Deg2Rad * 6367449 * Mathf.Cos(latMid);

        float latDelta = lat - LAT_ORIGIN;
        float lonDelta = lon - LON_ORIGIN;

        return new Vector3(lonDelta * metersPerDegreeLon, 0, latDelta * metersPerDegreeLat);
    }
}