using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Vector3 initialPosition;
    void Awake() {
        Simulator.instance.particles.Add(this);
        initialPosition = this.transform.position;
        rb = this.GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        Simulator.instance.ResetParticle(this, other.GetComponent<SpriteRenderer>().color);
    }
}
