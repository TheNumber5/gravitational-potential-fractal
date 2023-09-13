using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	public float speed;
	private Camera cam;
	void Start() {
		cam = this.GetComponent<Camera>();
	}
	void Update() {
	if(Input.GetKey(KeyCode.DownArrow)) {
		this.transform.Translate(0f, -speed*Time.deltaTime, 0f);
	}
	if(Input.GetKey(KeyCode.UpArrow)) {
		this.transform.Translate(0f, speed*Time.deltaTime, 0f);
	}
	if(Input.GetKey(KeyCode.LeftArrow)) {
		this.transform.Translate(-speed*Time.deltaTime, 0f, 0f);
	}
	if(Input.GetKey(KeyCode.RightArrow)) {
		this.transform.Translate(speed*Time.deltaTime, 0f, 0f);
	}
	if(Input.GetKey(KeyCode.Q)) {
		cam.orthographicSize -= speed*Time.deltaTime;
	}
	else if(Input.GetKey(KeyCode.W)) {
		cam.orthographicSize += speed*Time.deltaTime;
	}
	if(Input.GetKey(KeyCode.E)) {
		cam.transform.position = new Vector3(0f, 0f, -5f);
	}
	}
}
