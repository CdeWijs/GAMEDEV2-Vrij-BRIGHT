using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject buttonImage;
    public GameObject other;

    // FMOD
    [FMODUnity.EventRef]
    public string eventRef;
    private FMOD.Studio.EventInstance instance;

    private void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(eventRef);
    }

    public void SetButtonActive(bool isActive)
    {
        buttonImage.SetActive(isActive);
    }

    public void Teleport(GameObject other)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(eventRef, this.gameObject);
        instance.start();
        Debug.Log(other);
        other.transform.position = new Vector3(
             this.other.transform.position.x,
            other.transform.position.y,
            other.transform.position.z);
    }

    public void Teleport()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(eventRef, this.gameObject);
        instance.start();
        Debug.Log(other);
        other.transform.position = new Vector3(
             this.other.transform.position.x,
            other.transform.position.y,
            other.transform.position.z);
    }
}

