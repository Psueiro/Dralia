using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : Builders, IAttack
{
    public IAttackType st;
    public Projectile projectile;

    protected override void Start()
    {
        ser = GetComponentInParent<Entity>().server;
        AddKeys(KeyCode.Z);
        AddKeys(KeyCode.C);
        AddKeys(KeyCode.A);
        AddKeys(KeyCode.G);
        AddKeys(KeyCode.F);
        StatAssigner("Summoner");
        AttackTypeSetter(new Shooting(projectile, new StraightProjectile(4, 7)));
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
        ser.anim.AnimationChange(this, ser.anim.allAnimationNames[1]);
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
