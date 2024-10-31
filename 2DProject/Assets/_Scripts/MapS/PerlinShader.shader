Shader "Custom/PerlinShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale ("Scale", Float) = 10 // �������� �� �������������
        _Threshold ("Threshold", Float) = 0.5 // �������� �� �������������
        _NoiseIntensity ("Noise Intensity", Float) = 0.1 // ������������ ����
        _NoiseFrequency ("Noise Frequency", Float) = 1.0 // ������� ����
        _NoiseAmplitude ("Noise Amplitude", Float) = 0.5 // �������� ����
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
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
            float _Scale;
            float _Threshold;
            float _NoiseIntensity;
            float _NoiseFrequency; // ���� ����� ��� ������� ����
            float _NoiseAmplitude;  // ���� ����� ��� �������� ����

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float rand(float2 uv)
            {
                return frac(sin(dot(uv.xy, float2(12.9898, 78.233))) * 43758.5453);
            }

            float PerlinNoise(float2 uv)
            {
                return tex2D(_MainTex, uv).r;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv * _Scale;

                // ����������� ������� �� �������� ����
                uv *= _NoiseFrequency;

                // ������ ���������� ��� �� UV-���������
                float randomOffsetX = (rand(uv) - 0.5) * _NoiseIntensity; 
                float randomOffsetY = (rand(uv + float2(100.0, 0)) - 0.5) * _NoiseIntensity;

                uv += float2(randomOffsetX, randomOffsetY) * _NoiseAmplitude; // ������� �� ��������

                // ��������� ���������� ����
                float noise = PerlinNoise(uv);
                
                // ������� ������� ��� ���������� �����
                float cave = smoothstep(_Threshold - 0.1, _Threshold + 0.1, noise);
                
                // ���������� �������
                fixed4 col = cave == 1 ? fixed4(0.1, 0.1, 0.1, 1.0) : fixed4(0.9, 0.9, 0.9, 1.0); 
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
