using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfoHelper 
{
    public int playerMaxAmount;
    public float[] mapBoundaries = new float[4];

    public List<Vector3> spawnableAreas = new List<Vector3>();

    public List<Vector3> cryLocations = new List<Vector3>();
    public List<Vector3> minLocations = new List<Vector3>();
    public List<Vector3> acaciaLocations = new List<Vector3>();
    public List<Vector3> rockLocations = new List<Vector3>();

    public string surfaceMeshFilter;
    public string[] surface;
    public Vector3 surfaceScale;
    public Vector3 surfacePosition;
    public Vector3 surfaceCollider;
}
