using UnityEngine;
using System.Collections;
using WorldDefinition;

public class SprintDown : Action {

    public SprintDown(Actor actor) : base(actor) { }

    public override bool CanExecute(World world)
    {
        for (int i = 1; i <= World.SPRINT_LENGTH; i++)
        {
            if (world.GetTypeOfCell(actor.PosX, actor.PosY + i) == typeOfCell.obstacle)
            {
                return false;
            }
        }
        return true;
    }

    public override void Execute(World world)
    {
        world.MoveActor(actor, 0, World.SPRINT_LENGTH);
    }
}
