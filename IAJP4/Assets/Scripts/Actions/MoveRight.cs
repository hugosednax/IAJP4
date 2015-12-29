using UnityEngine;
using System.Collections;

public class MoveRight : Action {

    public MoveRight(Actor actor) : base(actor) { }

    public override bool CanExecute(World world)
    {
        if (world.GetTypeOfCell(actor.PosX + 1, actor.PosY) == World.typeOfCell.obstacle)
        {
            return false;
        }
        return true;
    }

    public override void Execute(World world)
    {
        world.MoveActor(actor, 1, 0);
    }
}
