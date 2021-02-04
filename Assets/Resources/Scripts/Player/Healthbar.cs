using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public Stats target;
    SpriteRenderer bor;
    SpriteRenderer health;
    SpriteRenderer maxHealth;
    public HealthbarSpawner hbSpawn;
    float barHeight;
    float per;

    private void Start()
    {
        bor = GetComponent<SpriteRenderer>();
        health = transform.GetChild(0).GetComponent<SpriteRenderer>();
        maxHealth = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (target && target is BuildingPlaceholder && target.GetComponent<BuildingPlaceholder>().percent == 0)
        {
            bor.enabled = false;
            health.enabled = false;
            maxHealth.enabled = false;
        }
        else
        {
            bor.enabled = true;
            health.enabled = true;
            maxHealth.enabled = true;
        }

        if(target) barHeight = target.GetComponent<Collider>().bounds.size.z/2 + 0.5f;
        if (!target || target.health <= 0) gameObject.SetActive(false);
        else
        {
            BarPosition();
            if (target.maxHealth == 0) target.maxHealth = 0.1f;
            per = (target.health / target.maxHealth * 100) * 0.01f;
            BarSize();
            health.size = new Vector2(maxHealth.size.x * per, maxHealth.size.y);
            health.transform.position = new Vector3(maxHealth.transform.position.x - (maxHealth.size.x - health.size.x) / 2, maxHealth.transform.position.y, maxHealth.transform.position.z);
            gameObject.layer = target.gameObject.layer;
            transform.GetChild(0).gameObject.layer = target.gameObject.layer;
            transform.GetChild(1).gameObject.layer = target.gameObject.layer;
        }
    }

    void BarPosition()
    {
        Vector3 _myPos = new Vector3(target.transform.position.x, target.transform.position.y + 5.5f, target.transform.position.z + barHeight);
        transform.position = _myPos;
    }

    void BarSize()
    {
        SpriteRenderer _border = GetComponent<SpriteRenderer>();
        float maxHe = target.GetComponent<Collider>().bounds.size.x;
        if(maxHe > 1.5f)maxHealth.size = new Vector2(maxHe, maxHealth.size.y);        
        else maxHealth.size = new Vector2(1.5f, maxHealth.size.y);
        _border.size = new Vector2(maxHealth.size.x + 0.05f, _border.size.y);
        health.size = maxHealth.size;
    }
}
