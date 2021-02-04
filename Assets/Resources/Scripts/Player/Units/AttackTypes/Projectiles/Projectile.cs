using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Stats attacker;
    public Server ser;
    public int id;
    public IProjectileMovement movement;

    private void Update()
    {
        movement.Move();        
    }

    private void OnCollisionEnter(Collision c)
    {
        Stats col = c.gameObject.GetComponent<Stats>();
        if (col && col.civ.id != id)
        {
            ser.RequestAttack(attacker, col);
        }
        if (c.gameObject != attacker.gameObject) Destroy(gameObject);
    }

    public Projectile SetMovement(IProjectileMovement m)
    {
        movement = m;
        return this;
    }
}
