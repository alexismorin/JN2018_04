using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PrisonManager : MonoBehaviour {

    public GameObject[] zones;
    public GameObject[] spawnPoints;
    public GameObject[] keysPrefabs; // a b c d
    [Space (10)]
    string[] separatingChars = { "-" };
    public string itemCaseCode;
    public string guardsCaseCode;
    public string[] itemCaseCodeDecrypted;
    public string[] guardsCaseCodeDecrypted;
    [Space (10)]
    public int currentPlayer = 0;
    [Space (10)]
    public string[] openExits;
    [Space (10)]
    public string pendingNewCaseCode;
    public float alarmTimer = 200f;
    public bool isTimerDecreasing;
    public Color[] playerColors;
    public Text playerText;
    public Text youreupText;
    public Text wardenDialog;
    public Text escapeStartedDialog;
    public bool escapeStarted;
    public GameState stateManager;

    void Update () {

        if (alarmTimer <= 0f) {
            if (isTimerDecreasing) {
                Alarm ();
                isTimerDecreasing = false;
            }

        }

        if (isTimerDecreasing == true && alarmTimer >= 0f) {
            alarmTimer -= 1f * Time.deltaTime;
        }
    }

    void Alarm () {
        GameObject[] guards = GameObject.FindGameObjectsWithTag ("guard");
        for (int i = 0; i < guards.Length; i++) {
            guards[i].SendMessage ("Alarm", SendMessageOptions.DontRequireReceiver);
        }
    }

    void RandomizeCaseCode () { // Debug function that gets a random case code on new game - eventually we keep the last one

        GameObject[] debugZones = GameObject.FindGameObjectsWithTag ("zone");

        foreach (GameObject zone in debugZones) {
            guardsCaseCode += (zone.GetComponent<Zone> ().zoneID + "-");
        }

        itemCaseCode = guardsCaseCode;

        string[] tempDecryptedItemCaseCode = itemCaseCode.Split (separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
        string[] tempScrambledItemCaseCode = new string[tempDecryptedItemCaseCode.Length];

        for (int i = 0; i < tempDecryptedItemCaseCode.Length; i++) {
            tempScrambledItemCaseCode[i] = (zones.Length - int.Parse (tempDecryptedItemCaseCode[i]) - 1).ToString ();
        }

        itemCaseCode = "";

        foreach (string character in tempScrambledItemCaseCode) {
            itemCaseCode += (character + "-");
        }

        PlayerPrefs.SetString ("itemCaseCode", itemCaseCode);
        PlayerPrefs.SetString ("guardsCaseCode", itemCaseCode);

    }

    void GetLatestCaseCode () {
        guardsCaseCode = PlayerPrefs.GetString ("guardsCaseCode", "0-1-2-3-");
        itemCaseCode = guardsCaseCode;

        string[] tempDecryptedItemCaseCode = itemCaseCode.Split (separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
        string[] tempScrambledItemCaseCode = new string[tempDecryptedItemCaseCode.Length];

        for (int i = 0; i < tempDecryptedItemCaseCode.Length; i++) {
            tempScrambledItemCaseCode[i] = (zones.Length - int.Parse (tempDecryptedItemCaseCode[i]) - 1).ToString ();
        }

        itemCaseCode = "";

        foreach (string character in tempScrambledItemCaseCode) {
            itemCaseCode += (character + "-");
        }

        PlayerPrefs.SetString ("itemCaseCode", itemCaseCode);
        PlayerPrefs.SetString ("guardsCaseCode", guardsCaseCode);

    }

    void DecryptItemCaseCode () { // turns string into data we can use for spawners
        itemCaseCodeDecrypted = itemCaseCode.Split (separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
    }

    void DecryptGuardsCaseCode () {
        guardsCaseCodeDecrypted = guardsCaseCode.Split (separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
    }

    void Start () {
        zones = GameObject.FindGameObjectsWithTag ("zone");
        spawnPoints = GameObject.FindGameObjectsWithTag ("jailSpawnPoint");

        StartNewGame ();
    }

    void StartNewGame () {

        PlayerPrefs.SetInt ("sucessfulEscapes", 0);
        PlayerPrefs.SetFloat ("sucessfulTimer", 0);

        openExits[0] = "a";
        openExits[1] = "b";
        openExits[2] = "c";
        openExits[3] = "d";

        itemCaseCode = "";
        guardsCaseCode = "";
        pendingNewCaseCode = "";

        currentPlayer = 1;
        alarmTimer = 200f;
        //    RandomizeCaseCode (); // we should replace this with an actual fetch
        GetLatestCaseCode ();

        DecryptItemCaseCode ();
        DecryptGuardsCaseCode ();

        SpawnGuards ();
        SpawnItems ();

        MovePlayer ();
    }

    void StartNewEscape () {

        GetLatestCaseCode ();
        pendingNewCaseCode = "";

        currentPlayer++;
        alarmTimer = 200f - (currentPlayer * 18.5f);
        //    RandomizeCaseCode (); // we should replace this with an actual fetch

        DecryptItemCaseCode ();
        DecryptGuardsCaseCode ();

        SpawnGuards ();
        //   SpawnItems ();

        MovePlayer ();
    }

    public void MovePlayer () {
        GameObject playerObject = GameObject.Find ("Player");
        playerObject.GetComponent<FirstPersonController> ().enabled = false;
        playerObject.transform.GetChild (0).GetComponent<Interact> ().Void ();
        playerObject.transform.position = spawnPoints[Random.Range (0, spawnPoints.Length)].transform.position;

        playerText.color = playerColors[currentPlayer - 1];
        playerText.text = "Player " + currentPlayer.ToString ();
        playerText.enabled = true;
        youreupText.enabled = true;
        Invoke ("GivePlayerControl", 4f);
    }

    public void GivePlayerControl () {
        playerText.enabled = false;
        youreupText.enabled = false;
        GameObject playerObject = GameObject.Find ("Player");
        playerObject.GetComponent<FirstPersonController> ().enabled = true;

        wardenDialog.text = "Alright Inmates, ever since the escape of inmate " + itemCaseCode + "Alpha, we've strenghted security in cell blocks " + guardsCaseCode + "Pendleton accordingly and alarm timers have been shortened to " + alarmTimer + " seconds. The escape was the first we've had and the last we'll have, so dont try anything funny.";
        wardenDialog.enabled = true;

        Invoke ("HideWardenDialog", 12f);
    }

    public void HideWardenDialog () {
        wardenDialog.enabled = false;
    }

    public void StartEscape () {
        escapeStarted = true;
        escapeStartedDialog.enabled = true;
        Invoke ("HideStartEscapeDialog", 2f);
    }

    public void HideStartEscapeDialog () {
        escapeStartedDialog.enabled = false;
    }

    public void SpawnItems () { // forgive me justin

        GameObject[] keySpawnLocations = GameObject.FindGameObjectsWithTag ("keySpawnLocation");
        int spawnPointA = int.Parse (itemCaseCodeDecrypted[0]);
        int spawnPointB = int.Parse (itemCaseCodeDecrypted[1]);
        int spawnPointC = int.Parse (itemCaseCodeDecrypted[2]);
        int spawnPointD = int.Parse (itemCaseCodeDecrypted[3]);

        for (int i = 0; i < keySpawnLocations.Length; i++) {
            if (keySpawnLocations[i].transform.parent.GetComponent<Zone> ().zoneID == spawnPointA) {
                print (i);
                print (keySpawnLocations[i].transform.parent.GetComponent<Zone> ().zoneID + "and" + spawnPointA);

                keySpawnLocations[i].transform.parent.GetComponent<Zone> ().SpawnItemInZone (keysPrefabs[0]);
            }

            if (keySpawnLocations[i].transform.parent.GetComponent<Zone> ().zoneID == spawnPointB) {
                keySpawnLocations[i].transform.parent.GetComponent<Zone> ().SpawnItemInZone (keysPrefabs[1]);
            }

            if (keySpawnLocations[i].transform.parent.GetComponent<Zone> ().zoneID == spawnPointC) {
                keySpawnLocations[i].transform.parent.GetComponent<Zone> ().SpawnItemInZone (keysPrefabs[2]);
            }

            if (keySpawnLocations[i].transform.parent.GetComponent<Zone> ().zoneID == spawnPointD) {
                keySpawnLocations[i].transform.parent.GetComponent<Zone> ().SpawnItemInZone (keysPrefabs[3]);
            }

        }

    }

    public void SpawnGuards () {
        foreach (GameObject currentZone in zones) {
            currentZone.GetComponent<Zone> ().ResetGuards ();

            for (int i = 0; i < guardsCaseCodeDecrypted.Length; i++) {
                if (int.Parse (guardsCaseCodeDecrypted[i]) == currentZone.GetComponent<Zone> ().zoneID) { // && currentZone.GetComponent<Zone> ().zoneID% 2 == 0
                    currentZone.GetComponent<Zone> ().EnableGuards ();
                }
            }

        }
    }

    public void AddToCurrentCaseCode (int zoneCode) {
        pendingNewCaseCode += zoneCode.ToString () + "-";
    }

    public void EndEscape () {

        escapeStarted = false;
        isTimerDecreasing = false;
        PlayerPrefs.SetFloat ("lastAlarmTimer", alarmTimer);
        PlayerPrefs.SetFloat ("PlayerTime" + currentPlayer.ToString (), alarmTimer);

        if (currentPlayer == 4) {
            stateManager.Win ();

            // end game
        } else {
            PlayerPrefs.SetString ("guardsCaseCode", pendingNewCaseCode);
            StartNewEscape ();
        }
    }

}