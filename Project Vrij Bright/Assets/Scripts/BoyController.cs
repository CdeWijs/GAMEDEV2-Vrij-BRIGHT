using UnityEngine;

public class BoyController : BaseController {

    public override void Start() {
        base.Start();

        // Check if Joystick exists
        if (Input.GetJoystickNames().Length > 0) {
            connectedController = new Joystick1();
        }
    }

    private void BasicAttack(Collider2D collision) {
        EnemyBaseClass enemyScript = collision.GetComponent<EnemyBaseClass>();
        int damage = GetComponent<BoyClass>().attackDamage;
        enemyScript.TakeDamage(GetComponent<BoyClass>().attackDamage);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (x_active || Input.GetKeyDown(KeyCode.E)) {
            if (collision.tag == "Monster") {
                Debug.Log("attack");
                BasicAttack(collision);
            }
        }
    }
}
