using System;
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
    private Vector2? _location;

    public Vector2 location
    {
        get
        {
            if (_location == null)
            {
                _location = new Vector2(this.gameObject.transform.localPosition.x - Map.ghost_offset.x,
                    this.gameObject.transform.localPosition.z - Map.ghost_offset.y);
            }

            return _location ?? default(Vector2);
        }
    }
    public List<Impact> impacts;

    private Rect? _bb;
    public Rect bb
    {
        get
        {
            if (_bb == null)
            {
                recalcBB();
            }

            return (Rect)_bb;
        }
    }

    public float sample(string measure, Vector2 poi)
    {
        for (int i = 0; i < impacts.Count; i++)
        {
            if (measure == impacts[i].measure.name)
            {
                return impacts[i].pattern.impact(location, poi);
            }
        }

        return 0.0f;
    }

    public void recalcBB()
    {
        foreach (var imp in impacts)
        {
            Rect impBB = imp.pattern.bb(location);
            Rect tbb = _bb ?? default(Rect);
            extendRect(impBB.min, ref tbb);
            extendRect(impBB.max, ref tbb);
            _bb = tbb;
        }
    }

    public void extendRect(Vector2 point, ref Rect r)
    {
        r.xMin = Mathf.Min(point.x, r.xMin);
        r.yMin = Mathf.Min(point.y, r.yMin);
        r.xMax = Mathf.Max(point.x, r.xMax);
        r.yMax = Mathf.Max(point.y, r.yMax);
    }
    

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
