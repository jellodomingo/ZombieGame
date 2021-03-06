﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyAI : MonoBehaviour
{

    public int Health = 50;
    public int AttackPower = 10;

    public int MinDist = -1;
    public int MaxDist = 100;
    public float MoveSpeed = 10f;

    public GameObject healthDrop;
    public GameObject ammoDrop;

    public Rigidbody2D rb;

    private bool canTakeDamage = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Health <= 0)
        {
            GameManager.Instance.KillUp();
            DropItem();
            Destroy(this.gameObject);
        }

        //transform.LookAt(PlayerDirection);
        Vector3 v3 = GameManager.Instance.player.transform.position;
        Vector2 v2 = v3;

        Vector2 lookDir = v2 - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) >= MinDist)
        {

            rb.MovePosition(rb.position + lookDir * MoveSpeed * Time.fixedDeltaTime);
            //transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (canTakeDamage)
            {
                StartCoroutine(WaitForSeconds());
                GameManager.Instance.PlayerHealthDown(AttackPower);
            }
        }
    }

    private void DropItem()
    {
        if (GameManager.Instance.ShouldDrop())
        {
            Debug.Log("DORP ME");
            if (Random.Range(0, 2) == 0)
            {
                GameObject h = Instantiate(healthDrop, this.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            }
            else
            {
                GameObject a = Instantiate(ammoDrop, this.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            }
        }
    }

    public void HealthDown(int damage)
    {
        Health -= damage;
    }

    IEnumerator WaitForSeconds()
    {
        canTakeDamage = false;
        yield return new WaitForSecondsRealtime(2);
        canTakeDamage = true;
    }




}
