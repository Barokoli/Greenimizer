using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectImpact : MonoBehaviour
{
    public Vector2 location;
    public List<Measure, Pattern> impact;

    public List<Texture2D> measureMaps;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Pattern pat;
        float pixelwert;
        Vector2 ort;
        measureMaps[Measure].SetPixel(ort.x, ort.y, Color.white * pixelwert);
        
    }
}
