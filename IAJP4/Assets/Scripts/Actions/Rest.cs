using UnityEngine;
using System.Collections;
using WorldDefinition;

public static class Rest
{
    public static bool CanExecute(Actor actor, IWorld world)
    {
        return true;
    }

    public static void Execute(Actor actor, IWorld world)
    {
        return;
    }
}