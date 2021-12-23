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
    public static GameManager instance;  //�NGameManager�ŧi���
    public static GameObject localPlayer;
    public static GameObject PlayerName;
    string gameVersion = "1";  //�����]�m��1
    private string roomName;
    private string Name;


    private GameObject defaultSpawnPoint;

    void Awake()
    {
        if (instance != null)  //�Y�����W�w�s�b�K�����o�Ӫ���
        {
            Debug.LogErrorFormat(gameObject,"Multiple instances of {0} is not allow", GetType().Name);
            DestroyImmediate(gameObject);
            return;
        }
        PhotonNetwork.AutomaticallySyncScene = true;  //�]�m�F�۰ʦP�B����
        DontDestroyOnLoad(gameObject);  //���|�Q����
        instance = this;

        defaultSpawnPoint = new GameObject("Default SpawnPoint");
        defaultSpawnPoint.transform.position = new Vector3(0, 0, 0);
        defaultSpawnPoint.transform.SetParent(transform, false);
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();  //�P Photon Cloud �s�u
        PhotonNetwork.GameVersion = gameVersion;  //�]�m�C������
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    public override void OnConnected()  //�w�s�u�W�F
    {
        Debug.Log("PUN Connected");
    }

    public override void OnConnectedToMaster()  //�s�u��Master
    {
        Debug.Log("PUN Connected to Master");
    }

    public override void OnDisconnected(DisconnectCause cause)  //�_�u��]
    {
        Debug.LogWarningFormat("PUN Disconnected was called by PUN with reason {0}", cause);
    }

    public void JoinGameRoom( string roomname)  //Ĳ�o���s�ɡA�Ыؤ@�өж�
    {
        roomname = roomName;
        var options = new RoomOptions  //�]�w�̤j�e�ǤH��
        {
            MaxPlayers = 6
        };        
        PhotonNetwork.JoinOrCreateRoom(roomname, options, null);  //(�ж��W,�]�m,�ж�����)
    }

    public override void OnJoinedRoom()  //�Y�s�u���\����
    {
        if (PhotonNetwork.IsMasterClient)  //�p�G�O�Ĥ@�ӳЫةж����N����Server
        {
            Debug.Log("Created"+ roomName + "room!!");
            PhotonNetwork.LoadLevel("GameScene");  //��GameScenes����
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
        GameObject.Find("�ж��W��2").GetComponent<Text>().text = roomName;
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
