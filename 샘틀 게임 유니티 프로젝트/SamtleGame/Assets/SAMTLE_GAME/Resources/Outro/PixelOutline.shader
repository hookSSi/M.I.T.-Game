Shader "Unlit/PixelOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", color) = (1,1,1,1)
    }
    SubShader
    {
        Pass
        {
			Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

				fixed up = tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y)).a;
				fixed down = tex2D(_MainTex, i.uv + fixed2(0, -_MainTex_TexelSize.y)).a;
				fixed left = tex2D(_MainTex, i.uv + fixed2(-_MainTex_TexelSize.x, 0)).a;
				fixed right = tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x, 0)).a;

				if (up + down + left + right != 0 && col.a == 0)
				{
					col = _Color;
				}
                return col;
            }
            ENDCG
        }
    }
}
