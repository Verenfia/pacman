using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float totalScore;
    public float TotalScore { get => totalScore; }
    float gameTime;
    public float GameTime { get => gameTime; }

    public float LoadTime { get => loadTime; }
    public float LoadScore { get => loadScore; }

    float loadTime,loadScore;
    
    public void AddScore(float value){
        totalScore += value;
    }

#region Singleton
public static GameManager instance = null;
private void Awake() {
    if(instance == null){
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else{
        Destroy(gameObject);
    }
}
#endregion
    private void Start() {
        Load();
    }
    public void SetGameTime(float value){
        gameTime = value;
    }
    public void Save(){
        
        if((loadScore == 0 || totalScore > loadScore) && (loadTime == 0 || gameTime < loadTime)){
            PlayerPrefs.SetFloat("score", totalScore);
            PlayerPrefs.SetFloat("time", gameTime);
        }

        /*
        if(loadScore == 0 || totalScore > loadScore){
            PlayerPrefs.SetFloat("score", totalScore);
        }
        if(loadTime == 0 || gameTime < loadTime){
            PlayerPrefs.SetFloat("time", gameTime);
        }
        */
        
        Debug.Log("Game Saved");
    }

    public void Load(){
        loadScore = PlayerPrefs.GetFloat("score");
        loadTime = PlayerPrefs.GetFloat("time");
        Debug.Log("GameLoad");
    }

    public void Reset(){
        PlayerPrefs.DeleteAll();
    }

    private void OnApplicationQuit() {
         Save();   
    }
    
}
