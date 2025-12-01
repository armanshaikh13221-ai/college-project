using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Timeline;

public class tpscam : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("references")]
    public Transform orientation;
    public Transform player;
    public Transform playerobj;
    public Rigidbody rg;
    public float roatationspeed;
    public Transform combatlook;
    public GameObject thirpersoncam;
    public GameObject fppcam;
    public GameObject combatcam;
   
    public camerastyle currentcamerastyle;
    public enum camerastyle
    {
        basic,
        combat,
        fpp
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewDir= player.position - new Vector3(transform.position.x,player.position.y,transform.position.z);
        orientation.forward = viewDir.normalized;

        if(currentcamerastyle==camerastyle.basic){

        float horizontalinput= Input.GetAxis("Horizontal");
        float verticalinput= Input.GetAxis("Vertical");
        Vector3 inputDir= orientation.forward * verticalinput + orientation.right * horizontalinput;

        if(inputDir != Vector3.zero)
            playerobj.forward=Vector3.Slerp(playerobj.forward, inputDir.normalized, Time.deltaTime*roatationspeed);
        }
        else if(currentcamerastyle==camerastyle.combat){
            Vector3 dirtocombatlook= combatlook.position - new Vector3(transform.position.x,combatlook.position.y,transform.position.z);
            orientation.forward= dirtocombatlook.normalized;
            playerobj.forward= dirtocombatlook.normalized;

        }

         if(Input.GetKeyDown(KeyCode.Alpha1)) switchcamerastyle(camerastyle.basic);
            if(Input.GetKeyDown(KeyCode.Alpha2)) switchcamerastyle(camerastyle.combat);
    }

    void switchcamerastyle(camerastyle newStyle)
    {
        combatcam.SetActive(false);
        thirpersoncam.SetActive(false);
        
       

        if(newStyle == camerastyle.basic)thirpersoncam.SetActive(true);
        if(newStyle == camerastyle.combat)combatcam.SetActive(true);

        currentcamerastyle=newStyle;

    } 
}
