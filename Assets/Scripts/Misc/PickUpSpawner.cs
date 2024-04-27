using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] GameObject goldCoin, healthGlobe, staminaGlobe;

    public void DropItems()
    {
        int randomNum = Random.Range(1,5);

        switch(randomNum)
        {
            case 1:
                int randomAmount = Random.Range(1,4);
                for (int i = 0; i < randomAmount; i++)
                {
                    Instantiate(goldCoin, transform.position, Quaternion.identity);
                }
                break;
            case 2:
                Instantiate(healthGlobe, transform.position, Quaternion.identity);
                break;
            case 3:
                Instantiate(staminaGlobe, transform.position, Quaternion.identity);
                break;
            default:
                break;
        }
        
        
    }
}
