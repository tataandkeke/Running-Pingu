using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator anim;
    private BoxCollider coinCollider;

    public Vector3 original_Size;
    public Vector3 colliderMagnetSize = new Vector3(20.0f, 20.0f, 20.0f);
    public bool Collected = false;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        coinCollider = GetComponent<BoxCollider>();
        original_Size = coinCollider.size;
        Collected = false;
        
    }
    // Start is called before the first frame update

    private void OnEnable()
    {
        //in the coin animator there is a trigger spawn
        //this transition is from anystate to the idle rotate state
        //this helps it spwn properly and despawn as well
        coinCollider.size = original_Size;
        if(FindObjectOfType<PlayerMotor>().isMagnetOn == true)
        {
            
            coinCollider.size = colliderMagnetSize;
        }
        else
        {
            coinCollider.size = original_Size;
            anim.SetTrigger("Spawn");
        }
        
    }

    private void Update()
    {
        if (FindObjectOfType<PlayerMotor>().isMagnetOn == true && Collected == false)
        {
            
            coinCollider.size = colliderMagnetSize;
        }
        else if (FindObjectOfType<PlayerMotor>().isMagnetOn == false)
        {
            
            coinCollider.size = original_Size;
        }

        if(Collected == true)
        {
            coinCollider.size = original_Size;
        }


    }

  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Collected = true;
            GameManager.Instance.AddtoCoinBank();
            coinCollider.size = original_Size;
            GameManager.Instance.GetCoin();
            anim.SetTrigger("Collected");
        }
        
    }

   
}
