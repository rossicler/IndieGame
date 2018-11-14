using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour {

    Animator anim;
    public float speed;
    int dir;
    float dirTimer = .7f;
    public int health;
    public GameObject deathParticle;
    bool canAttack;
    float attackTimer = 2f;
    public GameObject projectile;
    public float thrustPower;
    float changeTimer = .2f;
    bool shouldChange;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        dir = Random.Range(0, 3);
        canAttack = false;
        shouldChange = false;
	}
	
	// Update is called once per frame
	void Update () {
        dirTimer -= Time.deltaTime;
        if(dirTimer <= 0)
        {
            dirTimer = .7f;
            dir = Random.Range(0, 3);
        }
        Movement();
        attackTimer -= Time.deltaTime;
        if(attackTimer <= 0)
        {
            attackTimer = 2f;
            canAttack = true;
        }
        Attack();
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

    void Attack()
    {
        if (!canAttack)
        {
            return;
        }
        canAttack = false;
        if(dir == 0)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
        } else if(dir == 1)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.left * thrustPower);
        } else if(dir == 2)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.down * thrustPower);
        } else if(dir == 3){
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
        } else if(dir == 1)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", dir);
        } else if(dir == 2)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            anim.SetInteger("dir", dir);
        } else if(dir == 3)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", dir);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Sword")
        {
            health--;
            col.gameObject.GetComponent<Sword>().CreateParticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
            Destroy(col.gameObject);
            checkHealth();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            health--;
            checkHealth();
        }
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

    void checkHealth()
    {
        if(health <= 0)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CheckEnemiesDead>().KilledEnemy(gameObject);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CheckEnemiesDead>().AreEnemiesDead();
            Instantiate(deathParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }


}
