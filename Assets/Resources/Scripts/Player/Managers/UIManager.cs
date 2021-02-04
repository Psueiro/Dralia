using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;

public class UIManager : MonoBehaviour
{
    public Transform canv;
    public Transform statMenu;
    Transform buildMenu;
    Transform massMenu;
    Transform skillMenu;
    Transform hover;
    public PauseMenus pauseOverlay;
    Transform spawnBar;
    public Transform announcements;
    Text timer;
    Text crystals;
    Text minerals;
    Text unitLimits;
    Server ser;
    SelectionManager sel;
    BuildingManager bui;
    Entity civ;
    public Minimap miniMap;
    DirectoryInfo dirInfo;
    public Image winPrompt;
    public Image lossPrompt;
    SoundManager sound;

    void Start()
    {
        civ = GetComponent<Entity>();
        ser = civ.server;
        canv = transform.parent.GetComponentInChildren<Canvas>().transform;
        buildMenu = canv.GetChild(0);
        massMenu = canv.GetChild(1);
        statMenu = canv.GetChild(3);
        hover = canv.GetChild(6);
        timer = canv.GetChild(4).GetChild(3).GetComponent<Text>();
        announcements = canv.GetChild(7);
        pauseOverlay = canv.GetChild(canv.childCount - 1).GetComponent<PauseMenus>();
        crystals = canv.GetChild(5).GetChild(1).GetChild(0).GetComponent<Text>();
        minerals = canv.GetChild(5).GetChild(1).GetChild(1).GetComponent<Text>();
        unitLimits = canv.GetChild(8).GetChild(1).GetChild(0).GetComponent<Text>();
        winPrompt = pauseOverlay.transform.GetChild(3).GetComponent<Image>();
        lossPrompt = pauseOverlay.transform.GetChild(4).GetComponent<Image>();
        spawnBar = statMenu.GetChild(2);
        miniMap = new Minimap(canv.GetChild(2).GetChild(0).GetChild(0).GetComponent<RectTransform>(),FindObjectOfType<CamController>());
        dirInfo = new DirectoryInfo(Application.streamingAssetsPath + "/Saves");
        if (sound == null) sound = Camera.main.GetComponent<SoundManager>();
    }

    public void EndGamePrompt(Image i)
    {
        pauseOverlay.gameObject.SetActive(true);
        i.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public UIManager SetBuildingManager(BuildingManager b)
    {
        bui = b;
        return this;
    }

    public UIManager SetSelectionManager(SelectionManager s)
    {
        sel = s;
        return this;
    }

    void Update()
    {
        Timer();
        ResourceUpdater();
        UnitLimitUpdater();
        StartCoroutine(TurnOffAnnouncements());
        miniMap.Update();
        RemoveCampaignPrompt();

        if (pauseOverlay.transform.GetChild(1).GetChild(8).gameObject.activeSelf)
            SaveAmountinLoadMenu(dirInfo);

        if (sel.selectedUnits.Count > 1)
        {
            MassSelectionDisplay();
            buildMenu.gameObject.SetActive(false);
            statMenu.gameObject.SetActive(false);
        }
        else if(sel.selectedUnits.Count==1)
        {
            statMenu.gameObject.SetActive(true);
            StatDisplay();
            massMenu.gameObject.SetActive(false);
            if(!sel.selectedUnits[0].GetComponent<Builders>())
            hover.gameObject.SetActive(false);

            if(sel.selectedUnits[0] is BuildingPlaceholder)
            {
                spawnBar.position = new Vector2(425 * canv.localScale.x, 20 * canv.localScale.y);
                spawnBar.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 10);
                spawnBar.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(246, 6);
                spawnBar.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(246, 6);
                spawnBar.gameObject.SetActive(true);
                BuildingPlaceholder b = sel.selectedUnits[0].GetComponent<BuildingPlaceholder>();
                spawnBar.transform.GetChild(1).GetComponent<Image>().fillAmount = b.percent / 100;
            }else if(sel.selectedUnits[0] is Spawners)
            {
                Spawners s = sel.selectedUnits[0].GetComponent<Spawners>();
                if(s.spawning)
                {
                    spawnBar.position = new Vector2(55 * canv.localScale.x, 20 * canv.localScale.y);
                    spawnBar.GetComponent<RectTransform>().sizeDelta = new Vector2(89,8);
                    spawnBar.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(85, 4);
                    spawnBar.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(85, 4);
                    spawnBar.gameObject.SetActive(true);
                    spawnBar.transform.GetChild(1).GetComponent<Image>().fillAmount = ser.spawnRequirementList[0].timer / ser.spawnRequirementList[0].cooldown;
                }else spawnBar.gameObject.SetActive(false);
            }
            else spawnBar.gameObject.SetActive(false);
        }
        else
        {
            statMenu.gameObject.SetActive(false);
            buildMenu.gameObject.SetActive(false);
            massMenu.gameObject.SetActive(false);
            hover.gameObject.SetActive(false);
        }
    }

    private void RemoveCampaignPrompt()
    {
        GameObject prompt;
        prompt = pauseOverlay.transform.GetChild(0).gameObject;
        if (prompt.activeSelf == true)
        if (Input.anyKey)
        {
            prompt.SetActive(false);
            pauseOverlay.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void Timer()
    {
        float t = ser.timer;

        string minutes = ((int)t / 60).ToString();
        string seconds = Mathf.Round((t % 60)).ToString();
        timer.text = minutes +":"+ seconds;
    }

    void ResourceUpdater()
    {
        crystals.text = "Cry:" + Mathf.Round(ser.allCrystals[civ.id]);
        minerals.text = "Min:" + Mathf.Round(ser.allMinerals[civ.id]);
    }

    void UnitLimitUpdater()
    {
        unitLimits.text = "Units: " + ser.playerCurrentUnitCounters[civ.id] + " / " + ser.playerUnitLimits[civ.id];
    }

    IEnumerator TurnOffAnnouncements()
    {
         for (int i = 0; i<announcements.childCount; i++)
        {
            if (announcements.GetChild(i).gameObject.activeSelf)
                yield return new WaitForSeconds(2);
            announcements.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Pause()
    {
        pauseOverlay.gameObject.SetActive(!pauseOverlay.gameObject.activeSelf);
        pauseOverlay.transform.GetChild(pauseOverlay.transform.childCount-1).GetComponent<Text>().text = pauseOverlay.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
        //if not online
        if (Time.timeScale == 0) Time.timeScale = 1;
        else Time.timeScale = 0;
    }

    public void OpenThis(int c)
    {
        pauseOverlay.transform.GetChild(c).gameObject.SetActive(!pauseOverlay.transform.GetChild(c).gameObject.activeSelf);
    }

    void StatDisplay()
    {
        Transform childNav = statMenu.GetChild(0).GetChild(2);
        Text name = statMenu.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
        Text health = childNav.GetChild(1).GetComponent<Text>();
        Text attack =  childNav.GetChild(2).GetComponent<Text>();
        Text armor = childNav.GetChild(3).GetComponent<Text>();
        Text aspd = childNav.GetChild(4).GetComponent<Text>();
        Text speed = childNav.GetChild(5).GetComponent<Text>();

        statMenu.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = sel.selectedUnits[0].portrait;
        name.text = sel.selectedUnits[0].name;
        health.text = "Life:" + Mathf.Round(sel.selectedUnits[0].health) +"/"+ sel.selectedUnits[0].maxHealth;

        if (sel.selectedUnits[0].damage > 0)
            armor.text = "Def:" + sel.selectedUnits[0].armor;
        else armor.text = " ";

        if (sel.selectedUnits[0].damage > 0)
            attack.text = "Dmg:" + sel.selectedUnits[0].damage;
        else attack.text = "Def:" + sel.selectedUnits[0].armor; 

        if (sel.selectedUnits[0].maxAttackSpeed > 0)
            aspd.text = "Aspd:" + sel.selectedUnits[0].maxAttackSpeed * 10;
        else aspd.text = " ";

        if (sel.selectedUnits[0].maxMovSpeed > 0)
            speed.text = "Spe:" + sel.selectedUnits[0].movSpeed;
        else speed.text = " ";

        if (sel.selectedUnits[0] is Builders)
        {
            BuildingDisplay();
        }
        else buildMenu.gameObject.SetActive(false);
    }

    void BuildingDisplay()
    {
        if (sel.selectedUnits[0].civ.id != civ.id) return;
        CountResetter(buildMenu);

        Builders builder = sel.selectedUnits[0].GetComponent<Builders>();   
        for (int i = 0; i < builder.spawnGO.Count; i++)
        {
            buildMenu.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = builder.spawnGO[i].portrait;
            buildMenu.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = builder.spawnGO[i].name + " (" + builder.keys[i] + ")";
            buildMenu.GetChild(i).gameObject.SetActive(true);
            HoverButtonSetter(i, builder);
            if (builder is Spawners)
            {
                Spawners spawner = builder.GetComponent<Spawners>();
                SpawnButtonSetter(i, spawner, builder.spawnGO[i]);          
            }else BuildButtonSetter(i, builder.spawnGO[i]);
        }
        buildMenu.gameObject.SetActive(true);
    }

    void BuildButtonSetter(int i, Stats s)
    {
        buildMenu.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        buildMenu.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.AddListener(() => bui.SetPlacementChecker(s)); 
    }

    void SpawnButtonSetter(int i, Spawners b, Stats s)
    {
        buildMenu.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        buildMenu.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.AddListener(() => ser.SpawnQueueAdder(b,s, civ.id,i));       
    }

    void HoverButtonSetter(int i, Builders b)
    {
        EventTrigger t = buildMenu.transform.GetChild(i).GetChild(0).GetComponent<EventTrigger>();
        t.triggers.Clear();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data, i, b); });
        t.triggers.Add(entry);

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
        t.triggers.Add(exit);
    }

    void OnPointerEnterDelegate(PointerEventData data, int i, Builders b)
    {
        var item = buildMenu.transform.GetChild(i);

        hover.transform.position = new Vector2(item.GetChild(0).transform.position.x, item.GetChild(0).transform.position.y + 70  * canv.localScale.y);
        Text name = hover.GetChild(2).GetComponent<Text>();
        name.text = b.spawnGO[i].name + " ("+b.keys[i]+")";

        Text crycost = hover.GetChild(3).GetComponent<Text>();
        if(b.spawnGO[i].cryCost>0)
        crycost.text = "Crys:" + b.spawnGO[i].cryCost;
        else crycost.text = "Min:" + b.spawnGO[i].minCost;

        Text mincost = hover.GetChild(4).GetComponent<Text>();
        if (b.spawnGO[i].cryCost > 0 && b.spawnGO[i].minCost > 0)
            mincost.text = "Min:" + b.spawnGO[i].minCost;
        else mincost.text = " ";

        Text description = hover.GetChild(5).GetComponent<Text>();
        description.text = b.spawnGO[i].description;

            hover.gameObject.SetActive(true);
    }

    void OnPointerExitDelegate(PointerEventData data)
    {
        if (hover.gameObject.activeSelf)
            hover.gameObject.SetActive(false);
    }

    void SelectionSetter(int i)
    {
        massMenu.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        massMenu.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.AddListener(() => SelectOnlyThis(i));
    }

    void SelectOnlyThis(int o)
    {
        Stats unit = sel.selectedUnits[o];
        sel.selectedUnits.Clear();
        sel.selectedUnits.Add(unit);
        if(unit.civ.id == civ.id)
        sound.Play(unit.sounds[0]);
    }

    void MassSelectionDisplay()
    {
        CountResetter(massMenu);
        for (int i = 0; i < sel.selectedUnits.Count; i++)
        {
            massMenu.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = sel.selectedUnits[i].portrait;
            massMenu.GetChild(i).gameObject.SetActive(true);
            SelectionSetter(i);
        }
        massMenu.gameObject.SetActive(true);
    }

    void CountResetter(Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).gameObject.activeSelf)
            {
                t.GetChild(i).gameObject.SetActive(false);
            }
            else break;
        }
    }

    void SaveAmountinLoadMenu(DirectoryInfo dir)
    {
        FileInfo[] files = dir.GetFiles("*.json");
        var saveField = pauseOverlay.transform.GetChild(1).GetChild(8).GetChild(8).GetChild(0).GetChild(0).GetChild(0);

        for (int i = 0; i < files.Length; i++)
        {
            saveField.GetChild(i).GetChild(0).GetComponent<Text>().text = files[i].Name.Remove(files[i].Name.Length - 5);
            saveField.GetChild(i).gameObject.SetActive(true);
        }
    }

}

//Limit selection to 30
//Add Abilities
//Make it so whenever a selected unit gets killed the spots get filled and it doesn't explode
//Add the StatMenu for whenever you have many units selected