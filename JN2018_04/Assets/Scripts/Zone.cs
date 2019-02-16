using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zone : MonoBehaviour {

    public int zoneID;

    PrisonManager manager;
    public GameObject[] guardSpawnLocations;
    public Transform[] itemSpawnLocations;

    void Start () {
        manager = GameObject.Find ("GameStateManager").GetComponent<PrisonManager> ();
    }

    public void ResetGuards () {

        ResetTriggers ();

        foreach (GameObject guardSpawn in guardSpawnLocations) {

            var currentAgent = guardSpawn.GetComponent<NavMeshAgent> ();

            if (currentAgent.isOnNavMesh == true) {
                currentAgent.ResetPath ();

            }

            guardSpawn.SetActive (false);
        }
    }

    public void ResetTriggers () {
        Component[] colliders;
        colliders = gameObject.GetComponents (typeof (BoxCollider));
        for (int i = 0; i < colliders.Length; i++) {
            BoxCollider collider = colliders[i] as BoxCollider;
            collider.enabled = true;
        }
    }

    public void EnableGuards () {
        foreach (GameObject guardSpawn in guardSpawnLocations) {
            guardSpawn.SetActive (true);
        }

    }

    public void SpawnItemInZone (GameObject itemToSpawn) {
        Instantiate (itemToSpawn, itemSpawnLocations[Random.Range (0, itemSpawnLocations.Length)].position, Quaternion.identity);
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Player") {
            manager.AddToCurrentCaseCode (zoneID);

            Component[] colliders;
            colliders = gameObject.GetComponents (typeof (BoxCollider));
            for (int i = 0; i < colliders.Length; i++) {
                BoxCollider collider = colliders[i] as BoxCollider;
                collider.enabled = false;
            }
        }
    }

}