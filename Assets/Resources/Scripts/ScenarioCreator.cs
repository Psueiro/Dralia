using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class ScenarioCreator : MonoBehaviour
{
    IConditions condition;
    Server server;
    string path;
    string data;
    string mapName;
    MapInfoHelper helper;
    SaveManager sav;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "SinglePlayerSampleScene" && mapName != null)
        {
            LoadMapInfo();
            StartScenario();
        }
    }

    public void LoadMapInfo()
    {
        path = Application.streamingAssetsPath + "/MapInfo/" + mapName + ".json";
        data = File.ReadAllText(path);
        helper = JsonUtility.FromJson<MapInfoHelper>(data);
        Transform app = SceneManager.GetActiveScene().GetRootGameObjects()[0].transform;
        if (!FindObjectOfType<Server>() && app.childCount > 0)
        {
            CreateMapAssets(helper.cryLocations, "Crystal", app.GetChild(2).GetChild(0).GetChild(1));
            CreateMapAssets(helper.minLocations, "Mineral", app.GetChild(2).GetChild(0).GetChild(2));
            CreateMapAssets(helper.acaciaLocations, "Acacia", app.GetChild(2).GetChild(0).GetChild(4).GetChild(0));          
            CreateMapAssets(helper.rockLocations, "Stone", app.GetChild(2).GetChild(0).GetChild(4).GetChild(1));

            //Surface part
            GameObject surface;
            surface = app.GetChild(2).GetChild(0).GetChild(0).gameObject;
            surface.transform.localScale = helper.surfaceScale;
            surface.GetComponent<Renderer>().material = Resources.Load<Material>("Art/Visuals/Textures/Materials/" + helper.surface[0]);


            //surface.GetComponent<MeshFilter>().mesh = ;

            //surface.GetComponent<Renderer>().materials = new Material[helper.surface.Length];

            //for (int i = 0; i < helper.surface.Length; i++)
            //{
            //    surface.GetComponent<Renderer>().materials[0] = Resources.Load<Material>("Art/Visuals/Textures/Materials/" + helper.surface[i]);
            //}

                  
                  //for (int i = 0; i < helper.surface.Length; i++)
                  //{
                  //}

                  //surface.GetComponent<MeshCollider>().bounds = helper.surfaceCollider;
        }

        if (FindObjectOfType<NavMeshSurface>())
            FindObjectOfType<NavMeshSurface>().BuildNavMesh();
    }

    void CreateMapAssets(List<Vector3> l, string itemName, Transform t)
    {
        for (int i = 0; i < l.Count; i++)
        {
            GameObject item;
            item = (GameObject)Instantiate(Resources.Load("GameObjects/Scenario/"+itemName));
            item.transform.position = l[i];           
            item.transform.parent = t;            
        }
    }

    void StartScenario()
    {
        if (!FindObjectOfType<Server>())
        server = Instantiate(Resources.Load<Server>("GameObjects/Server"));
        if (sav)
        {
            server.load = true;
            sav.ServerSetter(server);
            sav.ServerUpdate();
        }
        server.map = mapName;
        server.SetCondition(condition);
        Time.timeScale = 1;
        Destroy(gameObject);
    }

    public ScenarioCreator SavSetter(SaveManager save)
    {
        sav = save;
        return this;
    }

    public ScenarioCreator MapNameSetter(string s)
    {
        mapName = s;
        return this;
    }    

    public ScenarioCreator ConditionSetter(IConditions c)
    {
        condition = c;
        return this;
    }

}