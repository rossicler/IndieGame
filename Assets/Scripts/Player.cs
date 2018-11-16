using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Player : MonoBehaviour {

	public float speed;
    Animator anim;
    public Image[] hearts;
    public Image skillImg;
    public Text skillText;
    private string skillName;
    public int maxHealth;
    public int currentHealth;
    public GameObject sword;
    GameObject skill;
    public float thrustPower;
    float thrustPowerDarkness;
    int skillCharges;
    public bool canMove;
    public bool canAttack;
    public bool iniFrames;
    SpriteRenderer sr;
    float iniTimer = 1f;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "InitialScene")
        {
            maxHealth = 2;
            currentHealth = maxHealth;
            skillCharges = 0;
        } else if (PlayerPrefs.HasKey("maxHealth") && PlayerPrefs.HasKey("currentHealth"))
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
	
	// Update is called once per frame
	void Update () {
        Movement();
        if (Input.GetKeyDown(KeyCode.A))
        {
            Attack();
        }
        if(Input.GetKeyDown(KeyCode.S) && skillCharges > 0)
        {
            Skill();
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

    void getHealth()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < currentHealth; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
    }
    void skillUI()
    {
        skillText.text = "x" + skillCharges.ToString();
    }

    void skillUI(Sprite skillSpriteTemp)
    {
        skillImg.sprite = skillSpriteTemp;
        skillUI();
        skillImg.gameObject.SetActive(true);
        skillText.gameObject.SetActive(true);
    }

    void skillLoad()
    {
        string path = "Skills/Sprites/" + skillName;
        // Sprite skillSprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
        Sprite skillSprite = Resources.Load<Sprite>(path);
        skillUI(skillSprite);
    }

    void Skill()
    {
        if (!canAttack)
        {
            return;
        }
        skillCharges--;
        skillUI();
        if (skillCharges <= 0)
        {
            skillImg.gameObject.SetActive(false);
            skillText.gameObject.SetActive(false);
        }
        canMove = true;
        canAttack = false;
        thrustPowerDarkness = 650;
        // skill = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/SkillDarkness.prefab", typeof(GameObject));
        skill = Resources.Load<GameObject>("Skills/Prefabs/SkillDarkness");
        GameObject newDarkness = Instantiate(skill, transform.position, skill.transform.rotation);

        #region SkillRotation
        int skillDir = anim.GetInteger("dir");
        anim.SetInteger("attackDir", skillDir);
        if (skillDir == 0)
        {
            newDarkness.transform.Rotate(0, 0, 0);
            newDarkness.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPowerDarkness);
        }
        else if (skillDir == 1)
        {
            newDarkness.transform.Rotate(0, 0, 180);
            newDarkness.GetComponent<Rigidbody2D>().AddForce(Vector2.up * -thrustPowerDarkness);
        }
        else if (skillDir == 2)
        {
            newDarkness.transform.Rotate(0, 0, 90);
            newDarkness.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -thrustPowerDarkness);
        }
        else if (skillDir == 3)
        {
            newDarkness.transform.Rotate(0, 0, -90);
            newDarkness.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPowerDarkness);
        }
        #endregion
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

    void checkHealth()
    {
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        checkHealth();
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
        if(collision.tag == "PotionSmallHP")
        {
            if(currentHealth >= 3)
            {
                return;
            }
            if(maxHealth < 3)
            {
                maxHealth++;
            }
            currentHealth++;
            Destroy(collision.gameObject);
        }
        if (collision.tag == "PotionMediumHP")
        {
            if (currentHealth >= 4)
            {
                return;
            }
            if (maxHealth < 4)
            {
                maxHealth++;
            }
            currentHealth = maxHealth;
            Destroy(collision.gameObject);
        }
        if (collision.tag == "PotionFullHP")
        {
            if (currentHealth >= 5)
            {
                return;
            }
            if (maxHealth < 5)
            {
                maxHealth++;
            }
            currentHealth = maxHealth;
            Destroy(collision.gameObject);
        }
        if (collision.tag == "BookSkillDarkness")
        {
            //Logic to create the skill with full charges
            skillName = "Darkness";
            skillLoad();
            skillCharges = 10;
            skillUI();
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            checkHealth();
            if (!iniFrames)
            {
                currentHealth--;
                iniFrames = true;
            }
        }
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("maxHealth", maxHealth);
        PlayerPrefs.SetInt("currentHealth", currentHealth);
        if(skillCharges > 0)
        {
            PlayerPrefs.SetInt("skillCharges", skillCharges);
            PlayerPrefs.SetString("skillName", skillName);
        } else
        {
            PlayerPrefs.SetInt("skillCharges", 0);
            PlayerPrefs.SetString("skillName", "");
        }
        
    }

    void LoadGame()
    {
        maxHealth = PlayerPrefs.GetInt("maxHealth");
        currentHealth = PlayerPrefs.GetInt("currentHealth");
        if (PlayerPrefs.HasKey("skillCharges"))
        {
            skillCharges = PlayerPrefs.GetInt("skillCharges");
            skillName = PlayerPrefs.GetString("skillName");
            skillLoad();
        }
    }

}
