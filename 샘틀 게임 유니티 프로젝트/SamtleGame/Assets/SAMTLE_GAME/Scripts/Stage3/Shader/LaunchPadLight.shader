Shader "Custom/LaunchPadLight"
{
    Properties
    {
        _originColor("Origin Color", Color) = (1,1,1,1)
        [HDR] _targetColor("Target Color", Color) = (1,1,1,1)
        _brightNess("Brightness", range(0, 5)) = 1
        _factor("Factor", range(0, 1)) = 1
        [NoScaleOffset]_baseTex("Base Texture", 2D) = "white" {}
        [NoScaleOffset]_lightTex("Light Texture", 2D) = "white" {}
    }
    SubShader
    {   
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _baseTex;
        sampler2D _lightTex;

        struct Input
        {
            float2 uv_baseTex;

            float3 viewDir;
            float3 worldNormal;
        };

        fixed4 _originColor;
        fixed4 _targetColor;
        float _brightNess;
        float _factor;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float rim = dot(normalize(IN.viewDir), normalize(IN.worldNormal));

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_baseTex, IN.uv_baseTex) * ( _originColor * _factor + _targetColor * (1 - _factor) );
            fixed4 l = tex2D (_lightTex, IN.uv_baseTex);
            o.Emission =  l * _brightNess * rim; 
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
