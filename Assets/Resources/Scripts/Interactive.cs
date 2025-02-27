﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace WasaaMP {
    public class Interactive : MonoBehaviourPun {

        private bool catchable = false ;
        private bool caught = false ;
        float transparencyValue;
        Color lastColor;
        bool objectHighlighted;
        float highlightingSpeed = 1f;
        Renderer objectRenderer;
        protected List<CursorTool> listCursors;
        protected List<CursorTool> listCursorsNotCaught;
        public int numberOfPlayersNeeded = 1;

        void Start()
        {
            transparencyValue = 1f;
            objectRenderer = GetComponent<Renderer>();
            lastColor = objectRenderer.material.color;
            listCursors = new List<CursorTool>();
            listCursorsNotCaught = new List<CursorTool>();
        }

        void Update () {
            
            if (caught && listCursors.Count >= numberOfPlayersNeeded) {
                gameObject.transform.position = averagePosition();
            }

            if (objectHighlighted && listCursorsNotCaught.Count >= numberOfPlayersNeeded && transparencyValue != 0) {
                transparencyValue = 0;
                objectRenderer.material.color = new Color(lastColor.r, lastColor.g, lastColor.b, transparencyValue);
                
            }

            else if (objectHighlighted && listCursorsNotCaught.Count < numberOfPlayersNeeded ) {
                transparencyValue -= highlightingSpeed*Time.deltaTime;
                if (transparencyValue < 0) 
                    {
                        highlightingSpeed *= -1;
                        transparencyValue = 0;
                    } else if (transparencyValue > 1) {
                        highlightingSpeed *= -1;
                        transparencyValue = 1;
                    }
                objectRenderer.material.color = new Color(lastColor.r, lastColor.g, lastColor.b, transparencyValue);
            }
        }

        public void startHighlight() {
            if (!objectHighlighted) {
                transparencyValue = 1f;}
            objectHighlighted = true;
        }

        public void stopHighlight() {
            objectHighlighted = false;
            transparencyValue = 1f;
        }

        [PunRPC] public void ShowCaught () {
            if (! caught) {
                var rb = GetComponent<Rigidbody> () ;
                rb.isKinematic = true ;
                objectRenderer.material.color = new Color(lastColor.r, lastColor.g, lastColor.b, 1);
                caught = true ;
                objectHighlighted = false;
                
            }
        }

        [PunRPC] public void ShowReleased () {
            if (caught) {
                var rb = GetComponent<Rigidbody> () ;
                rb.isKinematic = false ;
                objectRenderer.material.color = new Color(lastColor.r, lastColor.g, lastColor.b, 1);
                caught = false ;
                objectHighlighted = false;
            }
        }

        [PunRPC] public void ShowCatchable () {
            if (! caught) {
                if (! catchable) {
                    catchable = true ;
                    lastColor = objectRenderer.material.color;
                    objectHighlighted = true;
                }
            }
        }
        
        [PunRPC] public void HideCatchable () {
            if (! caught) {
                if (catchable) {
                    objectRenderer.material.color = new Color(lastColor.r, lastColor.g, lastColor.b, 1);
                    objectHighlighted = false;
                    catchable = false ;
                }
            }
        }

        [PunRPC] public void addCursorList(int cursorID) {
            CursorTool cursor = PhotonView.Find(cursorID).gameObject.GetComponent<CursorTool>();
            print("Add List " + cursor);
            if (!listCursors.Contains(cursor)) {
            listCursors.Add(cursor);
            }
        }

        [PunRPC] public void removeCursorList(int cursorID) {
            CursorTool cursor = PhotonView.Find(cursorID).gameObject.GetComponent<CursorTool>();
            print("Remove List" + cursor);
            listCursors.Remove(cursor);
        }

        [PunRPC] public void addCursorListNotCaught(int cursorID) {
            CursorTool cursor = PhotonView.Find(cursorID).gameObject.GetComponent<CursorTool>();
            print("Add List Not Caught " + cursor);
            if (!listCursorsNotCaught.Contains(cursor) && !listCursors.Contains(cursor)) {
            listCursorsNotCaught.Add(cursor);
            }
        }

        [PunRPC] public void removeCursorListNotCaught(int cursorID) {
            CursorTool cursor = PhotonView.Find(cursorID).gameObject.GetComponent<CursorTool>();
            print("Remove List Not Caught " + cursor);
            listCursorsNotCaught.Remove(cursor);
        }

        Vector3 averagePosition() {
            float x = 0;
            float y = 0;
            float z = 0;
            int n = listCursors.Count;
            foreach (CursorTool cursor in listCursors) {
                x += cursor.transform.position.x;
                y += cursor.transform.position.y;
                z += cursor.transform.position.z;
            }
            x /= n;
            y /= n;
            z /= n;
            return new Vector3(x, y, z);
        }

    }

}
