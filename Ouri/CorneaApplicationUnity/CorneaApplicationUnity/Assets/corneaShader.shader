Shader "Custom/corneaShader" 
{
        Properties
        {
                [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
                _Color ("Tint", Color) = (1,1,1,1)
                [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        }
 
        SubShader
        {
                Tags
                {
                        "Queue"="Geometry"
                        "IgnoreProjector"="True"
                        "RenderType"="Geometry"
                        "PreviewType"="Plane"
                        "CanUseSpriteAtlas"="True"
                }
 
                Cull Off
                Lighting On
                ColorMask RGB
 
                Pass
                {
                CGPROGRAM
                        #pragma vertex vert
                        #pragma fragment frag
                       
                        struct appdata_t
                        {
                                float4 vertex   : POSITION;
                                float4 color    : COLOR;
                        };
 
                        struct v2f
                        {
                                float4 vertex   : SV_POSITION;
                                fixed4 color    : COLOR;
                                half2 texcoord  : TEXCOORD0;
                        };
                       
                        fixed4 _Color;
 
                        v2f vert(appdata_t IN)
                        {
                                v2f OUT;
                                OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
                                OUT.color = IN.color * _Color;
 
                                return OUT;
                        }
 
                        sampler2D _MainTex;
 
                        fixed4 frag(v2f IN) : COLOR
                        {
                                half4 prev = IN.color * tex2D(_MainTex, IN.texcoord);
                                return lerp(half4(1,1,1,1), prev, prev.a);
                        }
                ENDCG
                }
        }
}