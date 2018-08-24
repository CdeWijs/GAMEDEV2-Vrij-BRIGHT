using UnityEngine;

/// <summary>
/// class to drop cage when rope is hit
/// </summary>
public class CageScript : MonoBehaviour
{
    public GameObject cage;

    public void DropCage()
    {
        if (cage.gameObject != null)
        {
            cage.gameObject.AddComponent<Rigidbody2D>();
        }
    }
}
