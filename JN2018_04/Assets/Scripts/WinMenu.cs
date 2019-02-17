using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour {

    void OnEnable () {
        Text textElement = GetComponent<Text> ();
        textElement.text = "Escape complete!\nEscapees: " + PlayerPrefs.GetInt ("sucessfulEscapes") + "\nTotal Escape Time: " + PlayerPrefs.GetFloat ("sucessfulTimer") + "\nScore: " + PlayerPrefs.GetInt ("sucessfulEscapes") * PlayerPrefs.GetFloat ("sucessfulTimer");

    }

}