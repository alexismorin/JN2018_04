using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {

    public int zoneID;

    public GameObject[] guardSpawnLocations;
    public Transform[] itemSpawnLocations;

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
}