using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackType 
{
    void Attack(Stats a, Stats t, Server ser, float maxAttackSpeed, float range);
}
