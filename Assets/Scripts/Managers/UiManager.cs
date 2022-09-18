using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UiManager : MonoBehaviour
{
    public static UiManager instance { get; private set; }

    [SerializeField] EventSystem eventSystem;
    [SerializeField] Text meatText;
    [SerializeField] ProductionLine productionLine;
    [SerializeField] UnitActions unitActionsPanel;

    private void Awake()
    {
        instance = this;
        InputManager.OnSelection += OnSelect;
        unitActionsPanel.OnActionSelected += GetUnitPRoductionRequest;
        productionLine.OnProductSelected += CancelPRoduct;
        Meat.OnMeatChange += UpdateMeat;

        UpdateMeat();
    }

    void UpdateMeat()
    {
        meatText.text = ":" + Meat.meatCount;
    }

    void OnSelect(Selectable selected)
    {
        unitActionsPanel.SetHeader(selected.name, selected.icon);

        var building = selected as Building;
        UpdateActionPanel(selected.utilities);
        UpdateProductionLine(building);

    }

    public List<RaycastResult> IsPointerOnUi()
    {
        //Set up the new Pointer Event
        var m_PointerEventData = new PointerEventData(eventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        var raycaster = GetComponent<GraphicRaycaster>();
        raycaster.Raycast(m_PointerEventData, results);

        return results;
    }

    void CancelPRoduct(int i)
    {
        var building=InputManager.instance.selected as Building;
        building.Cancel(i);
        UpdateProductionLine(building);
    }
    void GetUnitPRoductionRequest(int index)
    {
        var selected=InputManager.instance.selected;
        selected.utilities[index].Trigger();

        UpdateProductionLine(selected as Building);
    }
    void UpdateActionPanel(List<Selectable.Utility> utilities)
    {

        List<string> names = new List<string>();
        if (utilities != null)
        {
            foreach (var item in utilities)
            {
                names.Add(item.name +": "+ item.meatCost+" meat");
            }
        }
        unitActionsPanel.SetButtons(names);
    }
    void UpdateProductionLine(Building building)
    {

        //update production time
        List<(string, float, float)> infoes = new List<(string, float, float)>();

        if(building!=null)
        {
            foreach (var item in building.inProduct)
            {
                infoes.Add((item.product.name, item.progressTime, item.producingTimeLenght));

            }
        }

        productionLine.SetButtons(infoes);
    }
}
