using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviourSingleton<InputManager>
{
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void PerformTouchEvent(Vector2 position, float time);
    public event PerformTouchEvent OnPerformTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;
    private Inputs inputActions;

    private void Awake() {
        inputActions = new Inputs();
    }

    private void OnEnable() {
        inputActions.Enable();
    }

    private void OnDisable() {
        inputActions.Disable();
    }
    void Start()
    {
        inputActions.Gameplay.PlaceSoldiers.started += ctx => StartTouch(ctx);
        // inputActions.Gameplay.PositionSoldier.started += ctx => PerformTouch(ctx);
        inputActions.Gameplay.PlaceSoldiers.canceled += ctx => EndTouch(ctx);
        
    }

    // private void Update() {
    //     Debug.Log("Touch ended at " + inputActions.Gameplay.PositionSoldier.ReadValue<Vector2>());
    // }

    private void StartTouch(InputAction.CallbackContext context){
        if(OnStartTouch != null) OnStartTouch(inputActions.Gameplay.PositionSoldier.ReadValue<Vector2>(), (float)context.startTime);
    }

    // private void PerformTouch(InputAction.CallbackContext context){
    //     Debug.Log("Touch ended at " + inputActions.Gameplay.PositionSoldier.ReadValue<Vector2>());
    //     // if(OnEndTouch != null) OnEndTouch(inputActions.Gameplay.PositionSoldier.ReadValue<Vector2>(), (float)context.time);
    // }

    private void EndTouch(InputAction.CallbackContext context){
        if(OnEndTouch != null) OnEndTouch(inputActions.Gameplay.PositionSoldier.ReadValue<Vector2>(), (float)context.time);
    }

    public Vector2 GetPosition(){
        return inputActions.Gameplay.PositionSoldier.ReadValue<Vector2>();
    }

}
