using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ChatManager : NetworkBehaviour {
    public static ChatManager Singleton;

    [SerializeField] private ChatMessage chatMessagePrefab;
    [SerializeField] private CanvasGroup chatContent;
    [SerializeField] private TMP_InputField chatInputField;

    public string playerName;

    void Awake() {
        ChatManager.Singleton = this;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            SendChatMessage(chatInputField.text, playerName);
            chatInputField.text = "";
        }
    }

    public void SendChatMessage(string _message, string _fromWho = null) {
        if (string.IsNullOrEmpty(_message))
            return;
        
        string S = _fromWho + " : " + _message;
        SendChatMessageServerRpc(S);
    }

    void AddMessage(string msg) {
        ChatMessage CM = Instantiate(chatMessagePrefab, chatContent.transform);
        CM.SetText(msg);
    }

    [ServerRpc(RequireOwnership = false)]
    void SendChatMessageServerRpc(string msg) {
        RecieveChatMessageClientRpc(msg);
    }

    [ClientRpc]
    void RecieveChatMessageClientRpc(string msg) {
        ChatManager.Singleton.AddMessage(msg);
    }
}
