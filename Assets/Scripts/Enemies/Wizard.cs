﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{

    Animator anim;
    public float speed;
    public int dir;
    float dirTimer = 1.2f;
    public int health;
    public GameObject deathParticle;
    bool canAttack;
    float attackTimer = 2f;
    public GameObject projectile;
    public float thrustPower;
    float changeTimer = .2f;
    bool shouldChange;
    float specialTimer = .5f;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        canAttack = false;
        shouldChange = false;
    }

    // Update is called once per frame
    void Update()
    {
        dirTimer -= Time.deltaTime;
        if (dirTimer <= 0)
        {
            dirTimer = 1.2f;
            switch (dir)
            {
                case 0:
                    dir = 3;
                    break;
                case 1:
                    dir = 0;
                    break;
                case 2:
                    dir = 1;
                    break;
                case 3:
                    dir = 2;
                    break;
                default:
                    dir = 1;
                    break;
            }
        }
        specialTimer -= Time.deltaTime;
        if (specialTimer <= 0)
        {
            SpecialAttack();
            SpecialAttack();
            specialTimer = .5f;
        }
        Movement();
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            attackTimer = 2f;
            canAttack = true;
        }
        Attack();
        if (shouldChange)
        {
            changeTimer -= Time.deltaTime;
            if (changeTimer <= 0)
            {
                shouldChange = false;
                changeTimer = .2f;
            }
        }
    }

    void Attack()
    {
        if (!canAttack)
        {
            return;
        }
        canAttack = false;
        if (dir == 0)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
        }
        else if (dir == 1)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.left * thrustPower);
        }
        else if (dir == 2)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.down * thrustPower);
        }
        else if (dir == 3)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
        }
    }

    void Movement()
    {
        if (dir == 0)
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
            anim.SetInteger("dir", dir);
        }
        else if (dir == 1)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", dir);
        }
        else if (dir == 2)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            anim.SetInteger("dir", dir);
        }
        else if (dir == 3)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", dir);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sword")
        {
            health--;
            col.gameObject.GetComponent<Sword>().CreateParticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
            Destroy(col.gameObject);
            checkHealth();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (shouldChange) return;
            if (dir == 0) dir = 2;
            else if (dir == 1) dir = 3;
            else if (dir == 2) dir = 0;
            else if (dir == 3) dir = 1;
            shouldChange = true;
        }
    }

    private void SpecialAttack()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
        int randomDir = Random.Range(0, 3);
        switch (randomDir)
        {
            case 0:
                newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
                break;
            case 1:
                newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
                break;
            case 2:
                newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.left * thrustPower);
                break;
            case 3:
                newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.down * thrustPower);
                break;
            default:
                newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.left * thrustPower);
                break;
        }
    }

    void checkHealth()
    {
        if (health <= 0)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CheckEnemiesDead>().KilledEnemy(gameObject);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CheckEnemiesDead>().AreEnemiesDead();
            Instantiate(deathParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }


}
