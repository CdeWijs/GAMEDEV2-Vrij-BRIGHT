using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// script checks player health and  
/// </summary>
public class BoyClass : MonoBehaviour
{
    public int health;
    public int attackDamage = 50;
    public string reloadScene;
    public string nextScene;
    public Color loadToColor = Color.black;

    //temporary slider ui
    public Slider playerHealthSlider;

    public static bool boyIsScared = false;

    private void Start()
    {
        playerHealthSlider.value = 100;
    }

    private void Update()
    {
        playerHealthSlider.value = health;

        if (health <= 0)
        {
            Initiate.Fade(reloadScene, loadToColor, 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Door")
        {
            Initiate.Fade(nextScene, loadToColor, 0.5f);
        }
    }
}