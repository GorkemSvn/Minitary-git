using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    public bool selected { get; private set; }
    public Sprite icon;
    [SerializeField] protected Color hoverColor, selectedColor,pasiveColor;

    protected SpriteRenderer renderer;

    protected virtual void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Select()
    {
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
}
