﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour
{
    public int numberOfRounds = 3;
    public float startDelay = 2f;
    public float endDelay = 2f;
    public CameraController cameraControlScript;
    public GameObject tankPrefab;
    public MyTankManager[] tanks;

    private int roundNumber;
    private WaitForSeconds startWait;
    private WaitForSeconds endWait;
    private TankManager roundWinner;
    private TankManager gameWinner;

    void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }

    private void SpawnTanks()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            // instantiate tanks at tank's spawnposition
            tanks[i].instance = Instantiate(tankPrefab, tanks[i].spawnPosition.position, tanks[i].spawnPosition.rotation) as GameObject;
            tanks[i].playerNumber = i + 1;
            tanks[i].Setup();
        }
    }

    private void SetCameraTargets()
    {
        Transform[] _targets = new Transform[tanks.Length];
        for (int i = 0; i < _targets.Length; i++)
            _targets[i] = tanks[i].instance.transform;

        cameraControlScript.targets = _targets;
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (gameWinner != null)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else
            StartCoroutine(GameLoop());
    }

    private IEnumerator RoundStarting()
    {
        // reset all tanks
        // set camera pos and size
        // increment round number
        ReserAllTanks();
        DisableTankControls();
        cameraControlScript.SetStartPositionAndSize();
        roundNumber++;
        Debug.Log("==========ROUND NUMBER ----> " + roundNumber + "========== ");
        yield return startWait;

    }

    private void DisableTankControls()
    {
        for (int i = 0; i < tanks.Length; i++)
            tanks[i].DisableControls();
    }

    private void ReserAllTanks()
    {
        for (int i = 0; i < tanks.Length; i++)
            tanks[i].Reset();
    }

    private IEnumerator RoundPlaying()
    {
        yield return null;
    }

    private IEnumerator RoundEnding()
    {
        yield return endWait;
    }
}