using System.Collections.Generic;
using UnityEngine;
public class ServerStats
{
    public string unitName;

    public float health;
    public float maxHealth;

    public float percentageUnit;

    public float lineofSightRadius;
    public float lineofSightRadiusFog;
    public float lineofSightHitboxRadius;

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

    public List<Stats> spawnableUnits = new List<Stats>();

    public int unitLimitPusher;

    public void AddUnits(string name)
    {
        spawnableUnits.Add(Resources.Load<Stats>("GameObjects/Units/" + name));
    }
}
