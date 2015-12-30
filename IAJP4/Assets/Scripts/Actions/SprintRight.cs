using UnityEngine;
using System.Collections;

public class SprintRight : Action {

    public SprintRight(Actor actor) : base(actor) { }

    public override bool CanExecute(World world)
    {
        for (int i = 0; i < World.SPRINT_LENGTH; i++)
        {
            if (world.GetTypeOfCell(actor.PosX + i, actor.PosY) == World.typeOfCell.obstacle)
            {
                return false;
            }
        }
        return true;
    }

    public override void Execute(World world)
    {
        world.MoveActor(actor, World.SPRINT_LENGTH, 0);
    }
}
