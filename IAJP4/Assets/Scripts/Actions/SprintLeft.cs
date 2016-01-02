using UnityEngine;
using System.Collections;
using WorldDefinition;

public class SprintLeft : Action {

    public SprintLeft(Actor actor) : base(actor) { }

    public override bool CanExecute(World world)
    {
        for (int i = 1; i <= World.SPRINT_LENGTH; i++)
        {
            if (world.GetTypeOfCell(actor.PosX - i, actor.PosY) == typeOfCell.obstacle)
            {
                return false;
            }
        }
        return true;
    }

    public override void Execute(World world)
    {
        world.MoveActor(actor, -World.SPRINT_LENGTH, 0);
    }
}
