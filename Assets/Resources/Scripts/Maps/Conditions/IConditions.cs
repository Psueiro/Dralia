using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConditions
{
    void GameStarter();
    IConditions ServerSetter(Server s);
    void WinConditions();
    void LossConditions();
    string CanvasPrompt();
}
