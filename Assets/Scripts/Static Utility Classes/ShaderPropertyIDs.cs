using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderPropertyIDs : MonoBehaviour
{
    public static class Base
    {
        public static readonly int Color = Shader.PropertyToID("_BaseColor");
        public static readonly int MainTex = Shader.PropertyToID("_BaseMap");
        public static readonly int Metallic = Shader.PropertyToID("_Metallic");
        public static readonly int Smoothness = Shader.PropertyToID("_Smoothness");
        public static readonly int NormalMap = Shader.PropertyToID("_BumpMap");
    }

    public static class Emission
    {
        public static readonly int Color = Shader.PropertyToID("_EmissionColor");
        public static readonly string Keyword = "_EMISSION";
    }

    public static class Transparency
    {
        public static readonly int Surface = Shader.PropertyToID("_Surface");
        public static readonly int Blend = Shader.PropertyToID("_Blend");
        public static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
        public static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        public static readonly int ZWrite = Shader.PropertyToID("_ZWrite");
    }
}
