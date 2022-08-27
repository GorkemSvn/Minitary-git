using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event InputEvent OnTileSelected;
    public static event InputEvent OnTileHover;

    public Selectable selected { get; private set; }

    Camera cam;
    Selectable lastSelected;

    Vector3 rightClickPos,camRCP;

    private void Awake()
    {
        cam = Camera.main;
    }
    void Update()
    {
        Selectable selectable = RayCatch();
        if (selectable != null && selectable!=lastSelected)
        {
            lastSelected?.HoverLeave();
            lastSelected = selectable;

            selectable.Hover();
            OnTileHover?.Invoke(selectable);
        }

        if (Input.GetMouseButtonDown(0)&&selectable!=null)
        {
            selected?.Deselecet();
            selected = selectable;
            selectable.Select();
            OnTileSelected?.Invoke(selectable);
        }

        if (Input.GetMouseButtonDown(1))
        {
            rightClickPos = Input.mousePosition;
            camRCP = cam.transform.position;
        }
        if (Input.GetMouseButton(1))
        {
            cam.transform.position = TransformVerticalToHorizontal(rightClickPos - Input.mousePosition) *0.01f + camRCP;
        }
    }

    Vector3 TransformVerticalToHorizontal(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }
    Selectable RayCatch()
    {
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 20f))
            return hit.collider.GetComponent<Selectable>();
        else
            return null;
    }

    public delegate void InputEvent(Selectable tile);
}
