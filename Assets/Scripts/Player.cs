﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	public float speed;
    Animator anim;
    public Image[] hearts;
    public int maxHealth;
    public int currentHealth;
    public GameObject sword;
    public float thrustPower;
    public bool canMove;
    public bool canAttack;
    public bool iniFrames;
    SpriteRenderer sr;
    float iniTimer = 1f;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        if (PlayerPrefs.HasKey("maxHealth") && PlayerPrefs.HasKey("currentHealth"))
        {
            LoadGame();
        } else
        {
            currentHealth = maxHealth;
        }
        getHealth();
        canMove = true;
        canAttack = true;
        iniFrames = false;
        sr = GetComponent<SpriteRenderer>();
	}

    void getHealth()
    {
        for(int i=0; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }
        for(int i=0; i < currentHealth; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
        Movement();
        if (Input.GetKeyDown(KeyCode.A))
        {
            Attack();
        }
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (iniFrames)
        {
            iniTimer -= Time.deltaTime;
            int rn = Random.Range(0, 100);
            if (rn < 50) sr.enabled = false;
            if (rn >= 50) sr.enabled = true;
            if(iniTimer <= 0)
            {
                iniTimer = 1f;
                iniFrames = false;
                sr.enabled = true;
            }
        }
        getHealth();
	}

    void Attack()
    {
        if (!canAttack)
        {
            return;
        }
        canMove = false;
        canAttack = false;
        thrustPower = 250;
        GameObject newSword = Instantiate(sword, transform.position, sword.transform.rotation);
        if(currentHealth == maxHealth)
        {
            newSword.GetComponent<Sword>().special = true;
            canMove = true;
            thrustPower = 500;
        }
        #region SwordRotation
        int swordDir = anim.GetInteger("dir");
        anim.SetInteger("attackDir", swordDir);
        if (swordDir == 0)
        {
            newSword.transform.Rotate(0, 0, 0);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
        }
        else if (swordDir == 1)
        {
            newSword.transform.Rotate(0, 0, 180);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.up * -thrustPower);
        }
        else if (swordDir == 2)
        {
            newSword.transform.Rotate(0, 0, 90);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -thrustPower);
        }
        else if (swordDir == 3)
        {
            newSword.transform.Rotate(0, 0, -90);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
        }
        #endregion
        
    }

    void Movement(){
        if (!canMove)
            return;
		if(Input.GetKey(KeyCode.UpArrow)){
			transform.Translate(0, speed * Time.deltaTime, 0);
            anim.SetInteger("dir", 0);
            anim.speed = 1;
		}
        else if (Input.GetKey(KeyCode.DownArrow)){
            transform.Translate(0, -speed * Time.deltaTime, 0);
            anim.SetInteger("dir", 1);
            anim.speed = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow)){
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", 2);
            anim.speed = 1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", 3);
            anim.speed = 1;
        }
        else
        {
            anim.speed = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(currentHealth <= 0)
        {
            SceneManager.LoadScene(0);
        }
        if(collision.tag == "EnemyBullet")
        {
            if (!iniFrames)
            {
                iniFrames = true;
                currentHealth--;
            }
            collision.gameObject.GetComponent<Bullet>().CreateParticle();
            Destroy(collision.gameObject);
        }
        if(collision.tag == "Potion")
        {
            if(currentHealth >= 5)
            {
                return;
            }
            if(maxHealth < 5)
            {
                maxHealth++;
            }
            currentHealth = maxHealth;
            Destroy(collision.gameObject);
        }
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("maxHealth", maxHealth);
        PlayerPrefs.SetInt("currentHealth", currentHealth);
    }

    void LoadGame()
    {
        maxHealth = PlayerPrefs.GetInt("maxHealth");
        currentHealth = PlayerPrefs.GetInt("currentHealth");
    }

}
