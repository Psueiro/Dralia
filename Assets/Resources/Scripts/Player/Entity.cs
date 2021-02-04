using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class Entity : MonoBehaviour
{
    protected event Action OnUpdate = delegate { };
    public IController controller;
    public Server server;
    public int id;
    public int faction;
    public int team;
    public Color color;

    public float crystals;
    public float minerals;

    protected virtual void Awake()
    {
        server = FindObjectOfType<Server>();
        OnConnectedServer();
    }

    public void ExecuteUpdate()
    {
        OnUpdate();
        crystals = server.allCrystals[id];
        minerals = server.allMinerals[id];
    }

    public void OnConnectedServer()
    {
        server.AddPlayer(this);
    }

    public void OnDisconnectedServer()
    {
        server.RemovePlayer(this);
    }

    public void SetController (IController c)
    {
        controller = c;
        OnUpdate += controller.SetKeys;
    }
}
