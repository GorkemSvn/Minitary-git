using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : Selectable
{
    public static Pool tilePool;

    public bool blocked=false;

    public override void Select()
    {
        if (blocked)
            return;
        base.Select();
        tilePool?.Exclude(gameObject);
    }

    public virtual void Deselecet()
    {
        base.Deselecet();
        tilePool?.Include(gameObject);
    }

    public void SetBlockage(bool blocked)
    {
        this.blocked = blocked;
        if (blocked)
            tilePool.Exclude(gameObject);
        else
            tilePool.Include(gameObject);
    }

    public void Build(Building building)
    {
        StartCoroutine(BuildingProcess(building));
    }

    IEnumerator BuildingProcess(Building building)
    {
        //enable dust vfx
        yield return new WaitForSeconds(building.buildingTime);
        //disable dust vfx, maybe trigger some finishing vfxx
        Instantiate(building, transform.position + Vector3.one / 2f, Quaternion.identity);
    }
}
