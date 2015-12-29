using UnityEngine;
using System.Collections;

public class MoveDown : Action {

    public MoveDown(Actor actor) : base(actor) { }

    public override bool CanExecute(World world)
    {
        if (world.GetTypeOfCell(actor.PosX, actor.PosY + 1) == World.typeOfCell.obstacle)
        {
            return false;
        }
        return true;
    }

    public override void Execute(World world)
    {
        world.MoveActor(actor, 0, 1);
    }
}
