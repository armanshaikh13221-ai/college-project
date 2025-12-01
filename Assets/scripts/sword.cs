using UnityEngine;

public class sword : MonoBehaviour
{
    [SerializeField]
    private Animator sworddanim;
    public float cooldown=2f;
    private float nextattacktime=0f;
    public static int noofclicks=0;
    float lastclickedtime=0f;
    public float maxcombointerval=1f;
    [Header("Responsiveness")]
    [Tooltip("How quickly the animator will crossfade to the next hit state (seconds)")]
    public float transitionDuration = 0.05f;
    [Tooltip("Normalized time threshold to allow advancing combos earlier (0-1)")]
    public float comboAdvanceThreshold = 0.35f;
    [Tooltip("Normalized time threshold to clear hit bools")]
    public float exitTimeThreshold = 0.5f;
    void Start()
    {
        // Try to use an animator assigned in inspector first, otherwise try to get one on this GameObject
        if (sworddanim == null)
        {
            sworddanim = GetComponent<Animator>();
        }
        if (sworddanim == null)
        {
            sworddanim = GetComponentInChildren<Animator>();
        }
        if (sworddanim == null)
        {
            Debug.LogWarning("Animator not found on '" + gameObject.name + "'. Sword script will be disabled to avoid errors.");
            enabled = false; // disable this component to avoid repeated null checks if animator is missing
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(sworddanim.GetCurrentAnimatorStateInfo(0).normalizedTime>exitTimeThreshold && sworddanim.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            sworddanim.SetBool("hit1",false);
        }
        if(sworddanim.GetCurrentAnimatorStateInfo(0).normalizedTime>exitTimeThreshold && sworddanim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            sworddanim.SetBool("hit2",false);
        }
        if(sworddanim.GetCurrentAnimatorStateInfo(0).normalizedTime>exitTimeThreshold && sworddanim.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            sworddanim.SetBool("hit3",false);
            noofclicks=0;
        }

        if(Time.time - lastclickedtime > maxcombointerval)
        {
            noofclicks=0;
        }
        if(Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
    }
    
    void OnClick()
    {
        lastclickedtime=Time.time;
        noofclicks++;
        if(noofclicks==1)
        {
            // Start first hit immediately
            sworddanim.SetBool("hit1",true);
            sworddanim.Play("hit1", 0, 0f);
        }
        noofclicks=Mathf.Clamp(noofclicks,0,3);
        // Advance to hit2 earlier using a small crossfade for responsiveness
        if(noofclicks>=2 && sworddanim.GetCurrentAnimatorStateInfo(0).IsName("hit1")
           && sworddanim.GetCurrentAnimatorStateInfo(0).normalizedTime>=comboAdvanceThreshold)
        {
            sworddanim.SetBool("hit1",false);
            sworddanim.SetBool("hit2",true);
            sworddanim.CrossFade("hit2", transitionDuration, 0, 0f);
        }
        // Advance to hit3 similarly
        if(noofclicks>=3 && sworddanim.GetCurrentAnimatorStateInfo(0).IsName("hit2")
           && sworddanim.GetCurrentAnimatorStateInfo(0).normalizedTime>=comboAdvanceThreshold)
        {
            sworddanim.SetBool("hit2",false);
            sworddanim.SetBool("hit3",true);
            sworddanim.CrossFade("hit3", transitionDuration, 0, 0f);
            noofclicks=0;
        }
    }
}
