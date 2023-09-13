using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour {
    public UIManager uiManager;
    public float rangeX, rangeY, respawnDistance, maxSquares;
    public int numberOfParticles;
    public float gConstant;
    public float maxSpeed;
    public int spawnMode, currentPosition, colorMode;
    public bool trailsVisible = true, trailsRandomColor;
    public LayerMask attractorLayer;
    public static Simulator instance;
    public GameObject particlePrefab, collisionSpawn, attractorPrefab;
    public List<Attractor> attractors;
    public List<Particle> particles;
    private List<Particle> particlesToDelete = new List<Particle>();
    private bool spawningEnded;
    void Start() {
        if(instance == null) {
            instance = this;
        }
    }
    void FixedUpdate() {
        foreach(Particle currentParticle in particles) {
            if(Vector3.SqrMagnitude(currentParticle.transform.position)>respawnDistance&&!spawningEnded) {
                ResetParticle(currentParticle, new Color(0f, 0f, 0f, 0f)); //200 IQ solution 
            }
            Vector3 finalForce = Vector3.zero;
            foreach(Attractor currentAttractor in attractors) {
                Vector3 forceVector = currentAttractor.transform.position-currentParticle.transform.position;
                if(Vector3.SqrMagnitude(forceVector)>4f)
                    forceVector = forceVector.normalized;
                else {
                    RaycastHit2D[] hits = Physics2D.CircleCastAll(currentParticle.transform.position, 10f, Vector2.zero, 10f, attractorLayer);
                    ResetParticle(currentParticle, hits[0].transform.GetComponent<SpriteRenderer>().color); //Another 200 IQ solution 
                    break;
                }
                float distance = Vector3.Distance(currentParticle.transform.position, currentAttractor.transform.position);
                forceVector *= gConstant*currentParticle.rb.mass*currentAttractor.mass/Mathf.Pow(distance, 2f);
                finalForce += forceVector;
            }
            currentParticle.rb.AddForce(finalForce);
            if(currentParticle.rb.velocity.magnitude>maxSpeed) {
                currentParticle.rb.velocity = currentParticle.rb.velocity.normalized*maxSpeed;
            }
        }
    }
    Vector3 GetPosition() {
        Vector3 position = Vector3.zero;
        if(spawnMode == 1)
            position = new Vector3(currentPosition%(2f*rangeX)-rangeX, currentPosition/(2f*rangeX)-rangeY, 0f);
        else if(spawnMode == 0)
            position = new Vector3(Random.Range(-rangeX, rangeX), Random.Range(-rangeY, rangeY), 0f);
        return position;
    }
    public void Spawn(int numberOfParticles) {
        for(int i = currentPosition; i<numberOfParticles; i++) {
        GameObject clone = Instantiate(particlePrefab);
        if(!trailsVisible)
            clone.GetComponent<TrailRenderer>().enabled = false;
        if(trailsRandomColor&&trailsVisible)
            clone.GetComponent<TrailRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        Vector3 particleStartPosition = GetPosition();
        clone.transform.position = particleStartPosition;
        Particle particle = clone.GetComponent<Particle>();
        particle.initialPosition = particleStartPosition;
        particle.rb.velocity = Vector3.zero;
        currentPosition++;
        }
    }
    public void ResetParticle(Particle particle, Color setColor) {
        if(colorMode == 1)
            setColor = new Color(setColor.r+particle.rb.velocity.x/maxSpeed, setColor.g+particle.rb.velocity.y/maxSpeed, setColor.b, 1f);
        else if(colorMode == 2)
            setColor = new Color(particle.rb.velocity.x/maxSpeed, particle.rb.velocity.y/maxSpeed, 1f, 1f);
        else if(colorMode == 3)
            setColor = setColor = new Color(Mathf.Abs(particle.rb.velocity.x/maxSpeed), Mathf.Abs(particle.rb.velocity.y/maxSpeed), 1f, 1f);
        GameObject spawnObject = Instantiate(collisionSpawn);
        spawnObject.transform.position = new Vector3(Mathf.Floor(particle.initialPosition.x), Mathf.Floor(particle.initialPosition.y), 20f);
        spawnObject.GetComponent<SpriteRenderer>().color = setColor;
        particle.transform.position = GetPosition();
        particle.initialPosition = particle.transform.position;
        particle.rb.velocity = Vector3.zero;
        currentPosition++;
        if(currentPosition>4*rangeX*rangeY&&spawnMode!=0) {
            particles.Remove(particle);
            Destroy(particle.gameObject);
            spawningEnded = true;
            uiManager.InfoMessage("Simulation ended"); //Bad way to do it but it works
        }
        if(currentPosition>maxSquares&&spawnMode==0) {
            ClearRender();
        }
    }
    public void LowerParticles() {
        float tempParticles = 0f;
        foreach(Particle currentParticle in particles) {
            if(tempParticles!=particles.Count-numberOfParticles) {
                particlesToDelete.Add(currentParticle);
                tempParticles++;
            }
        }
        foreach(Particle currentParticle in particlesToDelete) {
            particles.Remove(currentParticle);
            Destroy(currentParticle.gameObject);
        } 
        particlesToDelete.Clear();
    }
    public void AddParticles() {
        Spawn(numberOfParticles-particles.Count);
    }
    public void NewAttractor() {
        GameObject newAttractor = Instantiate(attractorPrefab);
        newAttractor.transform.position = new Vector3(0f, 0f, 0f);
        newAttractor.transform.localScale = new Vector3(8f, 8f, 2f);
    }
    public void DeleteAttractor(Attractor attractor) {
        attractors.Remove(attractor);
        Destroy(attractor.gameObject);
    }
    public void ClearRender() {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Square");
        foreach(GameObject obj in allObjects) {
            Destroy(obj);
        } //Not efficient but it works
        currentPosition = 0;
    }
    public void ResetSimulation() {
        ClearRender();
        int tempParticles = numberOfParticles;
        numberOfParticles = 0;
        LowerParticles();
        numberOfParticles = tempParticles;
    }
}
