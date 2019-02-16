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
            exitType = "closed";
            PlayerPrefs.SetInt ("PlayerEscaped" + manager.currentPlayer.ToString (), 1);
            manager.EndEscape ();
        }
    }

}