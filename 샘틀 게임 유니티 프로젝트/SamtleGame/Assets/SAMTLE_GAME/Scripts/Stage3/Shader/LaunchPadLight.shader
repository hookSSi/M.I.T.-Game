Shader "Custom/LaunchPadLight"
{
    Properties
    {
        _originColor("OriginColor", Color) = (1,1,1,1)
        _targetColor("TargetColor", Color) = (1,1,1,1)
        _brightNess("Brightness", range(0, 5)) = 1
        _factor("Factor", range(0, 1)) = 0
    }
    SubShader
    {   
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _originColor;
        fixed4 _targetColor;
        float _brightNess;
        float _factor;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * ( _originColor * _factor + _targetColor * (1 - _factor) );
            o.Emission = c * _brightNess; 
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
