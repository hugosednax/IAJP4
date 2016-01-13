using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldDefinition;

public interface IWorld
{
    void killPrey();
    byte[] getState(Actor actor);
    Hunter Hunter { get; set; }
    Prey Prey { get; set; }
    void MoveActor(Actor actor, int offsetX, int offsetY);
    typeOfCell GetTypeOfCell(int x, int y);
}

