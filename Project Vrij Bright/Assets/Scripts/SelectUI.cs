using UnityEngine;
using UnityEngine.EventSystems;

public class SelectUI : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool buttonSelected;
    
    void Update()
    {
        if (Input.GetAxis("HorizontalJ1") != 0 && buttonSelected == false || Input.GetAxisRaw("HorizontalJ2") != 0 && buttonSelected == false)
        {
            Debug.Log("Menu");
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
    }

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
