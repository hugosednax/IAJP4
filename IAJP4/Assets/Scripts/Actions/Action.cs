using UnityEngine;
using System.Collections;

public abstract class Action {

    protected Actor actor;

    public Action(Actor actor)
    {
        this.actor = actor;
    }

    public abstract bool CanExecute(World world);
    public abstract void Execute(World world);
}
