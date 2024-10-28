using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UICutoffMask : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material _material = new Material(base.materialForRendering);
            _material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return _material;
        }
    }
}
