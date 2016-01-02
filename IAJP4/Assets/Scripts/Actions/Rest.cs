using UnityEngine;
using System.Collections;
using WorldDefinition;

public class Rest : Action {

    public Rest(Actor actor) : base(actor) { }

    public override bool CanExecute(World world)
    {
        return true;
    }

    public override void Execute(World world)
    {
        return;
    }
}
