using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDarkness : MonoBehaviour {

    float animTimer = .15f;
    float timer = 1.5f;
    public GameObject darknessParticle;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        animTimer -= Time.deltaTime;
        if (animTimer <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetInteger("attackDir", 5);
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            CreateParticle();
            Destroy(gameObject);
        }
    }

    public void CreateParticle()
    {
        Instantiate(darknessParticle, transform.position, transform.rotation);
    }
}
