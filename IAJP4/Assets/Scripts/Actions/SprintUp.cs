using UnityEngine;
using System.Collections;
using WorldDefinition;

public class SprintUp {
    public static bool CanExecute(Actor actor, World world)
    {
        for (int i = 1; i <= World.SPRINT_LENGTH; i++)
        {
            if (world.GetTypeOfCell(actor.PosX, actor.PosY - i) == typeOfCell.obstacle || actor.Energy < 0)
            {
                return false;
            }
        }
        return true;
    }

    public static void Execute(Actor actor, World world)
    {
        world.MoveActor(actor, 0, -World.SPRINT_LENGTH);
    }
}