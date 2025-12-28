Shader "Custom/WaveGlow"
{
    Properties
    {
        _Color ("Glow Color", Color) = (0.5, 1, 1, 1)
        _Intensity ("Glow Intensity", Range(0,5)) = 1
        _Speed ("Wave Speed", Range(0,10)) = 3
        _Frequency ("Wave Frequency", Range(0,10)) = 5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha One   // 発光
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _Color;
            float _Intensity;
            float _Speed;
            float _Frequency;
            
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 波模様を生成
                float wave = sin(i.uv.x * _Frequency + _Time.y * _Speed);

                // 0〜1に変換
                wave = (wave + 1) * 0.5;

                return _Color * wave * _Intensity;
            }
            ENDCG
        }
    }
}
