using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UiManager : MonoBehaviour
{
    public static UiManager instance { get; private set; }

    [SerializeField] Transform productsParent;
    [SerializeField] Text titleText;
    [SerializeField] Image iconImage;
    [SerializeField] SelfReportingButton productButton;
    [SerializeField] EventSystem eventSystem;

    private void Awake()
    {
        instance = this;
        InputManager.OnSelection += OnSelect;
    }

    void OnSelect(Selectable selected)
    {
        titleText.text = selected.name;
        iconImage.sprite = selected.icon;

        switch (selected)
        {
            case Tile tile:

                for (int i = productsParent.childCount - 1; i >=0; i--)
                {
                    Destroy(productsParent.GetChild(i).gameObject);
                }

                break;
            case Building building:
                HandleBuilding(building);
                // code block
                break;

            case null:
                break;

            default:
                // code block
                break;
        }
    }

    public bool IsPointerOnUi()
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

        return results.Count>0;
    }

    void HandleBuilding(Building barrack)
    {
        //transform buttons
        for (int i = 0; i < Mathf.Min(productsParent.childCount, barrack.products.Count); i++)
        {
            productsParent.GetChild(i).GetComponentInChildren<Text>().text = barrack.products[i].product.name;
        }
        //add more buttons if necesare
        if (barrack.products.Count > productsParent.childCount)
        {
            for (int i = productsParent.childCount; i < barrack.products.Count; i++)
            {
                MakeButton( barrack.products[i].product.name);
            }
        }
        //remove excess buttons
        else if (productsParent.childCount > barrack.products.Count)
        {
            for (int i = productsParent.childCount - 1; i >= barrack.products.Count; i--)
            {
                Destroy(productsParent.GetChild(i).gameObject);
            }
        }
    }

    SelfReportingButton MakeButton(string name)
    {
        var button = Instantiate(productButton, productsParent);
        button.GetComponentInChildren<Text>().text = name;
        button.onClick += GetRequest;
        return button;
    }

    void GetRequest(int index)
    {
        var building=InputManager.instance.selected as Building ;
        building.Produce(building.products[index]);
    }
}
