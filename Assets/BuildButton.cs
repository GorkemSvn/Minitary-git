using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButton : MonoBehaviour
{
    static Structure selectedStructure;
    static InputManager.InputEvent registeredOrder;
    static bool OverLoadedInput = false;

    [SerializeField] Structure structure;
    public void RegisterBuildOrder()
    {
        selectedStructure = structure;
        if (!OverLoadedInput)
        {
            InputManager.OnSelection += Order;
            OverLoadedInput = true;
        }
    }

    static void Order(Selectable selected)
    {
        if (selected is Tile tile)
            tile.Build(selectedStructure);

        InputManager.OnSelection -= Order;
        OverLoadedInput = false;
    }
}
