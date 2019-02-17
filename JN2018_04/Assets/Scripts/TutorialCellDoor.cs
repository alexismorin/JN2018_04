using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCellDoor : MonoBehaviour {

    PrisonManager manager;
    Interact inventory;
    public string exitType = "e";
    public GameObject destroyTarget;

    void Start () {
        manager = GameObject.Find ("GameStateManager").GetComponent<PrisonManager> ();
        inventory = GameObject.Find ("FirstPersonCharacter").GetComponent<Interact> ();
    }

    public void Interact (string interactedItem) {
        if (interactedItem == exitType) {
            inventory.Void ();
            Destroy (gameObject);
        }
    }

}