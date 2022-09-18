using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Unit")]
public class UnitProduction : Selectable.Utility
{
    public Selectable product;
    public float producingTimeLenght;
    public int quantityPerProduct;
    [HideInInspector] public float progressTime;

    public UnitProduction Clone()
    {
        var clone = new UnitProduction();
        clone.product = product;
        clone.producingTimeLenght = producingTimeLenght;
        clone.quantityPerProduct = quantityPerProduct;
        clone.progressTime = 0;
        return clone;
    }

}