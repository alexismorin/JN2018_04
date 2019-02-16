using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

    public string exitType = "a";

    public void Interact (string interactedItem) {
        if (interactedItem == exitType) {
            print ("exit");
        }
    }

}