using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {

    public int zoneID;

    PrisonManager manager;
    public GameObject[] guardSpawnLocations;
    public Transform[] itemSpawnLocations;

    void Start () {
        manager = GameObject.Find ("GameStateManager").GetComponent<PrisonManager> ();
    }

    public void ResetGuards () {
        foreach (GameObject guardSpawn in guardSpawnLocations) {
            guardSpawn.SetActive (false);
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
            Destroy (GetComponent<BoxCollider> ());
        }
    }

}