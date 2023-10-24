using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    public int popUpIndex = 100;
    public bool tutorialStarted = false;
    private bool popUpStarted = false;
    private bool timePaused = false;

    private bool firstTutPassed = false;
    private bool secondTutPassed = false;
    private bool thirdTutPassed = false;
    private bool fourthTutPassed = false;
    private bool fourthTutPressed = false;
    private bool fifthTutPassed = false;
    private bool fifthTutPressed = false;
    private bool sixthTutPassed = false;
    private bool sixthTutPressed = false;

    private void Update()
    {
        
        if (tutorialStarted == true)
        {
            StartCoroutine("Delay");
            if (popUpStarted == true)
            {
                for (int i = 0; i < popUps.Length; i++)
                {
                    if (i == popUpIndex)
                    {
                        if (i == 3 && fourthTutPressed == false)
                        {
                            Debug.Log("on 3");
                            popUps[i].SetActive(true);
                            break;
                        }
                        else if (i == 4 && fifthTutPressed == false)
                        {
                            popUps[i].SetActive(true);
                            break;
                        }
                        else if (i != 3 || i != 4)
                        {
                            if (popUpIndex == 3 || popUpIndex == 4)
                            {
                                break;
                            }
                            Debug.Log("SpawnAnyway");
                            popUps[i].SetActive(true);
                        }
                        
   
                    }
                    else
                    {
                        popUps[i].SetActive(false);
                    }
                }

                if (popUpIndex == 0)
                {
                    //Swipe left or Right
                    if (firstTutPassed == false)
                    {
                        timePaused = true;
                        StartCoroutine("FirstTutorial");
                    }
                    else if (firstTutPassed == true)
                    {
                        timePaused = false;
                    }
                }
                else if (popUpIndex == 1)
                {
                    //Swipe Down
                    if (secondTutPassed == false)
                    {
                        timePaused = true;
                        StartCoroutine("SecondTutorial");
                    }
                    else if (secondTutPassed == true)
                    {
                        timePaused = false;
                    }
                    
                }
                else if (popUpIndex == 2)
                {
                    //Swipe Up
                    if (thirdTutPassed == false)
                    {
                        timePaused = true;
                        StartCoroutine("ThirdTutorial");
                    }
                    else if (thirdTutPassed == true)
                    {
                        timePaused = false;
                    }

                }
                else if (popUpIndex == 3)
                {
                    //CollectCoins
                    if (fourthTutPassed == false)
                    {
                        timePaused = true;
                        StartCoroutine("FourthTutorial");
                    }
                    else if (fourthTutPassed == true)
                    {
                        timePaused = false;
                    }

                }
                else if (popUpIndex == 4)
                {
                    //CollectCoins
                    if (fifthTutPassed == false)
                    {
                        timePaused = true;
                        StartCoroutine("FifthTutorial");
                    }
                    else if (fifthTutPassed == true)
                    {
                        timePaused = false;
                    }

                }
                else if (popUpIndex == 5)
                {
                    //You are Ready to Play
                    if (sixthTutPassed == false)
                    {
                        timePaused = true;
                        StartCoroutine("SixthTutorial");
                    }
                    else if (sixthTutPassed == true)
                    {
                        timePaused = false;
                    }
                }

            }

        }

        if (timePaused == true)
        {
            Time.timeScale = 0;
        }
        else if (timePaused == false)
        {
            Time.timeScale = 1;
        }

    }
  
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.0f);
        popUpStarted = true;
    }

    IEnumerator FirstTutorial()
    {
        if (MobileInput.Instance.SwipeLeft || MobileInput.Instance.SwipeRight)
        {
            firstTutPassed = true;
            Debug.Log("Test1");
            yield return new WaitForSeconds(2.0f);
            popUpIndex++;
        }
    }

    IEnumerator SecondTutorial()
    {
        if (MobileInput.Instance.SwipeDown)
        {
            secondTutPassed = true;
            Debug.Log("Test2");
            yield return new WaitForSeconds(2.0f);
            popUpIndex++;
        }
    }

    IEnumerator ThirdTutorial()
    {
        if (MobileInput.Instance.SwipeUp)
        {
            thirdTutPassed = true;
            Debug.Log("Test3");
            yield return new WaitForSeconds(4.5f);
            popUpIndex++;
        }
    }

    IEnumerator FourthTutorial()
    {
        if (fourthTutPressed == true)
        {
            fourthTutPassed = true;
            yield return new WaitForSeconds(4.5f);
            popUpIndex++;
        }
    }

    IEnumerator FifthTutorial()
    {
        if (fifthTutPressed == true)
        {
            fifthTutPassed = true;
            yield return new WaitForSeconds(4.5f);
            popUpIndex++;
        }
    }

    IEnumerator SixthTutorial()
    {
        if (sixthTutPressed == true)
        {
            sixthTutPassed = true;
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("Game");
        }
    }
    public void FourthTut()
    {
        fourthTutPressed = true;
        Debug.Log("Test4");
        popUps[3].SetActive(false);
    }
    public void FifthTut()
    {
        fifthTutPressed = true;
        Debug.Log("Test5");
        popUps[4].SetActive(false);
    }
    public void SixthTut()
    {
        sixthTutPressed = true;
        Debug.Log("Test6, test6");
        popUps[5].SetActive(false);
    }

}
