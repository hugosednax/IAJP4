using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Prey : Actor {

    public Prey(int posX, int posY)
        : base(posX, posY, Actor.typeofActor.prey)
    {
        Actions = new List<Action>();
        Actions.Add(new MoveDown(this));
        Actions.Add(new MoveUp(this));
        Actions.Add(new MoveLeft(this));
        Actions.Add(new MoveRight(this));
        Actions.Add(new Rest(this));
    }
}
