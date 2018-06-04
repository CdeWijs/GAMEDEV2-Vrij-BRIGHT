using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {

    public GameObject buttonImage;
    public GameObject other;
    public string _tag;

    public void SetButtonActive(bool _bool) {
        buttonImage.SetActive(_bool);

    }

    public void Teleport(GameObject _player) {

        _player.transform.position = new Vector3(
             other.transform.position.x,
            _player.transform.position.y,
            _player.transform.position.z);
    }
}

