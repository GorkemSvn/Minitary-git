using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : Selectable
{
    public static Pool tilePool;



    public override void Select()
    {
        base.Select();
        tilePool?.Exclude(gameObject);
    }

    public virtual void Deselecet()
    {
        base.Deselecet();
        tilePool?.Include(gameObject);
    }

}
