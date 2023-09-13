using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorReset : MonoBehaviour {
    public UIManager ui;
    void OnMouseDown() {
        ui.HideEditor(); //For some reason it only works from a script
    }
}
