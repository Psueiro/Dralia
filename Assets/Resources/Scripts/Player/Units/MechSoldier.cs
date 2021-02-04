using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechSoldier : Stats, IAttack
{
    IAttackType st;
    Server ser;

    protected override void Start()
    {
        ser = GetComponentInParent<Entity>().server;
        AttackTypeSetter(new InstantShot());
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
        Debug.Log(attackRange);
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
