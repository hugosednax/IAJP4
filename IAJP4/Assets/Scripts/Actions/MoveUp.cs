using UnityEngine;
using System.Collections;
using WorldDefinition;

public static class MoveUp
{
    public static bool CanExecute(Actor actor, IWorld world)
    {
        if (world.GetTypeOfCell(actor.PosX, actor.PosY - 1) == typeOfCell.obstacle || actor.Energy < 0)
        {
            return false;
        }
        return true;
    }

    public static void Execute(Actor actor, IWorld world)
    {
        world.MoveActor(actor, 0, -1);
    }
}
