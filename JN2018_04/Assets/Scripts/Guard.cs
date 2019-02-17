using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class Guard : MonoBehaviour {

    public Transform[] patrolPoints;
    public NavMeshAgent agent;
    public Transform playerTransform;
    public Transform rootTransform;
    public float wanderRate = 5f;
    public float chaseRate = 2f;
    GameObject target;
    PrisonManager manager;
    bool isChasing;
    float losedistance = 20f;

    public Texture2D idleTexture;
    public Texture2D aimingTexture;
    public MeshRenderer materialRenderer;

    public void OnEnable () {
        losedistance = 20f;
        CancelInvoke ("Chase");
        InvokeRepeating ("Wander", wanderRate, wanderRate);
    }

    void Start () {
        playerTransform = GameObject.Find ("Player").transform;
        manager = GameObject.Find ("GameStateManager").GetComponent<PrisonManager> ();
        InvokeRepeating ("Wander", wanderRate, wanderRate);

    }

    void Wander () {

        materialRenderer.material.SetTexture ("_MainTex", idleTexture);

        Vector3 newDestination = patrolPoints[Random.Range (0, patrolPoints.Length)].position;
        agent.destination = newDestination;

    }

    void Chase () {

        materialRenderer.material.SetTexture ("_MainTex", aimingTexture);

        if (agent.isOnNavMesh == true) {
            agent.destination = target.transform.position;

        }

    }

    public void Alarm () {
        print (gameObject + " is in Alarm Mode!");
        target = GameObject.Find ("Player");
        losedistance = 500f;
        CancelInvoke ("Chase");
        CancelInvoke ("Wander");
        InvokeRepeating ("Chase", 0, chaseRate);

    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Player") {
            print (gameObject + " was triggered");
            target = other.gameObject;
            CancelInvoke ("Wander");
            isChasing = true;
            InvokeRepeating ("Chase", 0, chaseRate);
        }
    }

    void Update () {

        rootTransform.LookAt (playerTransform.position);

        if (target != null) {
            float dist = Vector3.Distance (target.transform.position, transform.position);

            if (dist > losedistance && isChasing == true) {
                LoseTrack ();
            }

            if (dist < 1f && isChasing == true) {
                Catch ();
            }

        }
    }

    void LoseTrack () {
        print (gameObject + " lost sight of the prisonner");
        isChasing = false;
        CancelInvoke ("Chase");
        InvokeRepeating ("Wander", wanderRate, wanderRate);

    }

    void Catch () {
        print ("Player was caught!");

        target.transform.GetChild (0).gameObject.GetComponent<Interact> ().Discard (target.transform.position);

        //Make player focus on guard that caught them.
        target.GetComponent<FirstPersonController> ().IsCaught (gameObject);

        manager.EndEscape ();
        isChasing = false;
        CancelInvoke ("Chase");
        InvokeRepeating ("Wander", wanderRate, wanderRate);
    }

}