using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WasaaMP {
	public class CursorTool : MonoBehaviourPun {
		private bool caught ;
		public Interactive cyanSphere;
		public Interactive magentaSphere;
		public Interactive yellowSphere;
		public Interactive cyanCube;
		public Interactive magentaCube;
		public Interactive yellowCube;
		private Interactive target ;
		private MonoBehaviourPun targetParent ;
		private Transform oldParent = null ;
		void Start () {
			caught = false ;
		}

		void Update() {
			
		}
		
		public void Catch () {
			//print ("Catch ?") ;
			if (target != null) {
				//print ("Catch :") ;
				if ((! caught) && (transform != target.transform)) { // pour ne pas prendre 2 fois l'objet et lui faire perdre son parent
					oldParent = target.transform.parent ;
					//target.transform.SetParent (transform) ;
					target.photonView.TransferOwnership (PhotonNetwork.LocalPlayer) ;
					target.photonView.RPC ("ShowCaught", RpcTarget.All) ;
					target.photonView.RPC ("addCursorList", RpcTarget.All, gameObject.GetComponent<PhotonView>().ViewID);
					target.photonView.RPC("removeCursorListNotCaught", RpcTarget.All, gameObject.GetComponent<PhotonView>().ViewID);
					PhotonNetwork.SendAllOutgoingCommands () ;
					caught = true ;
					
				}
				//print ("Catch !") ;
			} else {
				//print ("Catch failed") ;
			}
		}

		public void Release () {
			if (caught) {
				//print ("Release :") ;
				//target.transform.SetParent (oldParent) ;
				target.photonView.RPC ("ShowReleased", RpcTarget.All) ;
				target.photonView.RPC ("removeCursorList", RpcTarget.All,  gameObject.GetComponent<PhotonView>().ViewID);
				PhotonNetwork.SendAllOutgoingCommands () ;
				//print ("Release !") ;
				caught = false ;
				
			}
		}

		public void CreateInteractiveObject () {
			int objectIndex = Random.Range(0, 6);
			int nbPlayer = Random.Range(1, 3);
			if (objectIndex == 0) {
				var objectToInstanciate = PhotonNetwork.Instantiate (cyanCube.name, transform.position, transform.rotation, 0) ;
				objectToInstanciate.GetComponent<Interactive>().numberOfPlayersNeeded = nbPlayer;
			} else if (objectIndex == 1) {
				var objectToInstanciate = PhotonNetwork.Instantiate (magentaCube.name, transform.position, transform.rotation, 0) ;
				objectToInstanciate.GetComponent<Interactive>().numberOfPlayersNeeded = nbPlayer;
			} else if (objectIndex == 2) {
				var objectToInstanciate = PhotonNetwork.Instantiate (yellowCube.name, transform.position, transform.rotation, 0) ;
				objectToInstanciate.GetComponent<Interactive>().numberOfPlayersNeeded = nbPlayer;
			} else if (objectIndex == 3) {
				var objectToInstanciate = PhotonNetwork.Instantiate (cyanSphere.name, transform.position, transform.rotation, 0) ;
				objectToInstanciate.GetComponent<Interactive>().numberOfPlayersNeeded = nbPlayer;
			} else if (objectIndex == 4) {
				var objectToInstanciate = PhotonNetwork.Instantiate (magentaSphere.name, transform.position, transform.rotation, 0) ;
				objectToInstanciate.GetComponent<Interactive>().numberOfPlayersNeeded = nbPlayer;
			} else if (objectIndex == 5) {
				var objectToInstanciate = PhotonNetwork.Instantiate (yellowSphere.name, transform.position, transform.rotation, 0) ;
				objectToInstanciate.GetComponent<Interactive>().numberOfPlayersNeeded = nbPlayer;
			} else {
				var objectToInstanciate = PhotonNetwork.Instantiate (cyanCube.name, transform.position, transform.rotation, 0) ;
				objectToInstanciate.GetComponent<Interactive>().numberOfPlayersNeeded = nbPlayer;
			}
			
			
			
		}

		void OnTriggerEnter (Collider other) {
			if (! caught) {
				//print (name + " : CursorTool OnTriggerEnter") ;
				target = other.gameObject.GetComponent<Interactive> () ;
				if (target != null) {
					target.startHighlight();
					target.photonView.RPC ("ShowCatchable", RpcTarget.All) ;
					target.photonView.RPC("addCursorListNotCaught", RpcTarget.All, gameObject.GetComponent<PhotonView>().ViewID);
					PhotonNetwork.SendAllOutgoingCommands () ;
				}
			}
		}


		void OnTriggerExit (Collider other) {
			if (! caught) {
				//print (name + " : CursorTool OnTriggerExit") ;
				if (target != null) {
					target.stopHighlight();
					target.photonView.RPC ("HideCatchable", RpcTarget.All) ;
					target.photonView.RPC("removeCursorListNotCaught", RpcTarget.All, gameObject.GetComponent<PhotonView>().ViewID);
					PhotonNetwork.SendAllOutgoingCommands () ;
					target = null ;
				}
		}
	}
}
}