using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class Server : MonoBehaviour
{

    public List<Entity> allPlayers = new List<Entity>();

    public List<float> allCrystals = new List<float>();
    public List<float> allMinerals = new List<float>();

    public List<SpawnRequirements> spawnRequirementList = new List<SpawnRequirements>();

    public List<List<Entity>> teams = new List<List<Entity>>();
    public List<Entity> winningCivs = new List<Entity>();
    public List<Entity> losingCivs = new List<Entity>();

    public List<int> playerUnitLimits = new List<int>();
    public List<int> playerCurrentUnitCounters = new List<int>();
    public List<List<House>> allHouses = new List<List<House>>();

    public List<List<Stats>> playerUnitsLists = new List<List<Stats>>();
    public List<List<Stats>> playerRealUnitsLists = new List<List<Stats>>();

    public List<List<Vector3>> unitLocations = new List<List<Vector3>>();

    public Dictionary<string, ServerStats> unitValues = new Dictionary<string, ServerStats>();

    public int playerID;
    public float timer;
    BuildingPlaceholder buiPla;
    public List<List<BuildingPlaceholder>> buiPlaList = new List<List<BuildingPlaceholder>>();

    public string map;
    public IConditions gameCondition;

    public bool load;

    UnlockablesManager unlockablesManager;
    public AnimatorManager anim;

    public int unlockableLevels;
    public AudioClip levelMusic;
    public SoundManager sound;

    public MapInfoSaver mapinfo;

    private void Awake()
    {
        mapinfo = FindObjectOfType<MapInfoSaver>();
        transform.parent = SceneManager.GetActiveScene().GetRootGameObjects()[0].transform;
        transform.name = "Server";
        buiPla = Resources.Load<BuildingPlaceholder>("GameObjects/Units/BuildingPlaceholder");
        StatAdder();
        foreach (var item in unitValues)
        {
            StatAssigner(item.Key);
        }
        sound = Camera.main.GetComponent<SoundManager>();
        /* if (PhotonNetwork.IsConnected)
         {
             if (playerID < PhotonNetwork.CurrentRoom.PlayerCount)
             {
                 _view.RPC("CreateNewPlayer", RpcTarget.AllBuffered, new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)), playerID);
             }
         }else
             CreateNewPlayer(new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)), 0, Color.green);*/
    }

    private void Start()
    {
        gameCondition.ServerSetter(this);
        if (!load)
        {
            gameCondition.GameStarter();
        }

        StartPrompt();
        unlockablesManager = new UnlockablesManager();
        anim = new AnimatorManager();
        if (!levelMusic) levelMusic = Resources.Load("Art/Music/track11") as AudioClip;
        sound.Play(levelMusic, true);
    }

    public void StartPrompt()
    {
        if (gameCondition.CanvasPrompt() != null)
        {
            Time.timeScale = 0;
            Canvas canv;
            canv = FindObjectOfType<Canvas>();
            var test = canv.transform.GetChild(canv.transform.childCount - 1).gameObject;
            test.SetActive(true);
            var test2 = test.transform.GetChild(0).gameObject;
            var test3 = test.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            test2.SetActive(true);
            test3.text = gameCondition.CanvasPrompt();
        }
    }

    void Update()
    {
        timer += 1 * Time.deltaTime;
        for (int i = 0; i < allPlayers.Count; i++)
        {
            allPlayers[i].ExecuteUpdate();
        }
        SpawnRequirementTimer();


        BuildingPlaceHolderColorChange();

        LocationKeeper();
        //StatComparer();
        ResourceManager();
        UnitLimitSetter();

        gameCondition.WinConditions();
        gameCondition.LossConditions();
        DeclareWin();
        //        if (PhotonNetwork.IsConnected && playerID < PhotonNetwork.CurrentRoom.PlayerCount)
        //            _view.RPC("CreateNewPlayer",RpcTarget.AllBuffered, new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)), playerID);

    }


    void Unlockables()
    {
        unlockablesManager.UpdateLevelsUnlocked(unlockableLevels);
        if (timer < 300) unlockablesManager.UnlockHyperOffender();
        if (allCrystals[0] > 1000) unlockablesManager.UnlockSmelter();
        if (allMinerals[0] > 1000) unlockablesManager.UnlockMiner();
        if (allMinerals[0] > 1000 && allCrystals[0] > 0100) unlockablesManager.UnlockHoarder();
        if (playerRealUnitsLists[0].Count == 200) unlockablesManager.UnlockHyperOffender();
    }

    void DeclareWin()
    {
        if (winningCivs.Count > 0)
        {
            UIManager uiman = allPlayers[0].GetComponent<UIManager>();
            if (winningCivs[0] == allPlayers[0])
            {
                uiman.EndGamePrompt(uiman.winPrompt);
                Unlockables();
            }
            else uiman.EndGamePrompt(uiman.lossPrompt);
        }
    }

    public Server SetCondition(IConditions c)
    {
        gameCondition = c;
        return this;
    }

    public void AddPlayer(Entity e)
    {
        if (!allPlayers.Contains(e))
        {
            allPlayers.Add(e);
        }
    }

    public void UnitLimitSetter()
    {
        for (int i = 0; i < allPlayers.Count; i++)
        {
            for (int j = 0; j < playerUnitsLists[i].Count; j++)
            {
                if (playerUnitsLists[i][j] is House && !allHouses[i].Contains(playerUnitsLists[i][j] as House))
                {
                    allHouses[i].Add(playerUnitsLists[i][j] as House);
                }
            }
        }
        for (int i = 0; i < allHouses.Count; i++)
        {
            for (int j = 0; j < allHouses[i].Count; j++)
            {
                if (allHouses[i][j].health < 1) allHouses[i].Remove(allHouses[i][j]);
            }
        }

        for (int i = 0; i < playerUnitLimits.Count; i++)
        {
            playerUnitLimits[i] = 10 + (10 * allHouses[i].Count);
        }

        /* for (int i = 0; i < playerCurrentUnitCounters.Count; i++)
         {
             for (int j = 0; j < playerUnitsLists.Count; j++)
             {
                 currentLimit += playerUnitsLists[i][j].unitLimitPusher;
             }
             playerCurrentUnitCounters[i] = currentLimit;
         }*/
    }

    public void RemovePlayer(Entity e)
    {
        if (allPlayers.Contains(e)) allPlayers.Remove(e);
    }


    public void CreateNewPlayer(Vector3 location, int faction, int team, Color factionColor, IController controller)
    {
        Entity civ;
        civ = Instantiate(Resources.Load<Entity>("GameObjects/Civilization"));

        controller.SetManagers(civ, civ.gameObject.GetComponent<SelectionManager>(), new BuildingManager(civ.transform.GetChild(0).GetComponent<PlacementChecker>()), civ.GetComponent<MovementManager>().ServerSetter(this), civ.gameObject.GetComponent<UIManager>());
        civ.SetController(controller);
        civ.transform.parent = SceneManager.GetActiveScene().GetRootGameObjects()[0].transform;

        /*
        if (PhotonNetwork.IsConnected)
            _view.RPC("AddPlayer", RpcTarget.AllBuffered, civ);
        else*/
        AddPlayer(civ);

        playerUnitsLists.Add(new List<Stats>());
        playerRealUnitsLists.Add(new List<Stats>());
        allHouses.Add(new List<House>());
        unitLocations.Add(new List<Vector3>());
        buiPlaList.Add(new List<BuildingPlaceholder>());
        playerCurrentUnitCounters.Add(0);
        playerUnitLimits.Add(0);
        allCrystals.Add(100);
        allMinerals.Add(100);
        civ.team = team;
        civ.color = factionColor;
        civ.transform.position = location;
        civ.crystals = allCrystals[playerID];
        civ.minerals = allMinerals[playerID];
        civ.id = playerID;
        civ.name = "Player " + (civ.id + 1);

        if (teams.Count < team + 1)
        {
            teams.Add(new List<Entity>());
        }
        teams[team].Add(civ);
        playerID++;
    }

    public bool ResourceCompare(int id, string name)
    {
        if (allCrystals[id] >= unitValues[name].cryCost && allMinerals[id] >= unitValues[name].minCost) return true;
        else return false;
    }

    public void RequestBuilding(Vector3 pos, Vector3 scale, Quaternion rot, MeshFilter mesh, Material[] mat, Transform par, string name, Builders builder)
    {
        BuildingPlaceholder newBuilding;

        newBuilding = Instantiate(buiPla);
        if (builder.civ.id != 0)
            Destroy(newBuilding.transform.GetComponentInChildren<FoVAdjuster>().gameObject);
        newBuilding.transform.position = pos;
        newBuilding.transform.rotation = rot;
        newBuilding.transform.localScale = scale;
        newBuilding.transform.parent = par;
        newBuilding.GetComponent<MeshFilter>().sharedMesh = mesh.sharedMesh;
        newBuilding.GetComponent<MeshRenderer>().materials = mat;
        newBuilding.GetComponent<BoxCollider>().size = new Vector3(0.04f, 0.05f, 0.05f);
        newBuilding.GetComponent<NavMeshObstacle>().size = new Vector3(0.04f, 0.05f, 0.05f);
        newBuilding.sel = newBuilding.GetComponentInParent<SelectionManager>();
        for (int i = 0; i < mat.Length; i++)
        {
            newBuilding.GetComponent<MeshRenderer>().materials[i].color = new Color(1, 1, 1, 0);
        }
        buiPlaList[builder.civ.id].Add(newBuilding);

        BuildingPlaceholder placeHolder = newBuilding.GetComponent<BuildingPlaceholder>();

        placeHolder.AssignGameObject(name);
        placeHolder.myPlaceHolder.maxHealth = unitValues[name].maxHealth;
        placeHolder.myPlaceHolder.armor = unitValues[name].armor;
        placeHolder.myPlaceHolder.cryCost = unitValues[name].cryCost;
        placeHolder.myPlaceHolder.minCost = unitValues[name].minCost;
        placeHolder.builders.Add(builder);
        builder.currentBuiPla = placeHolder;
        par.GetComponent<MovementManager>().Move(pos, builder);
    }

    public float RequestAttack(Stats attacker, Stats target)
    {
        float newHealth = target.health -= (attacker.damage - target.armor);
        return newHealth;
    }

    public float RequestDistance(Stats a, Stats b)
    {
        float dist;
        if (b != null)
        {
            dist = Vector3.Distance(a.transform.position, b.transform.position);
        }
        else dist = 0;
        return dist;
    }

    public Vector3 RequestMovement(Vector3 newPos)
    {
        return newPos;
    }

    public void Spawn(string name, int id, Vector3 pos)
    {
        Stats prefab;
        prefab = Resources.Load<Stats>("GameObjects/Units/" + name);
        prefab.health = prefab.maxHealth;
        prefab.attackSpeed = unitValues[name].maxAttackSpeed;
        prefab.movSpeed = unitValues[name].movSpeed;
        prefab.armor = unitValues[name].armor;
        prefab.unitLimitPusher = unitValues[name].unitLimitPusher;
        prefab.lineofSightRadius = unitValues[name].lineofSightRadius;
        prefab.lineofSightHitboxRadius = unitValues[name].lineofSightHitboxRadius;
        prefab.lineofSightHitboxRadiusFog = unitValues[name].lineofSightRadiusFog;
        //Assign stuff it can build
        playerCurrentUnitCounters[id] += prefab.unitLimitPusher;

        Stats spawning;

        spawning = Instantiate(prefab);
        if (id != 0) Destroy(spawning.transform.GetComponentInChildren<FoVAdjuster>().gameObject);
        playerUnitsLists[id].Add(spawning);
        if (!prefab.GetComponent<NotRealUnits>()) playerRealUnitsLists[id].Add(spawning);
        spawning.name = name;
        spawning.transform.position = pos;
        spawning.transform.parent = allPlayers[id].transform;
    }

    void StatAssigner(string name)
    {
        Stats prefab;
        prefab = Resources.Load<Stats>("GameObjects/Units/" + name);
        prefab.name = name;
        prefab.maxHealth = unitValues[name].maxHealth;
        prefab.percentageUnit = unitValues[name].percentageUnit;
        prefab.lineofSightRadius = unitValues[name].lineofSightRadius;
        prefab.lineofSightHitboxRadius = unitValues[name].lineofSightHitboxRadius;
        prefab.lineofSightHitboxRadiusFog = unitValues[name].lineofSightRadiusFog;
        prefab.maxAttackSpeed = unitValues[name].maxAttackSpeed;
        prefab.movSpeed = unitValues[name].maxMovSpeed;
        prefab.attackRange = unitValues[name].attackRange;
        prefab.hitboxX = unitValues[name].hitboxX;
        prefab.hitboxZ = unitValues[name].hitboxZ;
        prefab.damage = unitValues[name].damage;
        prefab.armor = unitValues[name].armor;
        prefab.cryCost = unitValues[name].cryCost;
        prefab.minCost = unitValues[name].minCost;
        prefab.layerSpawn = unitValues[name].layerSpawn;
        prefab.portrait = Resources.Load<Sprite>("Art/Visuals/Icons/Portraits/" + name + "_Portrait");
        prefab.description = unitValues[name].description;
    }

    void StatComparer()
    {
        for (int i = 0; i < playerUnitsLists.Count; i++)
        {
            for (int j = 0; j < playerUnitsLists[i].Count; j++)
            {
                string n = playerUnitsLists[i][j].name;
                if (playerUnitsLists[i][j].maxHealth != unitValues[n].maxHealth) playerUnitsLists[i][j].maxHealth = unitValues[n].maxHealth;
                if (playerUnitsLists[i][j].lineofSightRadius != unitValues[n].lineofSightRadius) playerUnitsLists[i][j].lineofSightRadius = unitValues[n].lineofSightRadius;
                if (playerUnitsLists[i][j].maxAttackSpeed != unitValues[n].maxAttackSpeed) playerUnitsLists[i][j].maxAttackSpeed = unitValues[n].maxAttackSpeed;
                if (playerUnitsLists[i][j].maxMovSpeed != unitValues[n].maxMovSpeed) playerUnitsLists[i][j].maxMovSpeed = unitValues[n].maxMovSpeed;
                if (playerUnitsLists[i][j].attackRange != unitValues[n].attackRange) playerUnitsLists[i][j].attackRange = unitValues[n].attackRange;
                if (playerUnitsLists[i][j].damage != unitValues[n].maxHealth) playerUnitsLists[i][j].damage = unitValues[n].damage;
                if (playerUnitsLists[i][j].armor != unitValues[n].armor) playerUnitsLists[i][j].armor = unitValues[n].armor;
            }
        }
    }

    void LocationKeeper()
    {
        for (int i = 0; i < playerUnitsLists.Count; i++)
        {
            for (int j = 0; j < playerUnitsLists[i].Count; j++)
            {
                if (unitLocations[i].Count < playerUnitsLists[i].Count) unitLocations[i].Add(Vector3.zero);
                if (unitLocations[i].Count > playerUnitsLists[i].Count) unitLocations[i].Remove(unitLocations[i][0]);
                unitLocations[i][j] = playerUnitsLists[i][j].transform.position;
            }
        }
    }

    public void SpawnRequirementAdder(SpawnRequirements t)
    {
        if (!spawnRequirementList.Contains(t)) spawnRequirementList.Add(t);
    }

    public void SpawnQueueAdder(Spawners spawner, Stats unit, int id, int i)
    {
        if (ResourceCompare(id, spawner.spawnGO[i].name) && playerCurrentUnitCounters[id] + unit.unitLimitPusher < playerUnitLimits[id])
        {

            allCrystals[id] -= unitValues[spawner.spawnGO[i].name].cryCost;
            allMinerals[id] -= unitValues[spawner.spawnGO[i].name].minCost;
            playerCurrentUnitCounters[id] += unit.unitLimitPusher;
            spawner.queue.Add(unit);
        }
        else
        {
            if (id != 0) return;
            UIManager uiMan = allPlayers[id].GetComponent<UIManager>();
            if (allCrystals[id] < unitValues[spawner.spawnGO[i].name].cryCost)
                uiMan.announcements.GetChild(0).gameObject.SetActive(true);
            if (allMinerals[id] < unitValues[spawner.spawnGO[i].name].minCost)
                uiMan.announcements.GetChild(1).gameObject.SetActive(true);
            if (playerCurrentUnitCounters[id] + unit.unitLimitPusher < playerUnitLimits[id])
                uiMan.announcements.GetChild(2).gameObject.SetActive(true);
        }
    }

    void SpawnRequirementTimer()
    {
        for (int i = 0; i < spawnRequirementList.Count; i++)
        {
            if (spawnRequirementList[i].timer < spawnRequirementList[i].cooldown) spawnRequirementList[i].timer += 1 * Time.deltaTime;
            else
            {
                Entity civ = spawnRequirementList[i].spawner.GetComponentInParent<Entity>();
                Stats spawnSound = Resources.Load<Stats>("GameObjects/Units/" + spawnRequirementList[i].spawner.queue[0].name);
                if (spawnSound.sounds.Length > 0 && civ.id == 0) sound.Play(spawnSound.sounds[0]);
                Spawn(spawnRequirementList[i].spawner.queue[0].name, civ.id, spawnRequirementList[i].spawner.spawnPosition);

                playerUnitLimits[civ.id] -= spawnRequirementList[i].spawner.queue[0].unitLimitPusher;
                spawnRequirementList[i].spawner.queue.RemoveAt(0);
                spawnRequirementList[i].spawner.spawning = false;
                spawnRequirementList.Remove(spawnRequirementList[i]);
            }
        }
    }

    public float AttackSpeed(float aspd, float maxAspd)
    {
        if (aspd < maxAspd)
        {
            return aspd += 1 * Time.deltaTime;
        }
        else return maxAspd;
    }


    void BuildingPlaceHolderColorChange()
    {
        if (buiPlaList.Count > 0)
        {
            for (int i = 0; i < buiPlaList.Count; i++)
            {
                for (int j = 0; j < buiPlaList[i].Count; j++)
                {
                    if (buiPlaList[i][j] && buiPlaList[i][j].percent > 0)
                    {
                        buiPlaList[i][j].ColorManagement(Color.white, buiPlaList[i][j].percent * 0.01f);
                        Color unbuilt = new Color(Color.white.r, Color.white.g, Color.white.b, buiPlaList[i][j].percent * 0.01f);
                        Renderer ren = buiPlaList[i][j].GetComponent<Renderer>();
                        for (int k = 0; k < ren.materials.Length; k++)
                        {
                            ren.materials[k].color = unbuilt;
                        }
                    }
                }
            }
        }
    }

    public void ResourceManager()
    {
        for (int i = 0; i < playerUnitsLists.Count; i++)
        {
            for (int j = 0; j < playerUnitsLists[i].Count; j++)
            {
                if (playerUnitsLists[i][j] is Furnace) allCrystals[i] += 3 * Time.deltaTime;
                if (playerUnitsLists[i][j] is Drill) allMinerals[i] += 4 * Time.deltaTime;
            }
        }
    }

    void StatAdder()
    {
        //Civ1
        {
            ServerStats house = new ServerStats
            {
                unitName = "House",
                maxHealth = 200,
                percentageUnit =  3.5f,
                lineofSightRadius = 1f,
                lineofSightRadiusFog = 0.53f,
                lineofSightHitboxRadius = 0.65f,
                hitboxX = 0.04f,
                hitboxZ = 0.05f,
                armor = 3,
                minCost = 60,
                layerSpawn = 0,
                description = "Allows you to increase population limits by 6."
            };
            unitValues.Add("House", house);

            ServerStats barracks = new ServerStats
            {
                unitName = "Barracks",
                maxHealth = 500,
                lineofSightRadius = 1.35f,
                lineofSightRadiusFog = 0.53f,
                lineofSightHitboxRadius = 0.65f,
                percentageUnit = 2f,
                hitboxX = 0.04f,
                hitboxZ = 0.05f,
                armor = 3,
                minCost = 150,
                description = "Allows you to create basic infantry."
            };
            barracks.AddUnits("Scorpion");
            barracks.AddUnits("Mech Soldier");
            unitValues.Add("Barracks", barracks);

            ServerStats mechSoldier = new ServerStats
            {
                unitName = "Mech Soldier",
                maxHealth = 60,
                percentageUnit = 45, //45
                lineofSightRadius = 74f,
                lineofSightHitboxRadius = 27,
                lineofSightRadiusFog = 20f,
                maxAttackSpeed = 1.5f,
                movSpeed = 5,
                attackRange = 18,
                damage = 9,
                armor = 5,
                cryCost = 60,
                minCost = 10,
                unitLimitPusher = 1,
                description = "A fast moving soldier with a gun at his disposal."
            };
            unitValues.Add("Mech Soldier", mechSoldier);

            ServerStats scorpion = new ServerStats
            {
                unitName = "Scorpion",
                maxHealth = 45,
                percentageUnit = 35, //35
                lineofSightRadius = 40,
                lineofSightHitboxRadius = 14,
                lineofSightRadiusFog = 30f,
                maxAttackSpeed = 0.7f,
                movSpeed = 7,
                attackRange = 5.2f,
                damage = 7,
                armor = 5,
                cryCost = 10,
                minCost = 40,
                unitLimitPusher = 1,
                description = "A swift yet frail robot which specializes in digging."
            };
            unitValues.Add("Scorpion", scorpion);

            ServerStats tank = new ServerStats
            {
                unitName = "Tank",
                maxHealth = 500,
                percentageUnit = 0f,
                lineofSightRadius = 40,
                lineofSightRadiusFog = 30f,
                attackRange = 15,
                maxAttackSpeed = 1.2f,
                movSpeed = 7,
                damage = 25,
                armor = 8,
                unitLimitPusher = 5,
                description = "A powerful tank prototype."
            };
            unitValues.Add("Tank", tank);

            ServerStats furnace = new ServerStats
            {
                unitName = "Furnace",
                maxHealth = 200,
                percentageUnit = 9.5f,
                lineofSightRadius = 800,
                lineofSightHitboxRadius = 800,
                lineofSightRadiusFog = 600f,
                hitboxX = 100f,
                hitboxZ = 62f,
                armor = 3,
                minCost = 80,
                layerSpawn = 9,
                description = "Automatically extracts Crystals."
            };
            unitValues.Add("Furnace", furnace);

            ServerStats drill = new ServerStats
            {
                unitName = "Drill",
                maxHealth = 200,
                percentageUnit = 9.5f,
                lineofSightRadius = 800,
                lineofSightHitboxRadius = 800,
                lineofSightRadiusFog = 600f,
                hitboxX = 47.36f,
                hitboxZ = 52.63f,
                armor = 3,
                layerSpawn = 10,
                description = "Automatically extracts Minerals."
            };
            unitValues.Add("Drill", drill);

            ServerStats worker = new ServerStats
            {
                unitName = "Worker",
                maxHealth = 50,
                percentageUnit = 30f,
                lineofSightRadius = 85,
                lineofSightHitboxRadius = 26f,
                lineofSightRadiusFog = 40f,
                maxAttackSpeed = 1.5f,
                attackRange = 5f,
                movSpeed = 4.5f,
                damage = 7,
                armor = 2,
                cryCost = 40,
                unitLimitPusher = 1,
                description = "Basic unit capable of building structures."
            };
            worker.AddUnits("House");
            worker.AddUnits("Barracks");
            worker.AddUnits("Furnace");
            worker.AddUnits("Drill");
            worker.AddUnits("WorkShop");
            unitValues.Add("Worker", worker);

            ServerStats workshop = new ServerStats
            {
                unitName = "Workshop",
                maxHealth = 700,
                percentageUnit = 1f,
                lineofSightRadius = 1.35f,
                lineofSightRadiusFog = 0.65f,
                lineofSightHitboxRadius = 0.53f,
                hitboxX = 0.064f,
                hitboxZ = 0.054f,
                minCost = 400,
                armor = 4,
                description = "Allows you to spawn Workers."
            };
            workshop.AddUnits("Worker");
            unitValues.Add("Workshop", workshop);
        }

        //Civ2
        {
            ServerStats ziggurat = new ServerStats
            {
                unitName = "Ziggurat",
                maxHealth = 220,
                percentageUnit = 3.5f,
                lineofSightRadius = 700,
                lineofSightRadiusFog = 40f,
                lineofSightHitboxRadius = 1,
                hitboxX = 78.74f,
                hitboxZ = 68.89f,
                armor = 3,
                minCost = 60,
                layerSpawn = 0,
                description = "Allows you to increase population limits by 6."
            };
            unitValues.Add("Ziggurat", ziggurat);

            ServerStats academy = new ServerStats
            {
                unitName = "Academy",
                maxHealth = 500,
                lineofSightRadius = 15f,
                lineofSightRadiusFog = 10f,
                lineofSightHitboxRadius = 6,
                percentageUnit = 20, //2f
                hitboxX = 1f,
                hitboxZ = 1f,
                armor = 3,
                minCost = 10, //150
                description = "Allows you to create basic infantry."
            };
            academy.AddUnits("Golem");
            academy.AddUnits("Devil");
            unitValues.Add("Academy", academy);

            ServerStats devil = new ServerStats
            {
                unitName = "Devil",
                maxHealth = 60,
                percentageUnit = 40f,
                lineofSightRadius = 86,
                lineofSightRadiusFog = 45f,
                lineofSightHitboxRadius = 30,
                maxAttackSpeed = 1.5f,
                movSpeed = 7,
                attackRange = 20,
                damage = 9,
                armor = 5,
                cryCost = 80,
                minCost = 10,
                unitLimitPusher = 1,
                description = "A weak yet fast familiar who can cast magic but cannot fly very high."
            };
            unitValues.Add("Devil", devil);

            ServerStats golem = new ServerStats
            {
                unitName = "Golem",
                maxHealth = 130,
                percentageUnit = 45,
                lineofSightRadiusFog = 40f,
                lineofSightRadius = 130,
                lineofSightHitboxRadius = 65,
                maxAttackSpeed = 0.9f,
                movSpeed = 3,
                attackRange = 5.7f,
                damage = 13,
                armor = 5,
                cryCost = 10,
                minCost = 60,
                unitLimitPusher = 1,
                description = "A large humanoid familiar made entirely out of stone. Capable of digging tunnels."
            };
            unitValues.Add("Golem", golem);

            ServerStats altar = new ServerStats
            {
                unitName = "Altar",
                maxHealth = 200,
                percentageUnit = 2f,
                lineofSightRadius = 40,
                lineofSightRadiusFog = 45f,
                lineofSightHitboxRadius = 30,
                hitboxX = 5f,
                hitboxZ = 5f,
                armor = 3,
                minCost = 80,
                layerSpawn = 9,
                description = "Automatically extracts Crystals."
            };
            unitValues.Add("Altar", altar);

            ServerStats gravityPortal = new ServerStats
            {
                unitName = "Gravity Portal",
                maxHealth = 200,
                percentageUnit = 9.5f,
                lineofSightRadius = 40,
                lineofSightRadiusFog = 45f,
                lineofSightHitboxRadius = 30,
                hitboxX = 5f,
                hitboxZ = 5f,
                armor = 3,
                layerSpawn = 10,
                description = "Automatically extracts Minerals."
            };
            unitValues.Add("Gravity Portal", gravityPortal);

            ServerStats summoner = new ServerStats
            {
                unitName = "Summoner",
                maxHealth = 50,
                percentageUnit = 30f,
                lineofSightRadius = 80,
                lineofSightRadiusFog = 50,
                lineofSightHitboxRadius = 23,
                movSpeed = 4,
                maxAttackSpeed = 1.5f,
                attackRange = 16f,
                damage = 7,
                armor = 2,
                cryCost = 50,
                unitLimitPusher = 1,
                description = "Basic unit capable of building structures."
            };
            summoner.AddUnits("Ziggurat");
            summoner.AddUnits("Academy");
            summoner.AddUnits("Altar");
            summoner.AddUnits("Gravity Portal");
            summoner.AddUnits("Fountain");
            unitValues.Add("Summoner", summoner);

            ServerStats fountain = new ServerStats
            {
                unitName = "Fountain",
                maxHealth = 600,
                percentageUnit = 1f,
                lineofSightRadius = 60,
                lineofSightRadiusFog = 50,
                lineofSightHitboxRadius = 45,
                hitboxX = 1.85f,
                hitboxZ = 1.85f,
                minCost = 400,
                armor = 4,
                description = "Allows you to spawn Summoners."
            };
            fountain.AddUnits("Summoner");
            unitValues.Add("Fountain", fountain);
        }
    }
}

//Fog of war
//Amount of workers working on a building
//current healths, attack speeds, movementspeeds
//Teams