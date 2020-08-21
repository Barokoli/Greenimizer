using UnityEngine;

[System.Serializable]
public abstract class Pattern : ScriptableObject
{
    public abstract float impact(Vector2 position, Vector2 samplePosition);
}

[System.Serializable]
[CreateAssetMenu(fileName = "PointPattern", menuName = "PointPattern", order = 0)]
public class PointPattern : Pattern
{
    public float radius;
    
    public override float impact(Vector2 position, Vector2 samplePosition)
    {
        return 1.0f * Mathf.Max(radius - Vector2.Distance(position, samplePosition), 0.0f);
    }
}