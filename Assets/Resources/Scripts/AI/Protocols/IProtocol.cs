using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProtocol 
{
    void SetTasks(Stats s);
    IProtocol SetSources(ControllerAI a, Server s);
}
