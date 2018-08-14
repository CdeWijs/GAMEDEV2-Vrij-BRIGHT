using System.Collections;
using UnityEngine;

public class BaitScript : MonoBehaviour
{
    public GameObject buttonImage;
    public bool baitOnGround;

    private Rigidbody2D rigidBody2D;
    private GameObject player;
    private GuardianController guardianController;

    // FMOD
    [FMODUnity.EventRef]
    public string cutBaitRef;
    private FMOD.Studio.EventInstance cutBaitInstance;
    [FMODUnity.EventRef]
    public string dropBaitRef;
    private FMOD.Studio.EventInstance dropBaitInstance;

    private void Start()
    {
        baitOnGround = false;
        player = GameObject.FindGameObjectWithTag("Guardian");
        guardianController = player.GetComponent<GuardianController>();
        Debug.Log(guardianController);
        cutBaitInstance = FMODUnity.RuntimeManager.CreateInstance(cutBaitRef);
        dropBaitInstance = FMODUnity.RuntimeManager.CreateInstance(dropBaitRef);
    }

    public IEnumerator DropBait()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(cutBaitRef, this.gameObject);
        if (GetComponent<Rigidbody2D>() == null)
        {
            rigidBody2D = gameObject.AddComponent<Rigidbody2D>();
        }
        else
        {
            rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        }
        if (GetComponent<BoxCollider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
        rigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        yield return new WaitForSeconds(1);

        FMODUnity.RuntimeManager.PlayOneShotAttached(dropBaitRef, this.gameObject);
        baitOnGround = true;

        yield return null;
    }

    public void SetButtonActive(bool isActive)
    {
        buttonImage.SetActive(isActive);
    }
}
