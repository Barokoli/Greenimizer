using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Impact
{
    public Measure measure;
    public Pattern pattern;
}

public class ObjectImpact : MonoBehaviour
{
    private Vector2 location;
    public List<Impact> impacts;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Pattern pat;
        //float pixelwert;
        //Vector2 ort;
        //measureMaps[].SetPixel(ort.x, ort.y, Color.white * pixelwert);
        
    }
}
