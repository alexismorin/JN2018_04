using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

    PrisonManager manager;
    public string exitType = "a";

    void Start () {
        manager = GameObject.Find ("GameStateManager").GetComponent<PrisonManager> ();
    }

    public void Interact (string interactedItem) {
        if (interactedItem == exitType) {

            int hostA = PlayerPrefs.GetInt ("sucessfulEscapes");
            float hostB = PlayerPrefs.GetInt ("sucessfulTimer");

            hostA += 1;
            hostB += manager.alarmTimer;

            PlayerPrefs.SetInt ("sucessfulEscapes", hostA);
            PlayerPrefs.SetFloat ("sucessfulTimer", hostB);

            exitType = "closed";
            PlayerPrefs.SetInt ("PlayerEscaped" + manager.currentPlayer.ToString (), 1);
            print ("Player escaped!");
            manager.EndEscape ();
        }
    }

}