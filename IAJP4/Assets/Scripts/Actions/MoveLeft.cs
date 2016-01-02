using UnityEngine;
using System.Collections;
using WorldDefinition;

public class MoveLeft : Action {

    public MoveLeft(Actor actor) : base(actor) { }

    public override bool CanExecute(World world)
    {
        if (world.GetTypeOfCell(actor.PosX - 1, actor.PosY) == typeOfCell.obstacle)
        {
            return false;
        }
        return true;
    }

    public override void Execute(World world)
    {
        world.MoveActor(actor, -1, 0);
    }
}
