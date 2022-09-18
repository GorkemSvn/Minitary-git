using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    public bool selected { get; private set; }
    public Sprite icon;
    public List<Utility> utilities { get { return GetUtilities(); }  }
    public static Selectable lastSelected;
    [SerializeField] protected Color hoverColor, selectedColor,pasiveColor;

    protected SpriteRenderer renderer;

    protected virtual void Awake()
    {
        transform.eulerAngles = Vector3.right * 90f;
        renderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Select()
    {
        lastSelected = this;
        renderer.color = selectedColor;
        selected = true;
        GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
    public virtual void Deselecet()
    {
        renderer.color = pasiveColor;
        selected = false;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

    public virtual void Hover()
    {
        if (!selected)
            renderer.color = hoverColor;
    }
    public virtual void HoverLeave()
    {
        if (!selected)
            renderer.color = pasiveColor;
    }

    public virtual void ActOn(Selectable target)
    {

    }

    public virtual void UseUtility(Utility utility)
    {

    }

    protected virtual List<Utility> GetUtilities()
    {
        return null;
    }

    public abstract class Utility:ScriptableObject
    {
        public Sprite icon;
        public UtilityAction UtilityTriggered;
        public int meatCost;
        public virtual void Trigger()
        {
            if (Requirements())
            {
                UtilityTriggered(this);
                Meat.Eat(meatCost);
            }
        }

        protected virtual bool Requirements()
        {
            return Meat.meatCount >= meatCost;
        }
        public delegate void UtilityAction(Utility utility);
    }
}
