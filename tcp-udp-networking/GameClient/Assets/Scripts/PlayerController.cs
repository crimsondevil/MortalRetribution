using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    public Transform weaponMuzzle;

    [Header("Audio & Visual")]
    public GameObject muzzleFlashPrefab;
    public AudioClip shootSFX;
    //public AudioSource audioSource;
    //public AudioClip footstepSFX;
    //public AudioClip jumpSFX;

    private void Update()
    {

        //CalculatePingDisplay.UpdateClient();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            
            ClientSend.PlayerShoot(camTransform.forward);


        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ClientSend.PlayerThrowItem(camTransform.forward);
        }
    }

    private void FixedUpdate()
    {
        SendInputToServer();

        

    }

    /// <summary>Sends player input to the server.</summary>
    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space)
        };

        ClientSend.PlayerMovement(_inputs);
    }
}
