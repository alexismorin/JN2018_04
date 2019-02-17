using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmTrigger : MonoBehaviour {

    PrisonManager manager;
    // Start is called before the first frame update
    void Start () {
        manager = GameObject.Find ("GameStateManager").GetComponent<PrisonManager> ();
    }

    // Update is called once per frame
    void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Player") {
            manager.Alarm ();
        }
    }
}