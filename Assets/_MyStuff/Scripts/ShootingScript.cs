using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingScript : MonoBehaviour
{
    public int playerNumber = 1; // each player has a different fire button
    public Rigidbody bulletRB; // Rb attached to bullet to add force/velocity
    public Transform fireTransform; // bullet initial position 
    public Slider aimSlider;
    public AudioSource shootingAudio;
    public AudioClip chargingClip;
    public AudioClip fireClip;
    public float minLaunchForce = 15f; // same as min value in the aim slider
    public float maxLaunchForce = 30f; // same as max value in the aim slider
    public float maxChargeTime = 0.75f;


    private string fireButtonName; // input button
    private float currentLaunchForce;
    private float chargeSpeed;
    private bool hasfired;

    private void OnEnable()
    {
        //hasfired = false;
        aimSlider.value = minLaunchForce;
        currentLaunchForce = minLaunchForce;
    }

    void Start()
    {
        fireButtonName = "Fire" + playerNumber;

        // calculating speed by the formula Speed = Distance / Time
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
    }

    void Update()
    {
        // tracking fire button state and making decision based on that
        // there will be four states :
        // 1 - if button is held longer than the max launch force
        // 2 - if button is first pressed
        // 3 - if button is being held and not longer than the max launch force
        // 4 - if button is released

        // set slider's value to default/ min value at every frame
        aimSlider.value = minLaunchForce;

        if (currentLaunchForce >= maxLaunchForce && !hasfired)
        {
            // at max charge, and not yet fired

            currentLaunchForce = maxLaunchForce;
            Fire();
        }
        else if (Input.GetButtonDown(fireButtonName))
        {
            // pressed fire button for the first time

            hasfired = false;
            currentLaunchForce = minLaunchForce;

            shootingAudio.clip = chargingClip;
            shootingAudio.Play();

        }
        else if (Input.GetButton(fireButtonName) && !hasfired)
        {
            // still holding the fire button, not yet released

            currentLaunchForce += chargeSpeed * Time.deltaTime;

            // updating aim slider value UI
            aimSlider.value = currentLaunchForce;
        }
        else if (Input.GetButtonUp(fireButtonName) && !hasfired)
        {
            // fire button is released, and not yet fired

            Fire();
        }

    }

    private void Fire()
    {
        hasfired = true;
        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        print(currentLaunchForce);

        Rigidbody bullet = Instantiate(bulletRB, fireTransform.position, fireTransform.rotation) as Rigidbody;
        bullet.velocity = fireTransform.forward * currentLaunchForce;

        currentLaunchForce = minLaunchForce;
    }
}
