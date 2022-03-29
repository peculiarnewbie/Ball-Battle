using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AnimationKeys", order = 1)]
public class AnimationKeys : ScriptableObject
{
    // public string animationName;
    // public List<KeyData> keyFrames = new List<KeyData>();

    // [System.Serializable]
    // public struct KeyData{
    //     public float keyTimes;
    //     public Vector3 value;
    //     /*
    //     public KeyData(int frame, float keyTimes, Vector3 value){

    //     }
    //     */
    // }

    // public bool loop;
    // [EnableIf("loop")] public float loopDelay;

    public List<AnimationComponent> components = new List<AnimationComponent>();

    public AnimationComponent defaultVaules;

    [Button(enabledMode: EButtonEnableMode.Always)]
    private void UpdateValues(){

    }
}

[System.Serializable]
public class AnimationComponent{
    // [Dropdown("AnimationType")]
    // public string animationType;

    // private List<string> AnimationType { get { return new List<string>() { "Scale", "Translate", "Rotate", "Skew" }; } }

    public AnimationTypes animType;

    public bool isRandom = false;
    [ShowIf("animType", AnimationTypes.Scale)][AllowNesting] public Vector3 ratio = new Vector3(1f,1f, 1f);
    
    [ShowIf("animType", AnimationTypes.Translate)][AllowNesting] public Vector3 relativePosition = new Vector3(0f,0f, 0f);

    [ShowIf("animType", AnimationTypes.Rotate)][AllowNesting] public Vector3 degrees = new Vector3(0f,90f, 0f);

    [ShowIf(EConditionOperator.Or, "isRandom")][AllowNesting] public Vector3 Range = new Vector3(0f,0f, 0f);

    [ShowIf("animType", AnimationTypes.Skew)][AllowNesting] public Vector2 xDegree = new Vector2(0f,20f);
    [ShowIf("animType", AnimationTypes.Skew)][AllowNesting] public Vector2 yDegree = new Vector2(0f,0f);

    public EaseTypes easeType;

    [Label("Duration (ms)")][AllowNesting] public float duration = 1000f;
    [Label("Delay (ms)")][AllowNesting] public float delay = 0f;
    public int bounces = 4;
    public int stiffness = 3;

}

public enum AnimationTypes
{
    Scale,
    Translate,
    Rotate,
    Skew
}