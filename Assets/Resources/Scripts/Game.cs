using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public TextMesh cyanDisplay;
    public TextMesh yellowDisplay;
    public TextMesh magentaDisplay;
    public Detector cyanDetector;
    public Detector magentaDetector;
    public Detector yellowDetector;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        cyanDisplay.text = cyanDetector.getScore() + "";
        magentaDisplay.text = magentaDetector.getScore() + "";
        yellowDisplay.text = yellowDetector.getScore() + "";
    }

    
}
