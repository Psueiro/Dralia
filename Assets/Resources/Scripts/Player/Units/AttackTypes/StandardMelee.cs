using UnityEngine;
using UnityEngine.AI;

public class StandardMelee : IAttackType
{
    MovementManager m;
    public void Attack(Stats a, Stats t, Server ser, float maxAttackSpeed, float range)
    {
        if(m == null) m = a.civ.GetComponent<MovementManager>();
        if (ser.RequestDistance(a, t) < range && a.attackSpeed == maxAttackSpeed)
        {
            m.Move(a.transform.position, a);
            t.health = ser.RequestAttack(a, t);
            a.attackSpeed = 0;
            a.sound.Play(a.sounds[2]);
        }
        Collider col = t.GetComponent<Collider>();
        Vector3 cloCol = col.ClosestPointOnBounds(a.transform.position);
        a.GetComponent<NavMeshAgent>().SetDestination(ser.RequestMovement(cloCol));
    }
}
