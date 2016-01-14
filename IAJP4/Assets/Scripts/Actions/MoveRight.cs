using UnityEngine;
using System.Collections;
using WorldDefinition;

public static class MoveRight 
{
    public static bool CanExecute(Actor actor, IWorld world)
    {
        if (world.GetTypeOfCell(actor.PosX + 1, actor.PosY) == typeOfCell.obstacle || actor.Energy < 3)
        {
            return false;
        }
        return true;
    }

    public static void Execute(Actor actor, IWorld world)
    {
        world.MoveActor(actor, 1, 0);
    }
}
