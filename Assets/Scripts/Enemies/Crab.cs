﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour {

    public int health;
    public GameObject particleEffect;
    SpriteRenderer spriteRenderer;
    int direction;
    float timer = 1f;
    public float speed;
    public Sprite facingUp;
    public Sprite facingDown;
    public Sprite facingRight;
    public Sprite facingLeft;
    float changeTimer = .2f;
    bool shouldChange;
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = Random.Range(0, 3);
        shouldChange = false;
    }
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = 1.5f;
            direction = Random.Range(0, 3);
        }
        Movement();
        if (shouldChange)
        {
            changeTimer -= Time.deltaTime;
            if(changeTimer <= 0)
            {
                shouldChange = false;
                changeTimer = .2f;
            }
        }
	}

    void Movement()
    {
        if(direction == 0)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            spriteRenderer.sprite = facingDown;
        } else if(direction == 1)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            spriteRenderer.sprite = facingLeft;
        } else if(direction == 2)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            spriteRenderer.sprite = facingRight;
        } else if(direction == 3)
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
            spriteRenderer.sprite = facingUp;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Sword")
        {
            health--;
            checkHealth();
            col.GetComponent<Sword>().CreateParticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "SkillDarkness")
        {
            health -= 2;
            col.gameObject.GetComponent<SkillDarkness>().CreateParticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
            Destroy(col.gameObject);
            checkHealth();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            health--;
            checkHealth();
        }
        if(col.gameObject.tag == "Wall")
        {
            if (shouldChange) return;
            if (direction == 0) direction = 3;
            else if (direction == 1) direction = 2;
            else if (direction == 2) direction = 1;
            else if (direction == 0) direction = 3;
            shouldChange = true;
        }
    }

    private void checkHealth()
    {
        if (health <= 0)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CheckEnemiesDead>().KilledEnemy(gameObject);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CheckEnemiesDead>().AreEnemiesDead();
            Instantiate(particleEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
