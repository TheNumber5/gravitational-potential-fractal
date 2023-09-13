using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {
    public UIManager ui;
    public float mass;
    private bool isDragging = false;
    private Vector3 offset;
    void Awake() {
        StartCoroutine(Wait());
    }
    IEnumerator Wait() {
        yield return new WaitForSeconds(0.1f);
        Simulator.instance.attractors.Add(this);
        ui = UIManager.instance;
    }
    void OnMouseDown() {
        ui.ResetAttractorEditor(this);
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
    }
    private void OnMouseUp() {
        isDragging = false;
    }
    void Update() {
        if (isDragging) {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }
    }
}
