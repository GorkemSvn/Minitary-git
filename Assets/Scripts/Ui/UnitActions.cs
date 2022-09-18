using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActions : MonoBehaviour
{
    [SerializeField] Text titleText;
    [SerializeField] Image iconImage;
    [SerializeField] SelfReportingButton sRButtonPrefab;
    [SerializeField] Transform layout;
    public event UnitActionSelection OnActionSelected;

    public void SetHeader(string title, Sprite icon)
    {
        titleText.text = title;
        iconImage.sprite = icon;

    }
    public void SetButtons(List<string> buttonNames)
    {
        ProductionLine.SetChildCount(layout, buttonNames.Count, sRButtonPrefab.gameObject);

        for (int i = 0; i < buttonNames.Count; i++)
        {
            var but = layout.GetChild(i).GetComponent<SelfReportingButton>();
            but.Set(buttonNames[i]);
            but.onClick = ActionSelect;
        }
    }
    void ActionSelect(int index)
    {
        OnActionSelected(index);
    }

    public delegate void UnitActionSelection(int i);
}
