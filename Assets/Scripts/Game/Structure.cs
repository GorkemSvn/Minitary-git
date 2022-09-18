using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Structure")]
public class Structure : Selectable.Utility
{
    public Building building;
    public int size = 4;
    public float buildingTime = 60f;

    protected override bool Requirements()
    {
        if (Selectable.lastSelected != null && Selectable.lastSelected is Tile tile)
            return tile.BuilabilityInSize(size) && base.Requirements();

        return false;
    }
}
