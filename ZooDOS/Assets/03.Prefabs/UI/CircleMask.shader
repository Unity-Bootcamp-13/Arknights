Shader "UI/CircleMask"
{
    Properties
    {
        _HoleCenter ("Hole Center", Vector) = (0.5, 0.5, 0, 0)
        _HoleRadius ("Hole Radius", Float) = 0.05
        _Color ("Overlay Color", Color) = (0, 0, 0, 0.7)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay+1" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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

            fixed4 _Color;
            float2 _HoleCenter;
            float _HoleRadius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                 float2 uvOffset = i.uv - _HoleCenter;

                // 종횡비 보정: x 값을 축소 (원형 유지 목적)
                float aspect = _ScreenParams.x / _ScreenParams.y;
                uvOffset.x *= aspect;

                float dist = length(uvOffset);
                if (dist < _HoleRadius)
                    discard;

                return _Color;
            }
            ENDCG
        }
    }
}