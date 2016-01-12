using UnityEngine;
using System.Collections;
using WorldDefinition;
using System;

public static class ActionManager {

    public static bool CanExecute(int actionId, Actor actor, World world)
    {
        if (actionId == 0) return MoveDown.CanExecute(actor, world);
        if (actionId == 1) return MoveLeft.CanExecute(actor, world);
        if (actionId == 2) return MoveRight.CanExecute(actor, world);
        if (actionId == 3) return MoveUp.CanExecute(actor, world);
        if (actionId == 4) return Rest.CanExecute(actor, world);
        if (actionId == 5) return SprintDown.CanExecute(actor, world);
        if (actionId == 6) return SprintLeft.CanExecute(actor, world);
        if (actionId == 7) return SprintRight.CanExecute(actor, world);
        if (actionId == 8) return SprintUp.CanExecute(actor, world);
        else throw new Exception("Action not recognized");
    }

    public static void Execute(int actionId, Actor actor, World world)
    {
        if (actionId == 0) MoveDown.Execute(actor, world);
        else if (actionId == 1) MoveLeft.Execute(actor, world);
        else if (actionId == 2) MoveRight.Execute(actor, world);
        else if (actionId == 3) MoveUp.Execute(actor, world);
        else if (actionId == 4) Rest.Execute(actor, world);
        else if (actionId == 5) SprintDown.Execute(actor, world);
        else if (actionId == 6) SprintLeft.Execute(actor, world);
        else if (actionId == 7) SprintRight.Execute(actor, world);
        else if (actionId == 8) SprintUp.Execute(actor, world);
        else throw new Exception("Action not recognized: " + actionId);
    }
}
