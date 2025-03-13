Shader "Unlit/NightVision"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // Textura de la pantalla
        _GreenTint ("Green Tint", Color) = (0,1,0,1) // Color verde de visión nocturna
        _NoiseStrength ("Noise Strength", Range(0,1)) = 0.1 // Intensidad del ruido
        _Brightness ("Brightness", Range(0,2)) = 1.0 // Brillo de la visión nocturna
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _GreenTint;
            float _NoiseStrength;
            float _Brightness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Genera ruido aleatorio basado en la UV
            float rand(float2 co) {
                return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv); // Captura la imagen de la pantalla
                float noise = rand(i.uv) * _NoiseStrength; // Aplica ruido aleatorio

                // Aplica el color verde, el brillo y el ruido
                col.rgb = (col.rgb * _GreenTint.rgb * _Brightness) + noise; 
                
                return col;
            }
            ENDCG
        }
    }
}
