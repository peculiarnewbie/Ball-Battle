using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void MovetoNormalScne(){
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void MoveToARScene(){
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
