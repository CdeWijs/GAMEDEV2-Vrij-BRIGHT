using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// script checks player health and  
/// </summary>
public class BoyClass : MonoBehaviour {

    public int health;
    public int attackDamage = 5;

    //temporary slider ui
    public Slider playerHealthSlider;

    public static bool boyIsScared = false;

    private void Start() {
        playerHealthSlider.value = 100;
    }

    private void Update() {
        playerHealthSlider.value = health;

        if (health <= 0) {
            //Destroy(this.gameObject);
        }
    }

    //Change player properties on trigger enter
    private void OnTriggerEnter2D(Collider2D collision){
        //behaviour for when player is scared
        if (collision.tag == "Shadow"){
            boyIsScared = true;
            PhysicsScript.ChangeSpeedBoy(this.gameObject, BoyController.NormalSpeed * 0.25f);
        }


        else if (collision.tag == "GravityWell"){
            PhysicsScript.GravityIncrease(this.gameObject, 0.5f, 2f);
            PhysicsScript.ChangeSpeedBoy(this.gameObject, BoyController.NormalSpeed * 0.45f);
        }
    }

    //Restore player properties on trigger exit 
    private void OnTriggerExit2D(Collider2D collision){
        if (collision.transform.tag == "Shadow"){
            boyIsScared = false;
            PhysicsScript.ChangeSpeedBoy(this.gameObject, BoyController.NormalSpeed);
        }

        else if (collision.tag == "GravityWell"){
            PhysicsScript.ResetGravity(this.gameObject);
            PhysicsScript.ChangeSpeedBoy(this.gameObject, BoyController.NormalSpeed);
        }
    }
}