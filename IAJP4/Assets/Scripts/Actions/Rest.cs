using UnityEngine;
using System.Collections;
using WorldDefinition;

public static class Rest
{
    public static bool CanExecute(Actor actor, IWorld world)
    {
        if (actor.Energy < 1) return false;
        return true;
    }

    public static void Execute(Actor actor, IWorld world)
    {
        actor.Energy -= 1;
        return;
    }
}