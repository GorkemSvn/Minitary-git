using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionLine : MonoBehaviour
{
    [SerializeField] SRCountdownButton selfReportingCountdownButton;
    [SerializeField] GridLayoutGroup layout;

    public event ProductionLineSelection OnProductSelected;

    List<(string, float, float)> memo;
    RectTransform layoutRect;
    Vector2 targetPos;
    private void Awake()
    {
        layoutRect = layout.GetComponent<RectTransform>();
        targetPos = layoutRect.anchoredPosition;
    }
    public void SetButtons(List<(string,float,float)> ps)
    {
        memo = ps;
        SetChildCount(layout.transform, ps.Count, selfReportingCountdownButton.gameObject);

        for (int i = 0; i < ps.Count; i++) 
        {
            var but = layout.transform.GetChild(i).GetComponent<SRCountdownButton>();
            but.Set(ps[i].Item1, ps[i].Item2,ps[i].Item3,i==0);
            but.onClick = DeleteRequest;
            but.OnTimeOut = TimeOut;
        }
    }

    private void Update()
    {
        targetPos += Input.mouseScrollDelta*10f;
        layoutRect.anchoredPosition = Vector2.Lerp(layoutRect.anchoredPosition, targetPos,Time.deltaTime*4f);
    }


    void TimeOut(int i)
    {
        memo.RemoveAt(0);
        SetButtons(memo);
    }
    void DeleteRequest(int i)
    {
        OnProductSelected.Invoke(i);
    }

    public static void SetChildCount(Transform parent, int targetCount, GameObject prefab)
    {

        //add more buttons if necesary
        if (targetCount > parent.childCount)
        {
            for (int i = parent.childCount; i < targetCount; i++)
            {
                Instantiate(prefab, parent);
            }
        }
        //remove excess buttons
        else if (parent.childCount > targetCount)
        {
            for (int i = parent.childCount - 1; i >= targetCount; i--)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }
    }

    public delegate void ProductionLineSelection(int i);
}
