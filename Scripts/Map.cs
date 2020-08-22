using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public List<GameObject> placeables;
    private Dictionary<string, List<ObjectImpact>> objects;
    private Dictionary<string, Texture2D> measureMaps;
    public int width;
    public int height;

    public RawImage debugImg;
    public static Vector2 ghost_offset;
    public Vector2 offset;

    public GameObject rootObject;

    private Camera colliderCam;
    private RenderTexture colliderTexture;
    private void Start()
    {
        GameObject tmp = new GameObject("ColliderCam");
        colliderCam = tmp.AddComponent<Camera>();
        colliderCam.CopyFrom(Camera.main);
        colliderCam.orthographic = true;
        colliderCam.orthographicSize = height * 0.5f;
        colliderCam.aspect = (float)width / (float)height;
        colliderTexture = new RenderTexture(width, height, 0);
        colliderTexture.enableRandomWrite = true;
        colliderCam.targetTexture = colliderTexture;
        colliderCam.transform.position = rootObject.transform.position + 
                                         new Vector3(width * 0.5f, 100, height * 0.5f);
        colliderCam.transform.rotation = Quaternion.identity;
        colliderCam.transform.Rotate(Vector3.right * 90.0f, Space.Self);
        colliderCam.enabled = false;
        colliderCam.cullingMask = 1 << LayerMask.NameToLayer("Buildings");
        colliderCam.clearFlags = CameraClearFlags.SolidColor;
        colliderCam.backgroundColor = Color.clear;
        ghost_offset = offset;
        measureMaps = new Dictionary<string, Texture2D>();
        objects = new Dictionary<string, List<ObjectImpact>>();
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
                        Texture2D tex1 = new Texture2D(width, height); 
                        measureMaps.Add(measure.name, 
                            tex1);
                    }
                }
            }
        }

        if (measureMaps.Count > 0)
        {
            debugImg.texture = measureMaps.First().Value;
        }
        calculateMaps();
    }

    public void gatherObjects()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("placeable");
        foreach (var obj in objs)
        {
            ObjectImpact oi = obj.GetComponent<ObjectImpact>();
            if (oi != null)
            {
                foreach (var imp in oi.impacts)
                {
                    if (!objects.ContainsKey(imp.measure.name))
                    {
                        objects.Add(imp.measure.name, new List<ObjectImpact>());
                    }
                    objects[imp.measure.name].Add(oi);
                }
            }
        }
    }
    
    public void calculateMaps()
    {
        gatherObjects();
        int onedsize = width * height;
        foreach (KeyValuePair<string, List<ObjectImpact>> elem in objects)
        {
            string measure = elem.Key;
            List<ObjectImpact> objs = elem.Value;
            Color[] mColor = new Color[onedsize];
            for (int i = 0; i < onedsize; i++)
            {
                mColor[i] = Color.black;
                int x = i % width;
                int y = i / width;
                Vector2 poi = new Vector2(x, y);
                foreach (ObjectImpact oi in objs)
                {
                    if (oi.bb.Contains(poi))
                    {
                        //mColor[i].r = (float)y / height;
                        mColor[i].r += oi.sample(measure, poi);
                    }
                }
            }
            measureMaps[measure].SetPixels(mColor);
            measureMaps[measure].Apply();
        }

        colliderCam.targetTexture = colliderTexture;
        colliderCam.Render();
        colliderCam.targetTexture = null;
        filterMaps();
    }

    public void filterMaps()
    {
        foreach (KeyValuePair<string, List<ObjectImpact>> elem in objects)
        {
            string mname = elem.Key;
            ObjectImpact oi = elem.Value.First();
            Impact im = oi.impacts.Find((x) => x.measure.name == mname);
            Measure measure = im.measure;
            if (measure is FilteredMeasure)
            {
                RenderTexture tmptex = ((FilteredMeasure) measure).calculateFilter(measureMaps[mname], colliderTexture);
                //debugImg.texture = measureMaps[mname];
                for (int i = 0; i < 100; i++)
                {
                    tmptex = ((FilteredMeasure) measure).calculateFilter(tmptex, colliderTexture);
                }

                RenderTexture.active = tmptex;
                measureMaps[mname].ReadPixels(
                    new Rect(0, 0, tmptex.width, tmptex.height), 0, 0);
                measureMaps[mname].Apply();
            }
        }
    }
}
