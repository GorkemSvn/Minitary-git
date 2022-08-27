using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfReportingButton : MonoBehaviour
{
    public event ButtonEvent onClick;
    public void RequestProduct()
    {
        onClick(transform.GetSiblingIndex());
    }

    public delegate void ButtonEvent(int index);
}
