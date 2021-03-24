using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public Color detectorColor;
    private int scoreDetector;
    private List<Collider> alreadyScored;
    // Start is called before the first frame update
    void Start()
    {
        scoreDetector = 0;
        alreadyScored = new List<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getScore() {
        return scoreDetector;
    }

    public void OnTriggerEnter(Collider other) {
        if (!alreadyScored.Contains(other)) {
            Color enteringColor = other.GetComponent<Renderer>().material.color;
            if (!colorEgality(enteringColor, detectorColor)) {
                scoreDetector --;
            }
            else {
                scoreDetector ++;
            }
            alreadyScored.Add(other);
        }

    }

    public bool colorEgality(Color c1, Color c2) {
        return c1.r == c2.r && c1.b == c2.b && c1.g == c2.g;
    }
}
