
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public int maxCoin = 5; // chooses the limit of the coins you want to be spawning
    public float chanceToSpawn = 0.5f;
    public bool forceSpawnAll = false;

    private GameObject[] coins;

    private void Awake()
    {
        coins = new GameObject[transform.childCount]; // creates a new array and set to the size of the amount of children of gameobject
        for (int i = 0; i < transform.childCount; i++)// after initializing the array, this will loop through the array space and store each child of th coin gameobject into the array
        {
            coins[i] = transform.GetChild(i).gameObject;
        }

        OnDisable();
    }

    private void OnEnable()
    {
        // this is for when the game object gets active on the scene


        // this checks the chance of spawning set on eahc individual coni prefab
        if(Random.Range(0.0f, 1.0f) > chanceToSpawn)
        {
            return;
        }

        //if the coin is set to spawn all its children. Then this loo will go trough the children and activate each one
        if(forceSpawnAll)
        {
            for (int i = 0; i < maxCoin; i++)
            {
                coins[i].SetActive(true);
            }
        }
        else // if we are meant to spawn a set amount
        {
            int r = Random.Range(0, maxCoin); // this randomizes between 0 and number set to choose what to spawn
            for (int i = 0; i < r; i++)        //goes through each elements of the coin list based on the random number of times
            {
                coins[i].SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        // when called just disable the gamweobject
        foreach(GameObject go in coins)
        {
            go.SetActive(false);
        }
    }
}
