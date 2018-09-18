using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MyTankManager
{
    public Color tankColor;
    public Transform spawnPosition;

    [HideInInspector] public int playerNumber;
    [HideInInspector] public GameObject instance; // tank instance
    [HideInInspector] public int wins;

    private Tank tankMovements; // to disable tank movements control
    private ShootingScript tankShooting; // to disable tank shooting control

    public void Setup()
    {
        tankMovements = instance.GetComponent<Tank>();
        tankShooting = instance.GetComponent<ShootingScript>();

        tankMovements.playerNumber = playerNumber;
        tankShooting.playerNumber = playerNumber;

        // coloring mesh renderer
        MeshRenderer[] meshes = instance.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].material.color = tankColor;
        }
    }

    public void DisableControls()
    {
        tankMovements.enabled = false;
        tankShooting.enabled = false;
    }

    public void EnableControls()
    {
        tankMovements.enabled = true;
        tankShooting.enabled = true;
    }

    public void Reset()
    {
        instance.transform.position = spawnPosition.position;
        instance.transform.rotation = spawnPosition.rotation;

        instance.SetActive(false);
        instance.SetActive(true);
    }
}
