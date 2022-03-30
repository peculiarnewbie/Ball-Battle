using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public bool isInAR = false;
    public bool isFirstOpened = true;
    ARPlane arplane;
    ARPlaneManager planeManager;

    private void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() {
        if(Permission.HasUserAuthorizedPermission(Permission.Camera) && !isInAR && isFirstOpened){
            StartCoroutine(StartMoveToAR());
            isFirstOpened = false;
        }
    }

    private IEnumerator StartMoveToAR(){
        yield return new WaitForSeconds(0.1f);
        MoveToARScene();
    }

    public void MovetoNormalScne(){
        isInAR = false;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        RestartMatch();
    }

    public void MoveToARScene(){
        SceneManager.LoadScene(2, LoadSceneMode.Single);
        arplane = GameObject.FindObjectOfType<ARPlane>();
        planeManager = GameObject.FindObjectOfType<ARPlaneManager>();
        // planeManager. += StopSpawn;
    }

    private void StopSpawn() {
        arplane.gameObject.SetActive(false);
    }

    private void RestartMatch(){
        MatchManager.Instance.Start();
    }
}
