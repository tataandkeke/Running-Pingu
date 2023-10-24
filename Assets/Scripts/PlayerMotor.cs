using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private const float LANE_DISTANCE = 2.5f;
    private const float TURN_SPEED = 0.05f;

    //GameData saveData = new GameData();


    //Buffs
    public bool isMagnetOn = false;
    public bool isScoreOn = false;
    public bool isShieldOn = false;

    //Functionality
    private bool isRunning = false;
    [HideInInspector]
    public int reviveCost = 50;

    //Animation
    private Animator anim;

    // Movement
    private CharacterController controller;
    public float jumpForce = 5.0f;
    private float gravity = 12.0f;
    private float verticalVelocity;
    private int desiredLane = 1;       // 0 = left, 1 = middle, 2 = right

    // Speed Modifier
    private float originalSpeed = 7.0f;
    private float speed;
    private float speedIncreaseLastTick;
    private float speedIncreaseTime = 2.5f;
    private float speedIncreaseAmount = 0.1f;
    public float scoreBuffMulti = 0f;

    //sounds
    public AudioClip collectCoin;
    public AudioClip collectBuff;
    public AudioClip crashSound;
    public AudioSource playerAudio;

    

    // Start is called before the first frame update
    void Start()
    {
        speed = originalSpeed;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        //saveData = SaveSystem.instance.LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning)
        {
            return;
        }

        if(Time.time - speedIncreaseLastTick > speedIncreaseTime)
        { 
            speedIncreaseLastTick = Time.time;
            speed += speedIncreaseAmount;
            //change the modifier Text
            GameManager.Instance.UpdateModifier((speed - originalSpeed) + scoreBuffMulti);
            
            
        }

        //gather inputs on which lane we should be
        if (MobileInput.Instance.SwipeLeft)
        {
            MoveLane(false);
        }

        if (MobileInput.Instance.SwipeRight)
        {
            MoveLane(true);
        }

        //calculate where we should be
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * LANE_DISTANCE;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * LANE_DISTANCE;
        }

        //lets calculate our move delta
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        //bool isgrounded = IsGrounded();
        anim.SetBool("Grounded", IsGrounded());

        // Calculate Y
        if (IsGrounded()) // if Grounded
        {
            verticalVelocity = -0.1f;
            

            if(MobileInput.Instance.SwipeUp)
            {
                Time.timeScale = 1;
                //Jump
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce;
            }
            else if (MobileInput.Instance.SwipeDown)
            {
                //slide
                StartSliding();
                Invoke("StopSliding", 1.0f);
            }
        }
        else 
        {
            verticalVelocity -= (gravity * Time.deltaTime); // This will increase the gravity evry second player will fall gradualy

            //Fast fall / Siping Down while falling

            if (MobileInput.Instance.SwipeDown)
            {
                verticalVelocity = -jumpForce;
            }
        }
        moveVector.y = verticalVelocity; // this make it jump
        moveVector.z = speed;

        //move Pengu
        controller.Move(moveVector * Time.deltaTime);

        // Rotate the Peng to where he his going
        Vector3 dir = controller.velocity;

        if(dir != Vector3.zero)
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
        }
        
    }

    private void MoveLane(bool goingRight)
    {
        //checks wether going right is true/flase then increments the value based on the condition
        //Clamps the value into desirelane, between 0 and 2
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    private bool IsGrounded()
    {
        // this gets the position of the bottom of the character using the character controller component
        Ray groundRay = new Ray(new Vector3(controller.bounds.center.x,
            (controller.bounds.center.y - controller.bounds.extents.y) +  0.2f, 
            controller.bounds.center.z),
            Vector3.down);

        Debug.DrawRay(groundRay.origin, groundRay.direction,Color.cyan, 1.0f); // draws the ray groundRay on the scene

        if (Physics.Raycast(groundRay, 0.2f + 0.1f))
        {
            return true;
        }
        else
        {
            return false;
        } 

    }

    public void StartRunning()
    {
        isRunning = true;
        anim.SetTrigger("StartRunning");
    }

    private void StartSliding()
    {
        anim.SetBool("Sliding", true);
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
    }

    private void StopSliding()
    {
        anim.SetBool("Sliding", false);
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
    }

    private void Crash()
    {
        anim.SetTrigger("Death");
        isRunning = false;
        GameManager.Instance.OnDeath();
    }

    public void Revive()
    {
        if (FindObjectOfType<GameManager>().saveData.dataDiamond  /*PlayerPrefs.GetInt("CoinBank")*/ >= reviveCost)
        {
            GameManager.Instance.ReduceCoinBank();
            isShieldOn = true;
            GameManager.Instance.ShieldBuffEffect();
            anim.SetTrigger("Revive");
            isRunning = true;
            GameManager.Instance.OnRevive();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if(hit.gameObject.tag == "Obstacle" && isShieldOn == false)
        {
            playerAudio.PlayOneShot(crashSound, 1.0f);
            Crash();
        }
        else if(hit.gameObject.tag == "Obstacle" && isShieldOn == true)
        {
            hit.transform.parent.gameObject.SetActive(false);
            GameManager.Instance.shieldBuffUI.SetActive(false);
            GameManager.Instance.shieldBuffTime = 20f;
            isShieldOn = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Magnet Buff")
        {
            playerAudio.PlayOneShot(collectBuff, 1.0f);
            if (isMagnetOn == false)
            {
                isMagnetOn = true;
                GameManager.Instance.MagnetEffect();
            }
            else if(isMagnetOn == true)
            {
                GameManager.Instance.magnetBuffTime = 20f;
                GameManager.Instance.MagnetEffect();
            }
            
        }

        if(other.tag == "Score Buff")
        {
            playerAudio.PlayOneShot(collectBuff, 1.0f);
            if (isScoreOn == false)
            {
                isScoreOn = true;
                GameManager.Instance.ScoreBuffEffect();
            }
            else if(isScoreOn == true)
            {
                GameManager.Instance.scoreBuffTime = 20f;
                GameManager.Instance.ScoreBuffEffect();
            }
            
        }

        if(other.tag == "Shield Buff")
        {
            playerAudio.PlayOneShot(collectBuff, 1.0f);
            if (isShieldOn == false)
            {
                isShieldOn = true;
                GameManager.Instance.ShieldBuffEffect();
            }
            else if(isShieldOn == true)
            {
                GameManager.Instance.shieldBuffTime = 20f;
                GameManager.Instance.ShieldBuffEffect();
            }
            

        }

        if (other.gameObject.CompareTag("Coin"))
        {
            playerAudio.PlayOneShot(collectCoin, 1.0f);
        }
    }
}
