using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialGameManager : MonoBehaviour
{
    private const int COIN_SCORE_AMOUNT = 5;
    //public static GameManager Instance { set; get; }

    //This is an object of the GameData save Class
    public GameData saveData = new GameData();

    public bool IsDead { set; get; }
    private bool isGameStarted = false;
    private PlayerMotor motor;

    //save game data 
    private int totalCoin;
    private int finalCoin;






    // UI and UI Fields
    public Animator gameCanvas, menuAnim, diamondAnim;
    public TextMeshProUGUI scoreText, coinText, modifierText, hiscoreText, hiCoinText;
    private float score, coinScore, modifierScore;
    private int lastScore;

    //Buff UI and Cooldown
    public GameObject scoreBuffUI, shieldBuffUI, magnetBuffUI;
    public TextMeshProUGUI scoreBuffText, shieldBuffText, magnetBuffText, coinBankText;
    public float scoreBuffTime = 20f;
    public float shieldBuffTime = 20f;
    public float magnetBuffTime = 20f;
    public float bufftime = 20f;



    //Death Menu
    public Animator deathMenuAnim, reviveMenuAnim;
    public TextMeshProUGUI deadScoreText, deadCoinText;

    private void Awake()
    {
        //Instance = this;
        modifierScore = 1;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();



        modifierText.text = "x" + modifierScore.ToString("0.0");
        coinText.text = coinScore.ToString("0");
        scoreText.text = score.ToString("0");

        //Does the initial  and Save

        //SaveSystem.instance.SaveGame(saveData);
        //saveData = SaveSystem.instance.LoadGame();

    }

    public void Start()
    {
        if ((saveData = SaveSystem.instance.LoadGame()) == null)
        {
            SaveSystem.instance.SaveGame(saveData);
        }
        else
        {
            saveData = SaveSystem.instance.LoadGame();
        }
        //sets the required variable intially
        hiscoreText.text = saveData.dataHighScore.ToString();
        hiCoinText.text = saveData.dataDiamond.ToString();
        finalCoin = saveData.dataDiamond;
    }
    private void Update()
    {
       /* if (MobileInput.Instance.Tap && !isGameStarted)
        {
           
                isGameStarted = true;
                motor.StartRunning();
                FindObjectOfType<GlacierSpawner>().IsScrolling = true;
                FindObjectOfType<CameraMotor>().IsMoving = true;
                gameCanvas.SetTrigger("Show");
                menuAnim.SetTrigger("Hide");
        }
       */

        
            
        if (isGameStarted && !IsDead)
        {
            //Bmp up the score
            score += (Time.deltaTime * modifierScore);
            if (lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = score.ToString("0");

            }
        }

        buffTime();

    }

    public void PlayTutButton()
    {
        isGameStarted = true;
        motor.StartRunning();
        FindObjectOfType<GlacierSpawner>().IsScrolling = true;
        FindObjectOfType<CameraMotor>().IsMoving = true;
        gameCanvas.SetTrigger("Show");
        menuAnim.SetTrigger("Hide");
        FindObjectOfType<TutorialManager>().tutorialStarted = true;
        FindObjectOfType<TutorialManager>().popUpIndex = 0;
    }

    public void SkipTutButton()
    {
        SceneManager.LoadScene("Game");
    }


    public void GetCoin()
    {
        diamondAnim.SetTrigger("Collect");
        coinScore++;
        coinText.text = coinText.text = coinScore.ToString("0");
        score += COIN_SCORE_AMOUNT;
        scoreText.text = score.ToString("0");
    }



    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");

    }

    public void OnPlayButton()
    {
        finalCoin += totalCoin;
        saveData.AddDiamondData(finalCoin);
        SaveSystem.instance.SaveGame(saveData);

        SceneManager.LoadScene("Game");
    }

    public void OnDeath()
    {
        IsDead = true;



        FindObjectOfType<GlacierSpawner>().IsScrolling = false;
        gameCanvas.SetTrigger("Hide");
        deadScoreText.text = score.ToString("0");
        deadCoinText.text = coinScore.ToString("0");
        coinBankText.text = finalCoin.ToString();
        deathMenuAnim.SetTrigger("Dead");




        //check if this is a highscore
        if (score > saveData.dataHighScore)
        {
            float s = score;
            if (s % 1 == 0)
            {
                s += 1;
            }

            saveData.AddHighScoreData((int)s);
            SaveSystem.instance.SaveGame(saveData);

        }

    }

    public void OnRevive()
    {
        IsDead = false;
        FindObjectOfType<GlacierSpawner>().IsScrolling = true;
        reviveMenuAnim.SetTrigger("Hide");
        gameCanvas.SetTrigger("Show");
    }

    public void ReviveButton()
    {
        deathMenuAnim.SetTrigger("Default");
        reviveMenuAnim.SetTrigger("Show");
    }



    public void ScoreBuffEffect()
    {
        scoreBuffUI.SetActive(true);
        modifierText.color = new Color(0, 0, 0);
        motor.scoreBuffMulti = 3f;

    }

    public void ShieldBuffEffect()
    {
        shieldBuffUI.SetActive(true);

    }

    public void MagnetEffect()
    {
        magnetBuffUI.SetActive(true);

    }

    public void AddtoCoinBank()
    {
        totalCoin++;

    }

    public void ReduceCoinBank()
    {

        finalCoin -= motor.reviveCost;
        saveData.AddDiamondData(finalCoin);
        SaveSystem.instance.SaveGame(saveData);

    }
    private void buffTime()
    {
        if (motor.isScoreOn == true)
        {
            scoreBuffTime -= Time.deltaTime;
            int scoreTime = (int)scoreBuffTime;
            scoreBuffText.text = scoreBuffTime.ToString("0");

            if (scoreTime == 0)
            {
                modifierText.color = new Color(1, 1, 1);
                motor.isScoreOn = false;
                scoreBuffUI.SetActive(false);
                motor.scoreBuffMulti = 0f;
                scoreBuffTime = 20f;
            }

        }

        if (motor.isMagnetOn == true)
        {
            magnetBuffTime -= Time.deltaTime;
            int magnetTime = (int)magnetBuffTime;
            magnetBuffText.text = magnetBuffTime.ToString("0");

            if (magnetTime == 0)
            {
                magnetBuffUI.SetActive(false);
                FindObjectOfType<PlayerMotor>().isMagnetOn = false;
                magnetBuffTime = 20f;
            }

        }

        if (motor.isShieldOn == true)
        {
            shieldBuffTime -= Time.deltaTime;
            int shieldTime = (int)shieldBuffTime;
            shieldBuffText.text = shieldBuffTime.ToString("0");

            if (shieldTime == 0)
            {
                shieldBuffUI.SetActive(false);
                motor.isShieldOn = false;
                shieldBuffTime = 20f;
            }

        }
    }

}
