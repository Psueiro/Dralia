using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BuildingPlaceholder : NotRealUnits
{
    public Stats myPlaceHolder;
    public float percent;
    public float tresholdDistance;
    public SelectionManager sel;
    public List<Builders> builders;
    public List<Builders> nearbyBuilders;
    Renderer ren;
    Server ser;
    int id;
    bool check;
    BoxCollider col;
    NavMeshObstacle nav;


    void Awake()
    {
        health = 0.1f;
        ren = GetComponent<Renderer>();
        col = GetComponent<BoxCollider>();
        nav = GetComponent<NavMeshObstacle>();
    }
    protected override void Start()
    {

        ser = sel.GetComponent<Entity>().server;
        id = sel.GetComponent<Entity>().id;
        //_view.RPC("ColliderAdapter", RpcTarget.MasterClient);
        ColliderAdapter();
        col.enabled = false;
        nav.enabled = false;
        AssignStats(ser.unitValues[myPlaceHolder.name]);
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (percent >= 100)
            FinishBuild();

            DistanceManager();

        if(percent== 0 && builders.Count == 0)
        {
            Remover();
            Destroy(gameObject);
        }

        if(percent > 0 && !check)
        {
            col.enabled = true;
            nav.enabled = true;
            Cost();
            check = true;
        }
    }


    void ColliderAdapter()
    {
        Vector3 sizes = new Vector3(myPlaceHolder.hitboxX, myPlaceHolder.hitboxZ, myPlaceHolder.hitboxZ);
        if (myPlaceHolder.GetComponent<MeshRenderer>())
        {
            col.size = sizes;
            if (nav) nav.size = sizes;
        }else
        {
            col.size = sizes * 10;
            if (nav) nav.size = sizes * 10;
        }
    }
    
    void DistanceManager()
    {
        for (int i = 0; i < builders.Count; i++)
        {
            if (ser.RequestDistance(GetComponent<Stats>(), builders[i] as Stats) < tresholdDistance)
            {
                if(!nearbyBuilders.Contains(builders[i]))
                nearbyBuilders.Add(builders[i]);
            }
            else nearbyBuilders.Remove(builders[i]);
        }
        for (int j = 0; j < nearbyBuilders.Count; j++)
        {
            if (!builders.Contains(nearbyBuilders[j])) nearbyBuilders.Remove(nearbyBuilders[j]);
        }
        BeingBuilt(percentageUnit, nearbyBuilders.Count);
    }

    public void AssignGameObject(string goName)
    {
        myPlaceHolder = Resources.Load<Stats>("GameObjects/Units/"+goName);
    }

    void AssignStats(ServerStats stats)
    {
        gameObject.name = stats.unitName;
        maxHealth = stats.maxHealth;
        percentageUnit = stats.percentageUnit;
        lineofSightRadius = stats.lineofSightRadius;
        portrait = Resources.Load<Sprite>("Art/Visuals/Icons/Portraits/" + name + "_Portrait");
    }

    public void ColorManagement(Color c, float f)
    {
        Color unbuilt = new Color(c.r,c.g,c.b, f);
        for (int i = 0; i < ren.materials.Length; i++)
        {
            ren.materials[i].color = unbuilt;
        }
    }

    void BeingBuilt(float amountAdded, int builderAmount)
    {
        percent += amountAdded * builderAmount * Time.deltaTime;
        if (health < maxHealth)
            health += ((percentageUnit * maxHealth)/100) * builderAmount * Time.deltaTime;
        else health = maxHealth;
        for (int i = 0; i < nearbyBuilders.Count; i++)
        {
            ser.anim.AnimationChange(nearbyBuilders[i], ser.anim.allAnimationNames[1]);
        }
    }

    void Cost()
    {
        ser.allCrystals[id] -= ser.unitValues[myPlaceHolder.name].cryCost;
        ser.allMinerals[id] -= ser.unitValues[myPlaceHolder.name].minCost;
    }

    void FinishBuild()
    {
        for (int i = 0; i < builders.Count; i++)
        {
            ser.anim.DisableAllAnims(builders[i]);
        }
        if(builders[0].civ.id == 0)
        Camera.main.GetComponent<SoundManager>().Play(myPlaceHolder.sounds[0]);
        ser.Spawn(myPlaceHolder.name, id, transform.position);
        Remover();
        Destroy(gameObject);        
    }

    void Remover()
    {
        sel.highlightedUnits.Remove(this);
        sel.selectedUnits.Remove(this);
        sel.units.Remove(this);
        ser.buiPlaList[civ.id].Remove(this);
    }
}
