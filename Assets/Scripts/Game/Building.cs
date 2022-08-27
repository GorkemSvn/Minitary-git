using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Selectable
{
    public int size = 4;
    public float buildingTime = 60f;
    [SerializeField] List<Product> producables;
    [SerializeField] Vector3 localInstantiatingPoing;
    public List<Product> products { get { return producables; } }
    public List<Product> inProduct { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        inProduct = new List<Product>();

    }
    private void Start()
    {
        Vector3 halfExtends = (Vector3.one * (size-0.5f)) / 2f;
        var cols = Physics.OverlapBox(transform.position, halfExtends, Quaternion.identity);
        foreach (var col in cols)
        {
            Tile tile = col.GetComponent<Tile>();
            if (tile != null)
                tile.SetBlockage(true);
        }
    }

    public void Produce(Product product)
    {
        if (producables.Contains(product))
        {
            StartCoroutine(ProducingProcess(product));
        }
    }

    IEnumerator ProducingProcess(Product product)
    {
        inProduct.Add(product);
        yield return new WaitForSeconds(product.producingTimeLenght);
        inProduct.Remove(product);
        for (int i = 0; i < product.quantityPerProduct; i++)
        {
            Instantiate(product.product,transform.TransformPoint(localInstantiatingPoing), Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(localInstantiatingPoing), 0.5f);
        //Gizmos.DrawCube(transform.position, Vector3.one * size);
    }
    [System.Serializable]
    public struct Product
    {
        public Selectable product;
        public float producingTimeLenght;
        public int quantityPerProduct;
    }
}
