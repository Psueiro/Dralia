using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : IAttackType
{
    Projectile projectile;
    IProjectileMovement movement;
    MovementManager m;
    public Shooting(Projectile p, IProjectileMovement m)
    {
        projectile = p;
        movement = m;
    }

    public void Attack(Stats a, Stats t, Server ser, float maxAttackSpeed, float range)
    {
        if (m == null) m = a.civ.GetComponent<MovementManager>();
        if (a.attackSpeed >= a.maxAttackSpeed && ser.RequestDistance(a, t) < range)
        {
            a.attackSpeed = 0;
            projectile.attacker = a;
            projectile.ser = ser;
            projectile.id = a.civ.id;
            projectile.transform.position = a.transform.position + new Vector3(0, 2, 0);
            var newProj = MonoBehaviour.Instantiate(projectile);
            newProj.transform.parent = a.transform.GetComponentInParent<Civilization>().transform.GetChild(2);
            newProj.SetMovement(movement.Clone());
            newProj.movement.SetOrigin(projectile.transform.position);
            newProj.movement.SetTransform(newProj.transform);
            newProj.movement.SetTarget(t.transform.position);
            m.Move(a.transform.position, a);
            a.sound.Play(a.sounds[2]);
        }
        else m.Move(t.transform.position, a);
    }
}
