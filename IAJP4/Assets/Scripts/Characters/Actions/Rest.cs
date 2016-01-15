using UnityEngine;
using System.Collections;
using WorldDefinition;

public static class Rest
{
    public static bool CanExecute(Actor actor, World world)
    {
        if (actor.Energy < 1) return false;
        return true;
    }

    public static void Execute(Actor actor, World world)
    {
        actor.Energy -= 1;
        return;
    }
}