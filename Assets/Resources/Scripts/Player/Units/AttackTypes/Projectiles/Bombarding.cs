using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombarding : IProjectileMovement
{
    Vector3 origin;
    Transform transform;
    float speed;
    Vector3 target;
    float height;
    [Range(0, 1)]
    float trajectory = 0;

    public Bombarding(float s, float h)
    {
        speed = s;
        height = h;
    }

    public void Move()
    {
        trajectory += speed * Time.deltaTime;

        Vector3 midPoint = Vector3.Lerp(origin, target, 0.5f) + new Vector3(0, height, 0);
        Vector3 pos1 = Vector3.Lerp(origin, midPoint, trajectory);
        Vector3 pos2 = Vector3.Lerp(midPoint, target, trajectory);

        transform.position = Vector3.Lerp(pos1, pos2, trajectory);
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

    public IProjectileMovement Clone()
    {
        return new Bombarding(speed, height);
    }

    public IProjectileMovement SetOrigin(Vector3 o)
    {
        origin = o;
        return this;
    }
}
