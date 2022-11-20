using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using _ProjectBeta.Scripts.PlayerScrips;

public class Projectile : MonoBehaviourPun
{
    private float damage;
    private float speed;
    private float lifeTime;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lifeTime = 99999999;
    } 

    private void Update()
    {
        Move();
        if(lifeTime <= Time.time)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    public void Initialize(float speed, float lifeTime, float damage, Vector3 position)
    {
        this.speed = speed;
        this.lifeTime = lifeTime + Time.time;
        this.damage = damage;

        var rotation = Quaternion.LookRotation(position);
        var eulerAngles = transform.eulerAngles;

        eulerAngles = new Vector3(eulerAngles.x, rotation.eulerAngles.y, eulerAngles.z);
        transform.eulerAngles = eulerAngles;
    }

    private void Move()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerModel>(out var model))
        {
            model.DoDamage(damage, photonView.Owner);
        }
    }
}
