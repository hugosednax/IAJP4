using UnityEngine;
using System.Collections;
using WorldDefinition;

public class SprintDown {
    public static bool CanExecute(Actor actor, IWorld world)
    {
        for (int i = 1; i <= Actor.SPRINT_LENGTH; i++)
        {
            if (world.GetTypeOfCell(actor.PosX, actor.PosY - i) == typeOfCell.obstacle || actor.Energy < Actor.SPRINT_LENGTH * 3)
            {
                return false;
            }
        }
        return true;
    }

    public static void Execute(Actor actor, IWorld world)
    {
        world.MoveActor(actor, 0, -Actor.SPRINT_LENGTH);
    }
}