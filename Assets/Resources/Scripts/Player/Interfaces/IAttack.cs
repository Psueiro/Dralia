public interface IAttack 
{
    void TargetSetter(Stats s);
    void AttackTypeSetter(IAttackType at);
    void AttackMethod(Stats s, IAttackType at);
}
