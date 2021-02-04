using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightProjectile : IProjectileMovement
{
    Vector3 origin;
    Vector3 target;
    Transform transform;
    float speed;
    float timer;
    public StraightProjectile(float s, float t)
    {
        speed = s;
        timer = t;
    }

    public IProjectileMovement Clone()
    {
        return new StraightProjectile(speed, timer);
    }

    public void Move()
    {
        transform.position += (target - origin).normalized * speed * Time.deltaTime;
        if (timer <= 0) MonoBehaviour.Destroy(transform.gameObject); else timer -= 1 * Time.deltaTime; 
    }

    public IProjectileMovement SetOrigin(Vector3 o)
    {
        origin = o;
        return this;
    }

    public IProjectileMovement SetTarget(Vector3 t)
    {
        target = t;
        return this;
    }

    public IProjectileMovement SetTransform(Transform t)
    {
        transform = t;
        return this;
    }
}
