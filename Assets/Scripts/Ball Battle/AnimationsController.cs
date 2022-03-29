using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsController : MonoBehaviour
{
    public List<Animations> animationList = new List<Animations>();

    // Start is called before the first frame update
    void Start()
    {
        PlayStartingAnimations();
    }

    public void PlayAnimation(string animationName){
        UseAnimationKeys animationToPlay = null;
        foreach(Animations animation in animationList){
            if(animation.customName == animationName) animationToPlay = animation.animationKey;
        }
        if(animationToPlay != null) animationToPlay.PlayAnimation();
    }

    private void PlayStartingAnimations(){
        foreach(Animations animation in animationList){
            Debug.Log("trying to animate");
            if (animation.playAtStart) animation.animationKey.PlayAnimation();
        }
    }

    [System.Serializable]
    public struct Animations {
        public string customName;
        public bool playAtStart;
        public UseAnimationKeys animationKey;
    }
}
