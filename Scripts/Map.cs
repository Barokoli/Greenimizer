using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Map : MonoBehaviour
{
    public List<GameObject> placeables;
    private Dictionary<string, List<ObjectImpact>> objects;
    private Dictionary<string, Texture2D> measureMaps;
    public Vector2 mapSize;
    private void Start()
    {
        foreach (var obj in placeables)
        {
            ObjectImpact oi = obj.GetComponent<ObjectImpact>();
            if (oi != null)
            {
                List<Impact> impacts = oi.impacts;
                foreach (var imp in impacts)
                {
                    Measure measure = imp.measure;
                    if (!measureMaps.ContainsKey(measure.name))
                    {
                        measureMaps.Add(measure.name, 
                            new Texture2D((int)mapSize.x, (int)mapSize.y));
                    }
                }
            }
        }
    }

    public void calculateMaps()
    {
        foreach (KeyValuePair<string, List<ObjectImpact>> elem in objects)
        {
            string measure = elem.Key;
            List<ObjectImpact> objs = elem.Value;
            //Color[]
            
        }
    }
}
