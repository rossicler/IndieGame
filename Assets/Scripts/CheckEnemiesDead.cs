using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemiesDead : MonoBehaviour {

    List<GameObject> listOfEnemies = new List<GameObject>();
    public GameObject[] items;
    public Transform[] itemsPositions;

    // Use this for initialization
    void Start () {
        listOfEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));	
	}
	
    public void KilledEnemy(GameObject enemy)
    {
        if (listOfEnemies.Contains(enemy))
        {
            listOfEnemies.Remove(enemy);
        }
    }

    public void AreEnemiesDead()
    {
        if(listOfEnemies.Count <= 0)
        {
            // all are dead
            for (int i = 0; i < items.Length; i++)
            {
                Instantiate(items[i], itemsPositions[i].position, itemsPositions[i].rotation);
            }
        }
    }
	
}
