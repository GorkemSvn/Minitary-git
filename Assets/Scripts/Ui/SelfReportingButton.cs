using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfReportingButton : MonoBehaviour
{
    public ButtonEvent onClick;
    public void RequestProduct()
    {
        onClick(transform.GetSiblingIndex());
    }

    public virtual void Set(string name)
    {
        GetComponentInChildren<Text>().text = name;
    }

    public delegate void ButtonEvent(int index);

}
