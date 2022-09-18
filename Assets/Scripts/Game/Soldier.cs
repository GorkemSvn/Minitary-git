using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Soldier : Selectable
{
    Coroutine moveCo;
    Selectable target;
    private void Start()
    {
        transform.eulerAngles = new Vector3(90, 0, 0);
        Building.OnBuildingStart +=OnBuildingStart;
    }
    public override void ActOn(Selectable target)
    {
        if(target is Tile tile && !tile.blocked)
        {
            this.target = target;
            AStarSearch.FindWay(transform.position, tile.transform.position);
            var poss = AStarSearch.path.ToArray();
            poss[0] = transform.position;

            if(moveCo!=null)
                StopCoroutine(moveCo);

            moveCo=StartCoroutine(ManualPath(poss, 0.5f));
        }

    }

    IEnumerator ManualPath(Vector3[] poses,float timeInterval)
    {
        for (int i = 1; i < poses.Length; i++)
        {
            Vector3 targetPos = poses[i];
            Vector3 startPos = transform.position;
            Tile targetTile = Tile.GetTileAtPosition(targetPos);
            if (targetTile == null || targetTile.blocked)
                break;

            for (float t = 0; t < timeInterval; t+=Time.fixedDeltaTime)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, t / timeInterval);
                yield return new WaitForFixedUpdate();
            }
        }
        moveCo = null;
    }

    void OnBuildingStart()
    {
        //
    }
    private void OnDestroy()
    {
        Building.OnBuildingStart -= OnBuildingStart;

    }

    class AStarTile
    {

        public Vector3 pos { get; private set; }
        public float originCost { get; private set; }
        public float targetCost { get; private set; }
        public float totalCost { get { return originCost + targetCost; } }
        public bool explored { get; private set; }
        public AStarTile master { get; private set; }

        Vector3 targetPos;

        public AStarTile(Vector3 targetPos,Vector3 pos,Vector3 startPos,AStarTile previos)
        {
            this.targetPos = targetPos;
            this.pos = pos;

            if(previos!=null)
            {
                master = previos;
                originCost = Vector3.Distance(previos.pos, pos) + previos.originCost;
            }
            else
                originCost = Vector3.Distance(startPos, pos);


            targetCost = Vector3.Distance(targetPos, pos);

            explored = false;
        }
        public void UpdateMaster(AStarTile newMster)
        {
            master = newMster;
            originCost = Vector3.Distance(newMster.pos, pos) + newMster.originCost;
        }
        public void Explore()
        {
            explored = true;

            ExploreTile(pos + Vector3.right);
            ExploreTile(pos + Vector3.forward);
            ExploreTile(pos + Vector3.left);
            ExploreTile(pos + Vector3.back);

            /*
            ExploreTile(pos + Vector3.right+Vector3.forward);
            ExploreTile(pos + Vector3.left+Vector3.forward);
            ExploreTile(pos + Vector3.right+Vector3.back);
            ExploreTile(pos + Vector3.left+Vector3.back);*/

        }
        void ExploreTile(Vector3 position)
        {
            position = Tiler.RoundVector(position);
            if (Tile.blockedPositions.Contains(position))
                return;

            if (!AStarSearch.analyzers.ContainsKey(position))
            {
                AStarSearch.analyzers.Add(position, new AStarTile(targetPos, position, this.pos, this));
            }
            else if(!AStarSearch.analyzers[position].explored)
                AStarSearch.analyzers[position].UpdateMaster(this);
        }

    }
    static class AStarSearch
    {
        public static Dictionary<Vector3, AStarTile> analyzers = new Dictionary<Vector3, AStarTile>();
        public static List<Vector3> path = new List<Vector3>();
        static bool pathComplete;

        public static void FindWay(Vector3 startPos, Vector3 targetPos)
        {
            analyzers.Clear();
            pathComplete = false;
            startPos = Tiler.RoundVector(startPos);
            targetPos = Tiler.RoundVector(targetPos);
            targetPos.y = startPos.y;
            AStarTile startTile = new AStarTile(targetPos, startPos, startPos, null);
            analyzers.Add(startTile.pos, startTile);
            startTile.Explore();

            for (int x = 0; x < 10000; x++)
            {
                List<AStarTile> sortedList = new List<AStarTile>();
                foreach (var item in analyzers)
                {
                    sortedList.Add(item.Value);
                    if (item.Value.pos == targetPos)
                    {
                        path.Clear();
                        MakePath(item.Value);
                        return;
                    }
                }
                sortedList.Sort((AStarTile a, AStarTile b) => a.totalCost.CompareTo(b.totalCost));
                for (int i = 0; i < sortedList.Count; i++)
                {
                    if (!sortedList[i].explored)
                    {
                        sortedList[i].Explore();
                        break;
                    }
                }
            }


        }
        static void MakePath(AStarTile tile)
        {
            path.Add(tile.pos);
            if (tile.master != null)
                MakePath(tile.master);
            else
            {
                pathComplete = true;
                path.Reverse();
            }
        }
    }
}
