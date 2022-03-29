using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Text enemyScore;
    [SerializeField] Text playerScore;
    [SerializeField] Text matchText;
    [SerializeField] Text enemyTeam;
    [SerializeField] Text playerTeam;
    [SerializeField] Text timeText;
    [SerializeField] GameObject resultUI;
    [SerializeField] Text resultText;
    MatchManager matchManager;

    // Start is called before the first frame update
    void Start()
    {
        matchManager = MatchManager.Instance;

        resultUI.SetActive(false);

        matchManager.OnMatchStart += UpdateTeamName;
        matchManager.OnGameEnd += ShowResult;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTexts();
    }

    private void UpdateTexts(){
        enemyScore.text = matchManager.enemyScore.ToString();
        playerScore.text = matchManager.playerScore.ToString();
        timeText.text = matchManager.matchTime.ToString("F0") + "s";
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
        matchText.text = "Round " + matchManager.match.ToString();
    }

    private void ShowResult(int result){
        resultUI.SetActive(true);
        switch(result){
            case 0:
                resultText.text = "Draw";
                break;
            case 1:
                resultText.text = "Player Wins";
                break;
            case 2:
                resultText.text = "Enemy Wins";
                break;
        }
    }
}

