using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hunter : Actor
{
    public Hunter(int posX, int posY)
        : base(posX, posY, Actor.typeofActor.hunter)
    {
        Actions = new List<Action>();
        Actions.Add(new MoveDown(this));
        Actions.Add(new MoveUp(this));
        Actions.Add(new MoveLeft(this));
        Actions.Add(new MoveRight(this));
        Actions.Add(new Rest(this));
        Actions.Add(new SprintDown(this));
        Actions.Add(new SprintUp(this));
        Actions.Add(new SprintLeft(this));
        Actions.Add(new SprintRight(this));
    }
}
