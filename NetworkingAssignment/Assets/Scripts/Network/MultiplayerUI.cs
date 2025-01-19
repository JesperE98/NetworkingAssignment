using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerUI : MonoBehaviour {
    
    [SerializeField] private Button hostBtn, joinBtn;

    void Awake() {
        AssignInputs();
    }

    void AssignInputs() {
        hostBtn.onClick.AddListener( delegate {NetworkManager.Singleton.StartHost(); });
        joinBtn.onClick.AddListener( delegate {NetworkManager.Singleton.StartClient(); });
    }
}
