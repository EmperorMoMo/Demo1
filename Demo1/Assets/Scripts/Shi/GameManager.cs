using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public void OnStarGame(string ScenesName) {
        SceneManager.LoadScene(ScenesName);
    }
}
