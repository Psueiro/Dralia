using UnityEngine;
public interface IProjectileMovement
{
    void Move();
    IProjectileMovement SetTarget(Vector3 t);
    IProjectileMovement SetTransform(Transform t);
    IProjectileMovement SetOrigin(Vector3 o);
    IProjectileMovement Clone();
}
