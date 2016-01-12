using UnityEngine;
using System.Collections;
using WorldDefinition;

public static class Rest
{
    public static bool CanExecute(Actor actor, World world)
    {
        return true;
    }

    public static void Execute(Actor actor, World world)
    {
        return;
    }
}