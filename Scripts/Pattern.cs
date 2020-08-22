using UnityEngine;

[System.Serializable]
public abstract class Pattern : ScriptableObject
{
    public abstract float impact(Vector2 position, Vector2 samplePosition);
    public abstract Rect bb(Vector2 position);
}

[System.Serializable]
[CreateAssetMenu(fileName = "PointPattern", menuName = "PointPattern", order = 0)]
public class PointPattern : Pattern
{
    public float radius;
    
    public override float impact(Vector2 position, Vector2 samplePosition)
    {
        float fact = Mathf.Max(radius - Vector2.Distance(position, samplePosition), 0.0f) / radius;
        return 1.0f * fact * fact;
    }

    public override Rect bb(Vector2 position)
    {
        return new Rect(position, Vector2.one * 2.0f * radius);
    }
}