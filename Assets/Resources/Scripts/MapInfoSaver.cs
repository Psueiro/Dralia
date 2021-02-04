using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.AI;
using UnityEngine.UI;

public class MapInfoSaver : MonoBehaviour
{
    public string mapName;
    string path;
    string data;

    public int playerMaxAmount;
    float[] mapBoundaries = new float[4];

    public CamController cam;
    public Transform crystals;
    public Transform minerals;
    public Transform trees;
    public Transform rocks;

    public List<Vector3> spawnableAreas;

    public List<Vector3> cryLocations;
    public List<Vector3> minLocations;
    public List<Vector3> acaciaLocations;
    public List<Vector3> rockLocations;

    public string surfaceMeshFilter;
    public string[] surface;

    Vector3 surfaceScale;
    Vector3 surfacePosition;
    Vector3 surfaceCollider;

    MapInfoHelper helper;
    public RenderTexture mapScreenshot;


    NavMeshSurface nav;

    void Start()
    {
        cam = FindObjectOfType<CamController>();
        mapBoundaries[0] = cam.minlimitx;
        mapBoundaries[1] = cam.maxlimitx;
        mapBoundaries[2] = cam.minlimity;
        mapBoundaries[3] = cam.maxlimity;
        crystals = transform.GetChild(1);
        minerals = transform.GetChild(2);
        trees = transform.GetChild(4).GetChild(0);
        rocks = transform.GetChild(4).GetChild(1);

        LocationGatherer(crystals, cryLocations);
        LocationGatherer(minerals, minLocations);
        LocationGatherer(trees, acaciaLocations);
        LocationGatherer(rocks, rockLocations);
        mapScreenshot = Resources.Load<RenderTexture>("Art/Visuals/Textures/MinimapTextures/Minimap");

        surfaceMeshFilter = transform.GetChild(0).GetComponent<MeshFilter>().mesh.name;

        Renderer rend = transform.GetChild(0).GetComponent<Renderer>();

        surface = new string[rend.materials.Length];
        for (int i = 0; i < rend.materials.Length; i++)
        {
            surface[i] = rend.materials[i].name;
        }

        surfacePosition = transform.GetChild(0).transform.position;
        surfaceScale = transform.GetChild(0).transform.localScale;
        surfaceCollider = transform.GetChild(0).GetComponent<Collider>().bounds.size;
        path = Application.streamingAssetsPath + "/MapInfo/"+mapName+".json";
        
        if(!File.Exists(path) && mapName != "")
        File.WriteAllText(path,"{ }");        
        //data = File.ReadAllText(path);
    }
  
    void LocationGatherer(Transform t, List<Vector3> v)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            v.Add(t.GetChild(i).transform.position);
        }
    }

    public void SaveMapInfo()
    {
        data = File.ReadAllText(path);
        helper = JsonUtility.FromJson<MapInfoHelper>(data);
        helper.playerMaxAmount = playerMaxAmount;
        helper.spawnableAreas.Clear();
        helper.cryLocations.Clear();
        helper.minLocations.Clear();
        helper.acaciaLocations.Clear();
        helper.rockLocations.Clear();
        helper.surface = new string[surface.Length];
        for (int i = 0; i < spawnableAreas.Count; i++)
        {
            helper.spawnableAreas.Add(spawnableAreas[i]);
        }

        for (int i = 0; i < mapBoundaries.Length; i++)
        { 
            helper.mapBoundaries[i] = mapBoundaries[i];
        }        
        for (int i = 0; i < cryLocations.Count; i++)
        {
            helper.cryLocations.Add(cryLocations[i]);
        }
        for (int i = 0; i < minLocations.Count; i++)
        {
            helper.minLocations.Add(minLocations[i]);
        }
        for (int i = 0; i < acaciaLocations.Count; i++)
        {
            helper.acaciaLocations.Add(acaciaLocations[i]);
        }
        for (int i = 0; i < rockLocations.Count; i++)
        {
            helper.rockLocations.Add(rockLocations[i]);
        }
        helper.surfaceMeshFilter = surfaceMeshFilter.Remove(surfaceMeshFilter.Length - 9);
        for (int i = 0; i < surface.Length; i++)
        {
            helper.surface[i] = surface[i].Remove(surface[i].Length-11);
        }
        helper.surfacePosition = surfacePosition;
        helper.surfaceScale = surfaceScale;
        helper.surfaceCollider = surfaceCollider;

        data = JsonUtility.ToJson(helper);
        File.WriteAllText(path, data);
        SaveTexture();
    }

    public void LoadMapInfo()
    {
        data = File.ReadAllText(path);
        helper = JsonUtility.FromJson<MapInfoHelper>(data);
    }


    public void SaveTexture()
    {
        byte[] bytes = toTexture2D(mapScreenshot).EncodeToPNG();
        File.WriteAllBytes(Application.streamingAssetsPath + "/Resources/MapInfo/Previews/" + mapName + ".png", bytes);
    }
    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(140, 140, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

}
