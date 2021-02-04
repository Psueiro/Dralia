using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Worker : Builders, IAttack
{
    public IAttackType st;

    protected override void Start()
    {
        ser = GetComponentInParent<Entity>().server;
        AddKeys(KeyCode.H);
        AddKeys(KeyCode.B);
        AddKeys(KeyCode.F);
        AddKeys(KeyCode.D);
        AddKeys(KeyCode.W);
        StatAssigner("Worker");
        AttackTypeSetter(new StandardMelee());
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
        at.Attack(this, s, ser,maxAttackSpeed,attackRange);
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