﻿using UnityEngine;
using System.Collections;
using WorldDefinition;

public static class MoveDown
{
    public static bool CanExecute(Actor actor, World world)
    {
        if (world.GetTypeOfCell(actor.PosX, actor.PosY - 1) == typeOfCell.obstacle || actor.Energy < 3)
        {
            return false;
        }
        return true;
    }

    public static void Execute(Actor actor, World world)
    {
        world.MoveActor(actor, 0, -1);
    }
}
