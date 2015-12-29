using UnityEngine;
using System.Collections;

public class Prey : Actor {

    public Prey(int posX, int posY)
        : base(posX, posY, Actor.typeofActor.fugitive)
    {

    }
}
