using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : Stats, IAttack
{
    public IAttackType st;
    public Projectile projectile;
    Server ser;

    protected override void Start()
    {
        ser = GetComponentInParent<Entity>().server;
        AttackTypeSetter(new Shooting(projectile,new StraightProjectile(7, 5)));
        base.Start();
    }

    protected override void Update()
    {
        attackSpeed = ser.AttackSpeed(attackSpeed, maxAttackSpeed);
        if (target) AttackMethod(target, st);
        base.Update();
    }

    public void AttackMethod(Stats s, IAttackType at)
    {
        at.Attack(this, s, ser, maxAttackSpeed, attackRange);
    }

    public void AttackTypeSetter(IAttackType at)
    {
        st = at;
    }

    public void TargetSetter(Stats s)
    {
        target = s;
    }
}
