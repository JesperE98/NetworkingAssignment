using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class CameraController : NetworkBehaviour {
    public GameObject cameraHolder;
    
    public void Update() {
        if (IsOwner) {
            cameraHolder.SetActive(true);
        }
        else cameraHolder.SetActive(false);
    }
}
