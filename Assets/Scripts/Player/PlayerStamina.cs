using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStamina : Singleton<PlayerStamina>
{
    int startingStamina = 3;
    int maxStamina;
    public int CurrentStamina{get; private set;}
    [SerializeField] Sprite fullStaminaImage, emptyStaminaImage;
    [SerializeField] float timeBetweenStaminaRefresh = 3f;
    Transform staminaContainer;
    const string STAMINA_CONTAINER_TEXT = "Stamina Container";
    protected override void Awake() {
        base.Awake();
        maxStamina = startingStamina;
        CurrentStamina = maxStamina;
    }

    private void Start() {
        staminaContainer = GameObject.Find(STAMINA_CONTAINER_TEXT).transform;
    }

    public void UseStamina()
    {
        CurrentStamina--;
        UpdateStaminaImage();
    }

    public void RefreshStamina()
    {
        if(CurrentStamina < maxStamina)
        {
            CurrentStamina++;
            UpdateStaminaImage();
        }
    }

    private void UpdateStaminaImage()
    {
        for (int i = 0; i < maxStamina; i++)
        {
            if(i < CurrentStamina)
            {
                staminaContainer.GetChild(i).GetComponent<Image>().sprite = fullStaminaImage;
            }
            else
            {
                staminaContainer.GetChild(i).GetComponent<Image>().sprite = emptyStaminaImage;
            }
        }

        if(CurrentStamina < maxStamina)
        {
            StopAllCoroutines();
            StartCoroutine(RefreshStaminaRoutine());
        }
    }

    private IEnumerator RefreshStaminaRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            RefreshStamina();
        }
    }
}
