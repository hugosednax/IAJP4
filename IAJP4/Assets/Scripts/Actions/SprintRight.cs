using UnityEngine;
using System.Collections;
using WorldDefinition;

public static class SprintRight
{
    public static bool CanExecute(Actor actor, World world)
    {
        for (int i = 1; i <= World.SPRINT_LENGTH; i++)
        {
            if (world.GetTypeOfCell(actor.PosX + i, actor.PosY) == typeOfCell.obstacle || actor.Energy < 0)
            {
                return false;
            }
        }
        return true;
    }

    public static void Execute(Actor actor, World world)
    {
        world.MoveActor(actor, World.SPRINT_LENGTH, 0);
    }
}