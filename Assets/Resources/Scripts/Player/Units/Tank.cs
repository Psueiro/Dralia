using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Stats, IAttack
{
    public IAttackType st;
    public Projectile projectile;
    Server ser;

    protected override void Start()
    {
        ser = GetComponentInParent<Entity>().server;
        AttackTypeSetter(new Shooting(projectile, new Bombarding(0.5f,15)));
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
