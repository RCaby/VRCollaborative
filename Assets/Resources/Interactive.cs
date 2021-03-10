using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WasaaMP {
    public class Interactive : MonoBehaviourPun {

        private bool catchable = false ;
        private bool caught = false ;
        float transparencyValue;
        Color lastColor;
        bool objectHighlighted;
        float highlightingSpeed = 0.5f;
        Renderer objectRenderer;
        public int numberOfPlayers;
        public int numberOfPlayersNeeded;

        void Start()
        {
            numberOfPlayers = 0;
            transparencyValue = 1f;
            objectRenderer = GetComponent<Renderer>();
            lastColor = objectRenderer.material.color;
        }

        void Update () {
            if (objectHighlighted) {
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
                numberOfPlayers ++;
            }
        }

        [PunRPC] public void ShowReleased () {
            if (caught) {
                var rb = GetComponent<Rigidbody> () ;
                rb.isKinematic = false ;
                objectRenderer.material.color = new Color(lastColor.r, lastColor.g, lastColor.b, 1);
                caught = false ;
                objectHighlighted = false;
                numberOfPlayers --;
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

    }

}
