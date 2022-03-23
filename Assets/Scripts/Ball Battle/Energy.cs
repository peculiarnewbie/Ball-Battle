using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    [SerializeField] Image energyBar;
    [SerializeField] Image activatedBar;

    int filledEnergy;
    int activatedEnergy;

    // Start is called before the first frame update
    void Start()
    {
        energyBar.fillAmount = 0.0f;
        activatedBar.fillAmount = 0.0f;
        filledEnergy = activatedEnergy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        energyBar.fillAmount += 0.5f*Time.deltaTime/6f;
        filledEnergy = (int) (energyBar.fillAmount * 6);
        if(filledEnergy > activatedEnergy){
            activatedEnergy = filledEnergy;
            activatedBar.fillAmount = (float) activatedEnergy / 6f;
        }
    }
}
