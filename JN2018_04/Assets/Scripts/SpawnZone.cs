using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour {

    PrisonManager manager;
    public Transform dropLocation;

    void Start () {
        manager = GameObject.Find ("GameStateManager").GetComponent<PrisonManager> ();
    }

    void OnTriggerExit (Collider other) {
        if (other.gameObject.tag == "Player" && manager.escapeStarted == false) {
            manager.StartEscape ();
            manager.isTimerDecreasing = true;
        }
    }
}