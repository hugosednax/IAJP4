using UnityEngine;
using System.Collections;
using WorldDefinition;

public class MoveDown : Action {

    public MoveDown(Actor actor) : base(actor) { Id = 0; }

    public override bool CanExecute(World world)
    {
        if (world.GetTypeOfCell(actor.PosX, actor.PosY + 1) == typeOfCell.obstacle && actor.Energy > 0)
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
