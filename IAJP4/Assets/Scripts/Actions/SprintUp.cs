using UnityEngine;
using System.Collections;

public class SprintUp : Action {

    public SprintUp(Actor actor) : base(actor) { }

    public override bool CanExecute(World world)
    {
        for (int i = 1; i <= World.SPRINT_LENGTH; i++)
        {
            if (world.GetTypeOfCell(actor.PosX, actor.PosY - i) == World.typeOfCell.obstacle)
            {
                return false;
            }
        }
        return true;
    }

    public override void Execute(World world)
    {
        world.MoveActor(actor, 0, -World.SPRINT_LENGTH);
    }
}
