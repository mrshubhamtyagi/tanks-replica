using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public int playerNumber = 1; // Assigning a ID to each player
    public float moveSpeed = 10f;
    public float turnSpeed = 180f;
    public AudioSource movementAudio;
    public AudioClip engineDriving;
    public AudioClip engineIdle;
    public float audioPitchRange = 0.2f;

    private string moveAxisName;
    private string turnAxisName;
    private Rigidbody rb;
    private float moveInputValue;
    private float turnInputValue;
    private float originalPitch;

    private void Awake()
    {
        if (GetComponent<Rigidbody>())
            rb = GetComponent<Rigidbody>();
        else
            Debug.LogError("RigidBody is missing on the Tank");
    }

    private void OnEnable()
    {
        rb.isKinematic = false;
        moveInputValue = 0f;
        turnInputValue = 0f;
    }

    private void OnDisable()
    {
        rb.isKinematic = true;
    }

    void Start()
    {
        moveAxisName = "Vertical" + playerNumber;
        turnAxisName = "Horizontal" + playerNumber;
        originalPitch = movementAudio.pitch;
    }

    void Update()
    {
        moveInputValue = Input.GetAxis(moveAxisName);
        turnInputValue = Input.GetAxis(turnAxisName);
        EngineAudio();
    }

    private void EngineAudio()
    {
        // Adjusting audio according to the tank state (Driving or Ideling)
        if (Mathf.Abs(moveInputValue) < 0.1f && Mathf.Abs(turnInputValue) < 0.1f)
        {
            if (movementAudio.clip == engineDriving)
            {
                movementAudio.clip = engineIdle;
                movementAudio.pitch = Random.Range(originalPitch - audioPitchRange, originalPitch + audioPitchRange);
                movementAudio.Play();
            }
        }
        else
        {
            if (movementAudio.clip == engineIdle)
            {
                movementAudio.clip = engineDriving;
                movementAudio.pitch = Random.Range(originalPitch - audioPitchRange, originalPitch + audioPitchRange);
                movementAudio.Play();
            }
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            Move();
            Turn();
        }
    }

    private void Move()
    {
        // Adjusting position of tank
        Vector3 movement = transform.forward * moveInputValue * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);


    }

    private void Turn()
    {
        // Adjusting rotation of tank
        float turn = turnInputValue * turnSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * rotation);
    }
}
