using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiler : MonoBehaviour
{
    [SerializeField] Pool tilePool;
    [SerializeField] int tileRadius;

    Vector3 lastTilePos;
    private void Awake()
    {
        tilePool.Instantiate();
        Tile.tilePool = tilePool;
        TileIt();
    }

    private void Update()
    {
        if ((transform.position - lastTilePos).sqrMagnitude > 4)
            TileIt();
    }

    private void TileIt()
    {
        Vector3 mid = RoundVector(transform.position);
        lastTilePos = mid;
        mid.y = 0;

        for (int x = (int)(-tileRadius+mid.x); x < tileRadius+mid.x; x++)
        {
            for (int z = (int)(-tileRadius + mid.z); z < tileRadius + mid.z; z++)
            {
                var spawn=tilePool.Spawn(new Vector3(x, 0, z));
            }
        }
    }

    public static Vector3 RoundVector(Vector3 vector)
    {
        Vector3 newVector = vector;
        newVector.x = Mathf.Floor(vector.x);
        newVector.y = Mathf.Floor(vector.y);
        newVector.z = Mathf.Floor(vector.z);

        return newVector;
    }
}
