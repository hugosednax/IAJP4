using UnityEngine;
using System.Collections;
using WorldDefinition;

public class MoveUp : Action {

    public MoveUp(Actor actor) : base(actor) { }

    public override bool CanExecute(World world)
    {
        if (world.GetTypeOfCell(actor.PosX, actor.PosY - 1) == typeOfCell.obstacle)
        {
            return false;
        }
        return true;
    }

    public override void Execute(World world)
    {
        world.MoveActor(actor, 0, -1);
    }
}
