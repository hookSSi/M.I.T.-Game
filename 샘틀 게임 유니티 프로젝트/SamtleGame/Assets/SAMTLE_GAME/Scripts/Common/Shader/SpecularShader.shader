Shader "Test/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightColor("Light Color", color) = (0, 0, 0, 0)
        _SpecularMap("Specular Map", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 diffuse : TEXCOORD1;
                float3 reflection : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_M, v.vertex);
                float3 lightDir = normalize(v.vertex.xyz - _WorldSpaceLightPos0);

                float3 worldNormal = mul (UNITY_MATRIX_M, v.normal);
                worldNormal = normalize(worldNormal);

                o.vertex = mul(UNITY_MATRIX_VP, o.vertex);
                o.diffuse = dot(-lightDir, worldNormal);
                o.reflection = reflect(-lightDir, worldNormal);
                o.uv = v.uv;

                return o;
            }

            sampler2D _MainTex;
            sampler2D _SpecularMap;
            float3 _LightColor;

            float4 frag (v2f i) : SV_Target
            {
                float4 albedo = tex2D(_MainTex, i.uv);
                float3 diffuse = _LightColor * albedo * saturate(i.diffuse);

                float3 reflection = normalize(i.reflection);
                float3 viewDir = normalize(i.vertex.xyz - _WorldSpaceCameraPos);
                float3 specular = 0;
                
                if(diffuse.x > 0)
                {
                    specular = saturate(dot(reflection, -viewDir));
                    specular = pow(specular, 20.0);
                    float4 specularIntensity = tex2D(_SpecularMap, i.uv);
                    specular *= specularIntensity.rgb * _LightColor;
                }
                float3 ambient = float3(0.1, 0.1, 0.1) * albedo;

                return float4(albedo.rgb * (diffuse + specular + ambient), albedo.a);
            }
            ENDCG
        }
    }
}
