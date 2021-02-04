using UnityEngine;
using UnityEngine.AI;

public class Stats : MonoBehaviour
{
    public string unitName;

    public float health;
    public float maxHealth;

    public float percentageUnit;

    public float lineofSightRadius;
    public float lineofSightHitboxRadius;
    public float lineofSightHitboxRadiusFog;

    public float attackSpeed;
    public float maxAttackSpeed;

    public float movSpeed;
    public float maxMovSpeed;

    public float attackRange;

    public float hitboxX;
    public float hitboxZ;

    public int damage;
    public int armor;

    public int cryCost;
    public int minCost;

    public int layerSpawn;

    public Sprite portrait;
    public string description;

    public int unitLimitPusher;
    public AudioClip[] sounds;
    public Entity civ;
    public SoundManager sound;
    public Stats target;

    protected virtual void Start()
    {
        civ = GetComponentInParent<Entity>();
        if (sound == null) sound = Camera.main.GetComponent<SoundManager>();
        if (GetComponent<NavMeshAgent>()) GetComponent<NavMeshAgent>().speed = movSpeed;
        //UnitLimitPusher();
    }

    protected virtual void Update()
    {
        Dying();
    }

    public void Dying()
    {
        if(health<=0)
        {
            SelectionManager sel = GetComponentInParent<SelectionManager>();
            civ = GetComponentInParent<Entity>();
            civ.server.playerCurrentUnitCounters[civ.id] -= unitLimitPusher;
            if (civ.server.playerUnitsLists[civ.id].Contains(this)) civ.server.playerUnitsLists[civ.id].Remove(this);
            if (civ.server.playerRealUnitsLists[civ.id].Contains(this)) civ.server.playerRealUnitsLists[civ.id].Remove(this);
            if (sel.units.Contains(this)) sel.units.Remove(this);
            if (sel.selectedUnits.Contains(this)) sel.selectedUnits.Remove(this);
            if (sel.highlightedUnits.Contains(this)) sel.highlightedUnits.Remove(this);
            if (civ.GetComponent<MovementManager>().allMovingUnits.Contains(this)) civ.GetComponent<MovementManager>().allMovingUnits.Remove(this);
            sound.Play(sounds[sounds.Length-1]);
            Destroy(gameObject);
        }
    }
   
}
