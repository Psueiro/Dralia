using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
public class MovementManager : MonoBehaviour
{
    Server _ser;
    public List<Stats> allMovingUnits = new List<Stats>();
    SoundManager sound;

    public MovementManager ServerSetter(Server s)
    {
        _ser = s;
        return this;
    }

    private void LateUpdate()
    {
        Stop();
    }

    public void Move(Vector3 v, Stats unit)
    {
        unit.GetComponent<NavMeshAgent>().SetDestination(_ser.RequestMovement(v));

        //if(unit.civ.id == 0)
        //{
        //    if (sound == null) sound = Camera.main.GetComponent<SoundManager>();
        //    float f = Random.Range(0, 100);
        //    if(f < 50f && unit.transform.position != v)
        //    {
        //        sound.Play(unit.sounds[1]);
        //    }
        //}
        _ser.anim.AnimationChange(unit, _ser.anim.allAnimationNames[0]);
        if (!allMovingUnits.Contains(unit)) allMovingUnits.Add(unit);
    }

    void Stop()
    {
        for (int i = 0; i < allMovingUnits.Count; i++)
        {
           NavMeshAgent nav = allMovingUnits[i].GetComponent<NavMeshAgent>();
            float dist = Vector3.Distance(allMovingUnits[i].transform.position, nav.destination);
            if( dist < 0.2f)
            {
                _ser.anim.DisableAllAnims(allMovingUnits[i]);
                allMovingUnits.Remove(allMovingUnits[i]);
            }
        }
    }
}
