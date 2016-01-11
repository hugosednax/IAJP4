using UnityEngine;
using System.Collections;
using WorldDefinition;
using System;

public abstract class Action {

    protected Actor actor;
    public int Id { protected set; get; }

    public Action(Actor actor)
    {
        this.actor = actor;
    }

    public static Action GetAction(int actionId, Actor actor)
    {
        if (actionId == 0) return new MoveDown(actor);
        if (actionId == 1) return new MoveLeft(actor);
        if (actionId == 2) return new MoveRight(actor);
        if (actionId == 3) return new MoveUp(actor);
        if (actionId == 4) return new Rest(actor);
        if (actionId == 5) return new SprintDown(actor);
        if (actionId == 6) return new SprintLeft(actor);
        if (actionId == 7) return new SprintRight(actor);
        if (actionId == 8) return new SprintUp(actor);
        else throw new Exception("Action not recognized");
    }
    public abstract bool CanExecute(World world);
    public abstract void Execute(World world);
}
