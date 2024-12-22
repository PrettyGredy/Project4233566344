using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using VInspector;

public class ServerManager : MonoBehaviour
{
    [SerializeField] private string apiUrl;
    [SerializeField] private string authToken;
    
    private void Start()
    {
        GlobalManager.Instance.InventoryManager.OnItemInventoryAdded.AddListener(SendItemEventSC);
        GlobalManager.Instance.InventoryManager.OnItemInventoryRemoved.AddListener(SendItemEventSC);
    }

    private void OnDisable()
    {
        GlobalManager.Instance.InventoryManager.OnItemInventoryAdded.RemoveListener(SendItemEventSC);
        GlobalManager.Instance.InventoryManager.OnItemInventoryRemoved.RemoveListener(SendItemEventSC);
    }

    private void SendItemEventSC(string ID, string eventType)
    {
        StartCoroutine(SendItemEvent(ID, eventType));
    }

    private IEnumerator SendItemEvent(string item, string eventType)
    {
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        request.SetRequestHeader("Content-Type", "application/json");

        string json = JsonUtility.ToJson(new InventoryEvent(item, eventType));
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Server response: {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }
}