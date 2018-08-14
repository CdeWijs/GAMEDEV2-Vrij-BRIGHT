using UnityEngine;

public class DoorsController : MonoBehaviour
{

    private GameObject openDoors;
    private GameObject closedDoors;

    // FMOD
    [FMODUnity.EventRef]
    public string doorOpen;
    private FMOD.Studio.EventInstance instanceOpen;
    [FMODUnity.EventRef]
    public string doorClose;
    private FMOD.Studio.EventInstance instanceClose;


    private void Start()
    {
        openDoors = transform.GetChild(0).gameObject;
        openDoors.SetActive(false);
        closedDoors = transform.GetChild(1).gameObject;
        closedDoors.SetActive(true);

        instanceOpen = FMODUnity.RuntimeManager.CreateInstance(doorOpen);
        instanceClose = FMODUnity.RuntimeManager.CreateInstance(doorClose);
    }

    public void HandleDoors(bool _open)
    {
        if (_open)
        {
            openDoors.SetActive(true);
            closedDoors.SetActive(false);
            FMODUnity.RuntimeManager.PlayOneShotAttached(doorOpen, this.gameObject);
            instanceOpen.start();
        }
        else
        {
            openDoors.SetActive(false);
            closedDoors.SetActive(true);
            FMODUnity.RuntimeManager.PlayOneShotAttached(doorClose, this.gameObject);
            instanceClose.start();
        }
    }
}
