using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UiManager : MonoBehaviour
{
    public static UiManager instance { get; private set; }

    [SerializeField] Text titleText;
    [SerializeField] Image iconImage;

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
                // code block
                break;

            default:
                // code block
                break;
        }
    }

    public bool IsPointerOnUi()
    {
        //Set up the new Pointer Event
        var m_PointerEventData = new PointerEventData(GetComponent<EventSystem>());
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        var raycaster = GetComponent<GraphicRaycaster>();
        raycaster.Raycast(m_PointerEventData, results);

        return results.Count>0;
    }
}
