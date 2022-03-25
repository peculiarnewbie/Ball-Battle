using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    [SerializeField] Image energyBar;
    [SerializeField] Image activatedBar;
    float actualEnergy;

    int filledEnergy;
    public int activatedEnergy;

    // Start is called before the first frame update
    void Start()
    {
        actualEnergy = energyBar.fillAmount = activatedBar.fillAmount = 0.0f;
        filledEnergy = activatedEnergy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(actualEnergy > 6f) return;
        actualEnergy += 0.5f*Time.deltaTime/6f;
        UpdateFillAmount();
    }

    private void UpdateFillAmount(){
        energyBar.fillAmount = actualEnergy;

        filledEnergy = (int) (energyBar.fillAmount * 6);
        
        if(filledEnergy != activatedEnergy){
            activatedEnergy = filledEnergy;
            activatedBar.fillAmount = (float) activatedEnergy / 6f;
        }
    }

    public void UseEnergy(int cost){
        if(actualEnergy >= 6f) actualEnergy = 4f; //to keep value from exceeding 6
        else actualEnergy -= 2f;
    }
}
