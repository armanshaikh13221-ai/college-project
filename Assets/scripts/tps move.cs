using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class tpsmove : MonoBehaviour
{
    [Header("movement")]
    public float movespeed;
    public Transform orientation;
    Rigidbody rg;
    float horizontalinput;
    float verticalinput;
    public float jumpspeed;
    public float jumpforce;
    public float airMultiplier;
    bool readytojump=true;
    Vector3 movedirection;
    
    public float grounddrag;

    [Header("ground check")]
    public float playerheight;
    public LayerMask ground;
    bool grounded;

    public Transform groundcheck;
    public float groundDistance=0.4f;

    [Header("keybinds")]
    public KeyCode jumpkey=KeyCode.Space;

    
    
    void Start()
    {
        rg = GetComponent<Rigidbody>();
        rg.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.CheckSphere(groundcheck.position, groundDistance, ground);
        myinput();

        if (grounded)
        
            rg.linearDamping = grounddrag;
        
        else
        
            rg.linearDamping = 0f;
        
        
    }

    void FixedUpdate()
    {
        
        moveplayer();
        speedcontrol();
        
    }
    void myinput()
    {
        horizontalinput= Input.GetAxisRaw("Horizontal");
        verticalinput= Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(jumpkey) && readytojump && grounded)
        {
            readytojump = false;
            jump();

            Invoke(nameof(resetjump), jumpspeed);
        }
    }

    void moveplayer()
    {
        movedirection= orientation.forward * verticalinput + orientation.right * horizontalinput;   

        rg.AddForce(movedirection.normalized * movespeed * 10f, ForceMode.Force);

        if(!grounded)
        {
            rg.AddForce(movedirection.normalized * movespeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    void speedcontrol()
    {
        Vector3 flatvel= new Vector3(rg.linearVelocity.x, 0f, rg.linearVelocity.z);

        if(flatvel.magnitude > movespeed)
        {
            Vector3 limitedvel= flatvel.normalized * movespeed;
            rg.linearVelocity = new Vector3(limitedvel.x, rg.linearVelocity.y, limitedvel.z);
        }
    }

    void jump()
    {
        rg.linearVelocity = new Vector3(rg.linearVelocity.x, 0f, rg.linearVelocity.z);

        rg.AddForce(transform.up * jumpforce, ForceMode.Impulse);
    }
    void resetjump()
    {
        readytojump = true;
    }

    



}
