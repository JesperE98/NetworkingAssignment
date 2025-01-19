using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RespawnTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPoint;

    private BoxCollider _collider;

    private void Start() {
        _collider = GetComponent<BoxCollider>();

        _collider.enabled   = true;
        _collider.isTrigger = true;
        _collider.size      = new Vector3(100, 5, 100);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            other.transform.localPosition = respawnPoint.position;
        }
    }
}
