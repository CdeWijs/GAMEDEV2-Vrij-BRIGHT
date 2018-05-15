using UnityEngine;

public class BoyController : BaseController {

    public const float NormalSpeed = 8;
    public const float ScaredSpeed = 2;
    public const float JumpForce = 5;

    public override void Start() {
        base.Start();

        // Check if Joystick exists
        if (Input.GetJoystickNames().Length > 0) {
            connectedController = new Joystick1();
        }
    }

    public override void Update() {
        base.Update();
    }

    public override void FixedUpdate() {
        base.FixedUpdate();

        if (BoyClass.boyIsScared == true) {
            MoveHorizontally(ScaredSpeed);
        } else {
            MoveHorizontally(NormalSpeed);
        }
    }

    public override void GetInput() {
        base.GetInput();

        if (connectedController != null) {
            if (a_active && grounded) {
                Jump(JumpForce);
            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.Space) && grounded) {
                Jump(JumpForce);
            }
        }
    }

    private void BasicAttack(Collider2D collision) {
        EnemyBaseClass _enemyScript = collision.GetComponent<EnemyBaseClass>();
        int _damage = GetComponent<BoyClass>().attackDamage;
        _enemyScript.TakeDamage(_damage);
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
