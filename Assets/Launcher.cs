﻿using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace WasaaMP {

    public class Launcher : MonoBehaviourPunCallbacks {

    
        #region Private Serializable Fields

        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool isConnecting;

        #endregion

        #region Private Fields

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";

        #endregion

        #region Public Fields

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake () {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true ;
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start () {
            // Connect(); // remplacé par un appel de connect suite à un click sur le bouton de connexion
            progressLabel.SetActive (false) ;
            controlPanel.SetActive (true) ;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect () {
            // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
            isConnecting = true ;
            progressLabel.SetActive(true) ;
            controlPanel.SetActive(false) ;
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected) {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                Debug.Log ("PhotonNetwork.IsConnected : true, PhotonNetwork.JoinRandomRoom()") ;
                PhotonNetwork.JoinRandomRoom () ;
            } else {
                // #Critical, we must first and foremost connect to Photon Online Server.
                Debug.Log ("PhotonNetwork.IsConnected : false, PhotonNetwork.ConnectUsingSettings()") ;
                PhotonNetwork.GameVersion = gameVersion ;
                PhotonNetwork.ConnectUsingSettings () ;
                //PhotonNetwork.ConnectToMaster("localhost", 5055, "ff3bcfc7-ff4e-4689-8acc-7532edc10870");
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster () {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            // we don't want to do anything if we are not attempting to join a room.
            // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
            // we don't want to do anything.
            if (isConnecting) {
                Debug.Log ("isConnecting is true : PhotonNetwork.JoinRandomRoom()") ;
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom () ;
                //isConnecting = false ;
            }
        }

        public override void OnDisconnected (DisconnectCause cause) {
            progressLabel.SetActive (false) ;
            controlPanel.SetActive (true) ; 
            Debug.LogWarningFormat ("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause) ;
        }

        public override void OnJoinRandomFailed (short returnCode, string message) {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            // PhotonNetwork.CreateRoom(null, new RoomOptions());
            PhotonNetwork.CreateRoom (null, new RoomOptions { MaxPlayers = maxPlayersPerRoom }) ;
        }

        public override void OnJoinedRoom () {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
                Debug.Log ("We load the arena") ;
                // #Critical
                // Load the Room Level.
                PhotonNetwork.LoadLevel ("Arena") ;
            }
        }

        #endregion
    }
}