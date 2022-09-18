using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InfiniteScrollView : MonoBehaviour
{
    [SerializeField] RectTransform grid;
    [SerializeField] Pool imagePool;

    [SerializeField] float spacing,cellSize;
    float gridHeight;
    float nodeDelta;
    Vector2 upperSpawnPoint, lowerSpawnPoint;
    Vector2 gridUpperTreshold, gridLowerTreshold;

    Vector2 gridTargetPos;
    Vector2 localUpperEdge;
    bool directionUp = true;

    List<Transform> linedUpTrans = new List<Transform>();

    private void Awake()
    {
        //setting up variables
        nodeDelta = spacing + cellSize;
        gridHeight = grid.rect.height;

        gridTargetPos = grid.anchoredPosition;
        int gridCapacity = (int)(gridHeight / nodeDelta);

        upperSpawnPoint = transform.TransformPoint(Vector2.up * gridHeight / 2f + Vector2.up * nodeDelta);
        lowerSpawnPoint = upperSpawnPoint+Vector2.down*(gridCapacity+2)*nodeDelta;

        gridUpperTreshold = (Vector2)grid.transform.position + Vector2.up * nodeDelta;
        gridLowerTreshold = (Vector2)grid.transform.position - Vector2.up * nodeDelta;

        //first instantiation
        localUpperEdge = Vector2.up * gridHeight / 2f;

        imagePool.size = gridCapacity + 3;
        imagePool.Instantiate();
        for (int i = -1; i < gridCapacity+2; i++)
        {
            var image=imagePool.Spawn(Vector3.zero);
            image.transform.parent = grid;
            image.transform.localPosition = Vector2.down * i * (cellSize + spacing)+localUpperEdge;
            linedUpTrans.Add(image.transform);
        }
        enabled = false;
    }

    private void OnEnable()
    {
        gridTargetPos = grid.anchoredPosition;
    }

    private void Update()
    {
        gridTargetPos += Input.mouseScrollDelta * 30f;

        grid.anchoredPosition = Vector2.Lerp(grid.anchoredPosition, gridTargetPos, Time.deltaTime * 4f);

        if ( grid.position.y > gridUpperTreshold.y)
        {
            if (directionUp != true)
            {
                directionUp = true;
                imagePool.Reverse();
            }

            imagePool.Spawn(lowerSpawnPoint);
            gridUpperTreshold += Vector2.up * nodeDelta;
            gridLowerTreshold += Vector2.up * nodeDelta;
        }
        else if (grid.position.y < gridLowerTreshold.y)
        {
            if (directionUp != false)
            {
                directionUp = false;
                imagePool.Reverse();
            }

            imagePool.Spawn(upperSpawnPoint);
            gridUpperTreshold += Vector2.down * nodeDelta;
            gridLowerTreshold += Vector2.down * nodeDelta;
        }
    }


}
