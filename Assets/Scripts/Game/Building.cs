using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Selectable
{
    public static event BuildingEvent OnBuildingStart;
    [SerializeField] List<UnitProduction> units;
    [SerializeField] Vector3 localInstantiatingPoing;
    public List<UnitProduction> products { get { return units; } }
    public List<UnitProduction> inProduct { get; private set; }

    List<IEnumerator> productionLine = new List<IEnumerator>();
    Coroutine production;
    protected override void Awake()
    {
        base.Awake();
        inProduct = new List<UnitProduction>();
        transform.eulerAngles = Vector3.right * 90f;

    }
    private IEnumerator Start()
    {
        //tile blocking must be done when the building is preconstructed in editor
        yield return new WaitForEndOfFrame();
        Vector3 halfExtends = (Vector3.one * (GetComponent<BoxCollider>().size.x)) / 2f - 0.55f*Vector3.one;
        var cols = Physics.OverlapBox(transform.position, halfExtends, Quaternion.identity);
        foreach (var col in cols)
        {
            Tile tile = col.GetComponent<Tile>();
            if (tile == null)
                continue;


        }
        yield return new WaitForEndOfFrame();
        production = StartCoroutine(ProductionProcess());
    }
        IEnumerator ProductionProcess()
    {
        while (gameObject)
        {
            yield return new WaitForEndOfFrame();
            if (productionLine.Count > 0)
            {
                yield return productionLine[0];
                productionLine.RemoveAt(0);
            }
        }
    }
    public override void UseUtility(Utility utility)
    {
        if (utility.Requirements())
        {
            Meat.Eat(utility.meatCost);
            Produce(utility);
        }
    }
    public void Produce(Utility util)
    {
        var unit = util as UnitProduction;
        if (units.Contains(unit))
        {
            unit = unit.Clone();
            inProduct.Add(unit);
            productionLine.Add(ProducingProcess(unit));
        }
    }
    public void Cancel(int i)
    {
        if (i < inProduct.Count)
        {
            inProduct.RemoveAt(i);
            productionLine.RemoveAt(i);
            StopCoroutine(production);
            production = StartCoroutine(ProductionProcess());
        }
    }

    protected override List<Utility> GetUtilities()
    {
        var utils = new List<Utility>();
        utils.AddRange(units);
        return utils;
    }

    IEnumerator ProducingProcess(UnitProduction product)
    {
        OnBuildingStart?.Invoke();
        for (float t = 0; t < product.producingTimeLenght; t+=Time.fixedDeltaTime)
        {
            product.progressTime = t;
            yield return new WaitForFixedUpdate();
        }

        inProduct.Remove(product);

        for (int i = 0; i < product.quantityPerProduct; i++)
        {
            Instantiate(product.product,transform.TransformPoint(localInstantiatingPoing), Quaternion.identity);
        }
    }

    public delegate void BuildingEvent();
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(localInstantiatingPoing), 0.5f);
        //Gizmos.DrawCube(transform.position, Vector3.one * size);
    }

}
