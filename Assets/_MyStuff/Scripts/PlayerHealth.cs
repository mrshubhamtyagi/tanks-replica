using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public GameObject explosionParticlesPrefab;
    public Slider slider; // required to change fill image value
    public Image fillImage; // required to change color based on health
    public int startingHealth = 100;
    public Color zerohealthColor = Color.red;
    public Color fullHealthColor = Color.green;

    private ParticleSystem explosionParticles;
    private AudioSource explosionAudio;
    private int currentHealth;
    private bool isDead;


    private void Awake()
    {
        // instantiating particles in the beginning is the most efficient way to use particles
        // then turn it on and off according to the situation.
        explosionParticles = Instantiate(explosionParticlesPrefab).GetComponent<ParticleSystem>();
        explosionAudio = explosionParticles.GetComponent<AudioSource>();
        explosionParticles.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        isDead = false;
        currentHealth = startingHealth;

        SetHealthUI();
    }

    public void TakeDamage(int _damage)
    {
        // adjust the health, update the health UI, check if player is dead
        currentHealth -= _damage;

        SetHealthUI();

        if (currentHealth <= 0 && !isDead)
            OnDeath();
    }

    private void SetHealthUI()
    {
        // update slider value 
        slider.value = currentHealth;

        // update color value
        fillImage.color = Color.Lerp(zerohealthColor, fullHealthColor, (currentHealth / (float)startingHealth));
    }

    private void OnDeath()
    {
        isDead = true;
        // explosion particles position
        explosionParticles.transform.position = transform.position;
        explosionParticles.gameObject.SetActive(true);
        explosionParticles.Play();
        explosionAudio.Play();

        // deactivate the tank
        gameObject.SetActive(false);
    }
}
