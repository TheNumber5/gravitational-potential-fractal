using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance;
    public TMP_Text info;
    public TMP_InputField particleInput, rangeInputX, rangeInputY, maxSpeedInput, massInput;
    public TMP_Dropdown colorModeDropdown, spawnModeDropdown;
    public Slider RSlider, GSlider, BSlider;
    public Toggle trailToggle, randomColorToggle;
    public GameObject options, editor, uiTip, mainCanvas;
    public Simulator sim;
    private bool messageSent;
    [HideInInspector]
    public bool editingAttractor;
    [HideInInspector]
    public Attractor currentAttractor;
    private bool isFullscreen, uiHiddenOnce;
    void Start() {
        if(instance == null) {
            instance = this;
        }
    }
    public void ChangeParticles() {
        sim.numberOfParticles = int.Parse(particleInput.text);
    }
    public void ChangeRange() {
        sim.rangeX = int.Parse(rangeInputX.text);
        sim.rangeY = int.Parse(rangeInputY.text);
    }
    public void ShowHideOptions() {
        options.SetActive(!options.activeSelf);
    }
    public void ChangeMaxSpeed() {
        sim.maxSpeed = float.Parse(maxSpeedInput.text);
    }
    public void ChangeColorMode() {
        sim.colorMode = colorModeDropdown.value;
    }
    public void ChangeSpawnMode() {
        sim.spawnMode = spawnModeDropdown.value;
    }
    public void StartSimulation() {
        sim.ClearRender();
        sim.numberOfParticles = 0;
        sim.LowerParticles();
        ChangeParticles();
        sim.Spawn(sim.numberOfParticles);
        messageSent = false;
        InfoMessage("");
        messageSent = false;
    }
    public void ChangeTrails() {
        sim.trailsVisible = trailToggle.isOn;
    }
    public void ChangeTrailsRandomColor() {
        sim.trailsRandomColor = randomColorToggle.isOn;
    }
    public void ChangeAttractor() {
        currentAttractor.GetComponent<SpriteRenderer>().color = new Color(RSlider.value, GSlider.value, BSlider.value);
        currentAttractor.mass = float.Parse(massInput.text);
    }
    public void ResetAttractorEditor(Attractor attractor) {
        editor.SetActive(true);
        currentAttractor = attractor;
        Color attractorColor = attractor.gameObject.GetComponent<SpriteRenderer>().color;
        RSlider.value = attractorColor.r;
        GSlider.value = attractorColor.g;
        BSlider.value = attractorColor.b;
        massInput.text = attractor.mass.ToString("0.0");
    }
    public void HideEditor() {
        editor.SetActive(false);
    }
    public void InfoMessage(string infoMessage) {
        if(!messageSent)
            StartCoroutine(DisplayMessage(infoMessage));
    }
    IEnumerator DisplayMessage(string infoMessage) {
        messageSent = true;
        info.text = infoMessage;
        yield return new WaitForSeconds(500f);
        info.text = "";
        messageSent = false;
    }
    public void NewAttractor() {
        sim.NewAttractor();
    }
    public void DeleteAttractor() {
        sim.DeleteAttractor(currentAttractor);
        HideEditor();
    }
    void Update() {
        if(!messageSent)
            info.text = (1f/Time.smoothDeltaTime).ToString("0.0") + " FPS";
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(!uiHiddenOnce) {
                Destroy(uiTip);
                uiHiddenOnce = true;
                mainCanvas.SetActive(!mainCanvas.activeSelf);
            }
            else {
                mainCanvas.SetActive(!mainCanvas.activeSelf);
            }   
        }
    }
    public void ChangeFullscreen() {
        Application.targetFrameRate = 0;
        Debug.Log("Fullscreen mode changed");
        if(!isFullscreen)
            Screen.SetResolution(1920, 1080, true);
        else
            Screen.SetResolution(1280, 720, false);
        isFullscreen = !isFullscreen;
    }
}
