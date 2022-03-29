using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Text enemyScore;
    [SerializeField] Text playerScore;
    [SerializeField] Text enemyTeam;
    [SerializeField] Text playerTeam;
    [SerializeField] Text timeText;
    MatchManager matchManager;

    // Start is called before the first frame update
    void Start()
    {
        matchManager = MatchManager.Instance;

        matchManager.OnMatchStart += UpdateTeamName;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTexts();
    }

    private void UpdateTexts(){
        enemyScore.text = matchManager.enemyScore.ToString();
        playerScore.text = matchManager.playerScore.ToString();
        timeText.text = matchManager.matchTime.ToString("F0");
    }

    private void UpdateTeamName(){
        if(matchManager.isPlayerAttacking){
            enemyTeam.text = "Defender";
            playerTeam.text = "Attacker";
        }
        else{
            enemyTeam.text = "Attacker";
            playerTeam.text = "Defender";
        }
    }
}

