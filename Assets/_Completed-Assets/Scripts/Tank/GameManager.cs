using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Tanks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks

{
    public static GameManager instance;  //將GameManager宣告單例
    public static GameObject localPlayer;
    public static GameObject PlayerName;
    string gameVersion = "1";  //版本設置為1
    private string roomName;
    private string Name;


    private GameObject defaultSpawnPoint;

    void Awake()
    {
        if (instance != null)  //若場景上已存在便消除這個物件
        {
            Debug.LogErrorFormat(gameObject,"Multiple instances of {0} is not allow", GetType().Name);
            DestroyImmediate(gameObject);
            return;
        }
        PhotonNetwork.AutomaticallySyncScene = true;  //設置了自動同步場景
        DontDestroyOnLoad(gameObject);  //不會被消除
        instance = this;

        defaultSpawnPoint = new GameObject("Default SpawnPoint");
        defaultSpawnPoint.transform.position = new Vector3(0, 0, 0);
        defaultSpawnPoint.transform.SetParent(transform, false);
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();  //與 Photon Cloud 連線
        PhotonNetwork.GameVersion = gameVersion;  //設置遊戲版本
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    public override void OnConnected()  //已連線上了
    {
        Debug.Log("PUN Connected");
    }

    public override void OnConnectedToMaster()  //連線到Master
    {
        Debug.Log("PUN Connected to Master");
    }

    public override void OnDisconnected(DisconnectCause cause)  //斷線原因
    {
        Debug.LogWarningFormat("PUN Disconnected was called by PUN with reason {0}", cause);
    }

    public void JoinGameRoom( string roomname)  //觸發按鈕時，創建一個房間
    {
        roomname = roomName;
        var options = new RoomOptions  //設定最大容納人數
        {
            MaxPlayers = 6
        };        
        PhotonNetwork.JoinOrCreateRoom(roomname, options, null);  //(房間名,設置,房間類型)
    }

    public override void OnJoinedRoom()  //若連線成功執行
    {
        if (PhotonNetwork.IsMasterClient)  //如果是第一個創建房間的將成為Server
        {
            Debug.Log("Created"+ roomName + "room!!");
            PhotonNetwork.LoadLevel("GameScene");  //載GameScenes場景
        }
        else
        {
            Debug.Log(Name+"Joined room!!");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!PhotonNetwork.InRoom)
        {
            MainMenu.instance.ShowMenu(true);
            DeadMenu.instance.ShowDead(false);
            return;
        }
        var spawnPoint = GetRandomSpawnPoint();
        localPlayer = PhotonNetwork.Instantiate("TankPlayer", spawnPoint.position, spawnPoint.rotation, 0);
        
        localPlayer.GetComponentInChildren<Text>().text = Name;
        GameObject.Find("房間名稱2").GetComponent<Text>().text = roomName;
    }

    public static List<GameObject> GetAllObjectsOfTypeInScene<T>()
    {
        var objectsInScene = new List<GameObject>();

        foreach (var go in (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {
            if (go.hideFlags == HideFlags.NotEditable ||
            go.hideFlags == HideFlags.HideAndDontSave)
                continue;

            if (go.GetComponent<T>() != null)
                objectsInScene.Add(go);
        }

        return objectsInScene;
    }
    private Transform GetRandomSpawnPoint()
    {
        var spawnPoints = GetAllObjectsOfTypeInScene<SpawnPoint>();
        return spawnPoints.Count == 0
        ? defaultSpawnPoint.transform
        : spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
    }

    public void SetName()
    {
        Name = GameObject.Find("Name").GetComponent<Text>().text;
    }

    public void SetRoomName()
    {
        roomName = GameObject.Find("Room").GetComponent<Text>().text;
    }

    public void DisconnectPlayer()
    {
        MainMenu.instance.ClearText();
        StartCoroutine(DisconnectAndLoaded());
    }
    IEnumerator DisconnectAndLoaded()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;
        PhotonNetwork.LoadLevel("MainScene");
    }
}
