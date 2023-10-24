using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    

    public int dataHighScore = 0;
    public int dataDiamond = 0;

    public void AddDiamondData(int diamond)
    {
        dataDiamond = diamond;
    }

    public void ReduceDiamondData(int diamond)
    {
        dataDiamond = diamond;
    }

    public void AddHighScoreData(int score)
    {
        dataHighScore = score;
    }
}
