using UnityEngine;

public class GuardianController : BaseController
{
    public float NormalSpeed = 7;
    public  float JumpForce = 5;

    public bool levelChase;

    private int flutterAmount = 5;

    public GameObject lightSource;
    public Light lt;
    public float decrease;

    public bool captured;

    public override void Start()
    {
        base.Start();

        // Check if Joystick exists
        if (Input.GetJoystickNames().Length > 0)
        {
            if (!switchControls)
            {
                connectedController = new Joystick2();
            }
            else
            {
                connectedController = new Joystick1();
            }
        }
    }

    public override void Update()
    {
        base.Update();

        if (levelChase) {
            //NormalSpeed += Time.deltaTime * 5;
            NormalSpeed += 0.3f * Time.deltaTime;
            //NormalSpeed = currentSpeed;
        }

        if (flutterAmount < 5 && grounded)
        {
            flutterAmount = 5;
        }

        //keeps guardian trapped in cage while imprisoned 
        if (captured)
        {
            this.gameObject.layer = 20;
        }
        else { this.gameObject.layer = 9; }
    }

    public void FixedUpdate()
    {

        MoveHorizontally(NormalSpeed); 
        
    }

    //override collision check so players can jump on eachothers head
    new public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Player" || collision.transform.tag == "GuardianObstacle")
        {
            grounded = true;
        }
    }


    //override collision check so players can jump on eachothers head
    new public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Player" || collision.transform.tag == "GuardianObstacle")
        {
            grounded = false;
        }
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Hint")
        {
            lightSource.SetActive(true);
            if (x_active)
            {
                if (!Conversation._Instance.playing)
                {
                    col.gameObject.GetComponent<Hint>().SetHintActive();
                }
            }
        }

        if (col.tag == "Bait" || col.tag == "BoyCandy") 
        {
            BaitScript baitScript = col.GetComponent<BaitScript>();
            baitScript.SetButtonActive(true);
            if (connectedController != null && x_active)
            {
                StartCoroutine(baitScript.DropBait());
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(baitScript.DropBait());

            }
        }
    }


    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Hint")
        {
            lightSource.SetActive(false);
            if (col.gameObject.GetComponent<Hint>() != null)
            {
                col.gameObject.GetComponent<Hint>().SetHintActive();
            }
        }

        if (col.tag == "Bait")
        {
            BaitScript baitScript = col.GetComponent<BaitScript>();
            baitScript.SetButtonActive(false);
        }
    }

    public void UseLight()
    {
        lt.intensity -= decrease;
    }

    public override void GetInput()
    {
        base.GetInput();

        if (connectedController != null)
        {
            if (a_active && flutterAmount > 0)
            {
                Jump(JumpForce);
                flutterAmount--;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && flutterAmount > 0)
            {
                Jump(JumpForce);
                flutterAmount--;
            }
        }
    }
}
