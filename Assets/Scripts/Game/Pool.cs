using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPool",menuName ="ObjectPool")]
public class Pool : ScriptableObject
{
    public int size;
    public GameObject prefab;

    Queue<GameObject> queue;
    bool instantiated = false;
    HashSet<GameObject> excluded = new HashSet<GameObject>();

    public void Instantiate()
    {
        queue = new Queue<GameObject>();
        Debug.Log("instantiated");
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            queue.Enqueue(obj);
            obj.name = prefab.name;
        }
        instantiated = true;
    }

    public GameObject Spawn(Vector3 position)
    {
        if (queue==null)
            Instantiate();

        GameObject spawn = queue.Dequeue();
        while (excluded.Contains(spawn))
            spawn = queue.Dequeue();
        spawn.SetActive(true);

        spawn.transform.position = position;

        queue.Enqueue(spawn);
        return spawn;
    }
    public GameObject Spawn(Vector3 position,Quaternion rotation)
    {
        if (!instantiated)
            Instantiate();

        GameObject spawn = queue.Dequeue();
        while (excluded.Contains(spawn))
            spawn = queue.Dequeue();
        spawn.SetActive(true);

        spawn.transform.position = position;
        spawn.transform.rotation = rotation;

        queue.Enqueue(spawn);
        return spawn;
    }
    public GameObject Spawn(Vector3 position, Quaternion rotation,Vector3 velocity)
    {
        if (!instantiated)
            Instantiate();

        GameObject spawn = queue.Dequeue();
        while (excluded.Contains(spawn))
            spawn = queue.Dequeue();

        spawn.SetActive(true);

        spawn.transform.position = position;
        spawn.transform.rotation = rotation;

        Rigidbody rb = spawn.GetComponent<Rigidbody>();
        if(rb!=null)
            rb.velocity = velocity;

        queue.Enqueue(spawn);
        return spawn;
    }

    public void Exclude(GameObject objct)
    {
        if (queue.Contains(objct) && !excluded.Contains(objct))
            excluded.Add(objct);
    }
    public void Include(GameObject objt)
    {
        if (excluded.Contains(objt))
        {
            excluded.Remove(objt);
            if (!queue.Contains(objt))
                queue.Enqueue(objt);
        }
    }

    public void Reverse()
    {
        var list = new List<GameObject>();
        list.AddRange(queue);
        list.Reverse();
        queue.Clear();
        foreach (var itm in list)
            queue.Enqueue(itm);
    }
}


