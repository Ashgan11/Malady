using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    private float currentHealth, liquidHealth;

    public float liquidDecayDelay = 2;
    private float currentLiquidDecayTimer;
    private bool decayInProgress = false;

    public float liquidDecaySpeed = 5;

    public Slider healthSlider, liquidHealthSlider;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = liquidHealth = maxHealth;
        currentLiquidDecayTimer = liquidDecayDelay;        
    }

    // Update is called once per frame
    void Update()
    {
        updateSliders();

        //Tick Timer & Liquid Health Decay
        if (decayInProgress && currentLiquidDecayTimer < liquidDecayDelay) currentLiquidDecayTimer += Time.deltaTime;
        else if (liquidHealth > currentHealth) {
            liquidHealth -= liquidDecaySpeed * Time.deltaTime;
            liquidHealth = Mathf.Clamp(liquidHealth, currentHealth, maxHealth);
            if (liquidHealth == currentHealth) decayInProgress = false;
        }

        //Test Input
        /*if (Input.GetKeyDown(KeyCode.X)) takeDamage(10);
        if (Input.GetKeyDown(KeyCode.Y)) liquidHeal(10);
        if (Input.GetKeyDown(KeyCode.C)) trueHeal(10);
        if (isDead()) Debug.Log("Is dead!");*/
    }

    void updateSliders(){
        healthSlider.value = currentHealth / maxHealth;
        liquidHealthSlider.value = liquidHealth / maxHealth;
    }

    bool isDead() {
        return (currentHealth == 0 && liquidHealth == 0);
    }

    void startDecayTimer(){
        if (!decayInProgress) {
            currentLiquidDecayTimer = 0;
            decayInProgress = true;
        }
    }

    void readjustHealth() {
        if (currentHealth >= liquidHealth) {
            liquidHealth = currentHealth;
            decayInProgress = false;
            currentLiquidDecayTimer = liquidDecayDelay + 1;
        }
    }

    //Public Functions
    public void takeDamage(float damage){
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);        
        startDecayTimer();
    }

    public void liquidHeal(float value) {
        currentHealth = Mathf.Clamp(currentHealth + value, currentHealth, liquidHealth);
        readjustHealth();
    }

    public void trueHeal(float value) {
        currentHealth = Mathf.Clamp(currentHealth + value, currentHealth, maxHealth);
        readjustHealth();
    }
}
