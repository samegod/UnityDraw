Shader "Custom/Terrain"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _M1 ("Albedo (RGB)", 2D) = "white" {}
        _M2 ("mask", 2D) = "white" {}
        _m1sc ("ms", Range(0,1)) = 0.5 // for scaling second texture
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _M1;
        sampler2D _M2;

        struct Input
        {
            float2 uv_MainTex : TEXCOORD0;
            float2 uv2_M2;
        };

        half _Glossiness;
        half _Metallic;
        half _m1sc;
        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = lerp(tex2D (_MainTex, IN.uv_MainTex), tex2D (_M1, IN.uv_MainTex * _m1sc), tex2D (_M2, IN.uv2_M2).a);
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}