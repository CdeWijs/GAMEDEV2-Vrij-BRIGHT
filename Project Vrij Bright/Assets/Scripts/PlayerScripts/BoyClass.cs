using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// script checks player health and  
/// </summary>
public class BoyClass : MonoBehaviour {

    public int health;
    public int attackDamage = 50;

    //temporary slider ui
    public Slider playerHealthSlider;

    public static bool boyIsScared = false;

    private void Start() {
        playerHealthSlider.value = 100;
    }

    private void Update() {
        playerHealthSlider.value = health;

        if (health <= 0) {
            Destroy(this.gameObject);
        }
    }

   
}