using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "measure", menuName = "Measure", order = 0)]
public class Measure : ScriptableObject
{
    public string name;
}

[CreateAssetMenu(fileName = "filteredMeasure", menuName = "Filtered Measure", order = 0)]
public class FilteredMeasure : Measure
{
    public ComputeShader filter;
    private RenderTexture intermediate;

    public RenderTexture calculateFilter(Texture a, RenderTexture collider)
    {
        int kernelIndex = filter.FindKernel("CSMain");
        filter.SetTexture(kernelIndex, "input", a);
        filter.SetTexture(kernelIndex, "collider", collider);
        if (intermediate == null)
        {
            intermediate = new RenderTexture(a.width, a.height, 0);
            intermediate.enableRandomWrite = true;
            intermediate.Create();
        }
        filter.SetInt("width", a.width);
        filter.SetInt("height", a.height);
        filter.SetTexture(kernelIndex, "output", intermediate);
        filter.Dispatch(kernelIndex, Mathf.CeilToInt(a.width / 8),
            Mathf.CeilToInt(a.height / 8), 1);
        return intermediate;
    }
}
