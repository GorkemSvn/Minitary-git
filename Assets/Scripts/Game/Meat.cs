using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Meat : Selectable
{
    public static int meatCount { get; private set; } = 100;
    public static event MeatEvent OnMeatChange;
    protected override void Awake()
    {
        base.Awake();
        transform.DOScale(0f, 1f).OnComplete(() =>
        {
            Destroy(gameObject);
            meatCount++;
            OnMeatChange?.Invoke();
        });
    }

    public static void Eat(int x)
    {
        meatCount = Mathf.Max(0, meatCount - Mathf.Abs(x));
        OnMeatChange?.Invoke();
    }
    public delegate void MeatEvent();
}
