using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public ParticleSystem bulletExplosionPartcles;
    public AudioSource bulletExplosionAudio;
    public LayerMask tankMask; // layers to affect
    public float maxDamage = 50;
    public float explosionForce = 1000f;
    public float lifeSpan = 2f;
    public float explosionRadius = 5f;


    void Start()
    {
        Destroy(gameObject, lifeSpan);
    }

    private void OnTriggerEnter(Collider col)
    {
        // Find all the tanks (targets on layer mask) around the given radius.
        Collider[] targetCollliders = Physics.OverlapSphere(transform.position, explosionRadius, tankMask);
        for (int i = 0; i < targetCollliders.Length; i++)
        {
            // get target's rigidobdy
            Rigidbody _targetRB = null;
            if (targetCollliders[i].GetComponent<Rigidbody>())
                _targetRB = targetCollliders[i].GetComponent<Rigidbody>();

            // Add force to the target
            _targetRB.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            // get target's helathscript
            PlayerHealth _targetHealth = null;
            if (targetCollliders[i].GetComponent<PlayerHealth>())
                _targetHealth = targetCollliders[i].GetComponent<PlayerHealth>();

            int _damage = CalculateDamage(_targetRB.position);
            _targetHealth.TakeDamage(_damage);
        }

        // unparent the particles from bullet
        bulletExplosionPartcles.transform.parent = null;
        bulletExplosionPartcles.Play();
        bulletExplosionAudio.Play();

        // destroy particles after the particles duration
        Destroy(bulletExplosionPartcles.gameObject, bulletExplosionPartcles.main.duration);

        // destroy bullet
        Destroy(gameObject);
    }

    private int CalculateDamage(Vector3 _targetPosition)
    {
        // calculate damage based on its position. 
        // closer the target, higher the damage.

        // find the distance between explosion and the target
        Vector3 _targetdistance = _targetPosition - transform.position;

        // find distance length
        float _distanceLength = _targetdistance.magnitude;

        float _relativeDistance = (explosionRadius - _distanceLength) / explosionRadius;

        // actual damage based on length
        int _damage = (int)(_relativeDistance * maxDamage);

        // we have to do this because there is an edege case in this scenerio
        // if the tank is not in the radius but its collider is within the range then
        // the distance could be a negative value.
        // in this case set damage to 0
        _damage = (int)Mathf.Max(0, _damage);

        //print(_damage);
        return _damage;
    }
}
