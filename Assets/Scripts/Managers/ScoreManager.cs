using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using YG;
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText, endScoreText;
    
   
    private int score;


   
    private void Start()
    {
        score = 0;
        Enemy.onDeadZombie += ScoreAdd;
        PlayerHealth.onDeath += Death;      
        ChangeScoreText(score);

    }
    public void OnDestroy()
    {        
        Enemy.onDeadZombie -= ScoreAdd;
        PlayerHealth.onDeath -= Death;
    }
    private void ScoreAdd(Enemy enemy)
    {
        score++;        
        ChangeScoreText(score);      
    }
    private void ChangeScoreText(int _score)
    {
        scoreText.text = _score.ToString();
        endScoreText.text = _score.ToString();
       

    }
    private void Death()
    {
        int maxscore = YandexGame.savesData.kills;
        if(score>maxscore){
            YandexGame.NewLeaderboardScores("Leaders", score);
            YandexGame.savesData.kills = score;
        }
    }
    
}
