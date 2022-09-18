using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : Selectable
{
    public static Pool tilePool;
    public static List<Vector3> blockedPositions = new List<Vector3>();
    [SerializeField] List<Structure> structures;
    public bool blocked { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        foreach (var st in structures)
            st.UtilityTriggered = Build;
    }

    private void Update()
    {
        
    }

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
        {
            tilePool.Exclude(gameObject);
            blockedPositions.Add(Tiler.RoundVector( transform.position));
        }
        else
        {
            tilePool.Include(gameObject);
            blockedPositions.Remove(Tiler.RoundVector(transform.position));
        }

    }
    public bool BuilabilityInSize(int size)
    {
        Vector3 halfExtends = (Vector3.one * (size - 0.5f)) / 2f;
        var cols = Physics.OverlapBox(transform.position + Vector3.one / 2f, halfExtends, Quaternion.identity);

        foreach (var col in cols)
        {
            Tile tile = col.GetComponent<Tile>();
            if (tile == null)
                continue;

            if (tile.blocked)
                return false;


        }
        return true;
    }

    public void Build(Utility structure)
    {
        var strct = structure as Structure;

        Vector3 halfExtends = (Vector3.one * (strct.size - 0.5f)) / 2f;
        var cols = Physics.OverlapBox(transform.position+Vector3.one/2f, halfExtends, Quaternion.identity);
        var dustes = new List<ParticleSystem>();
        foreach (var col in cols)
        {
            Tile tile = col.GetComponent<Tile>();
            if (tile == null)
                continue;

            dustes.Add(tile.GetComponentInChildren<ParticleSystem>());
            if (tile.blocked)
                return;
            else
                tile.SetBlockage(true);

        }
        StartCoroutine(BuildingProcess(dustes,strct.building, strct.buildingTime));
    }
    protected override List<Utility> GetUtilities()
    {
        var utils = new List<Utility>();
        utils.AddRange(structures);
        return utils;
    }
    public static Tile GetTileAtPosition(Vector3 position)
    {
        position.y = 0;
        var cols = Physics.OverlapBox(position, Vector3.one*0.1f, Quaternion.identity);
        foreach (var col in cols)
        {
            Tile tile = col.GetComponent<Tile>();
            if (tile != null)
                return tile;
        }
        return null;
    }
    IEnumerator BuildingProcess(List<ParticleSystem> dustes, Building building,float buildTime)
    {
        //enable dust vfx
        foreach (var dust in dustes)
            dust.Play();
        yield return new WaitForSeconds(buildTime);

        foreach (var dust in dustes)
            dust.Stop();
        //disable dust vfx, maybe trigger some finishing vfxx
        Instantiate(building, transform.position + Vector3.one / 2f, Quaternion.identity);
    }
}
