using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public void OnStarGame(string ScenesName) {
        Application.LoadLevel(ScenesName);
    }
}
