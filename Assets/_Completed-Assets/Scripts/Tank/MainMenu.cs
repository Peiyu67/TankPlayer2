using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tanks
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        public static MainMenu instance;
        private GameObject m_ui;
        private Button m_joinGameButton;
        public InputField namee, room;

        void Awake()
        {
            if (instance != null)
            {

                DestroyImmediate(gameObject);
                return;
            }

            instance = this;
            m_ui = transform.FindAnyChild<Transform>("UI").gameObject;
            m_joinGameButton = transform.FindAnyChild<Button>("JoinGameButton");
            m_ui.SetActive(true);
            m_joinGameButton.interactable = false;
        }

        public override void OnConnectedToMaster()
        {
            m_joinGameButton.interactable = true;
        }

        public override void OnEnable()
        {
            // Always call the base to add callbacks
            base.OnEnable();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable();

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            m_ui.SetActive(!PhotonNetwork.InRoom);
        }

        public void ClearText()
        {
            namee.text = "";
            room.text = "";
        }

        public void ShowMenu(bool b)
        {
            m_ui.SetActive(b);
        }
    }
}