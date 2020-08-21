using UnityEngine;

public abstract class Pattern : ScriptableObject
{
    public abstract float impact(Vector2 samplePosition);
}

[CreateAssetMenu(fileName = "PointPattern", menuName = "PointPattern", order = 0)]
public class PointPattern : ScriptableObject
{
    public float radius;
    
    public float impact(Vector2 position, Vector2 samplePosition)
    {
        return 1.0f * Mathf.Max(radius - Vector2.Distance(position, samplePosition), 0.0f);
    }
}