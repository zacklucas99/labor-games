Shader "Unlit/Outline"
{
    Properties
    {
        _Color("Main color", Color) = (1,1,1,0)
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor("Outline color", Color) = (255, 172, 0, 255)
        _OutlineWidth("Outline width", Range(0.1,5.0)) = 1.03
    }

    CGINCLUDE
    #include "UnityCG.cginc"

    struct appdata
    {
         float4 vertex : POSITION;
         float3 normal : NORMAL;
    };

    struct v2f
    {
        float4 pos : POSITION;
        float3 normal : NORMAL;
    };

    float _OutlineWidth;
    float4 _OutlineColor;

    v2f vert(appdata v)
    {
        v.vertex.xyz *= _OutlineWidth;
        v2f o;
        o.pos = UnityObjectToClipPos(v.vertex);
        return o;
    }
    
    ENDCG

    Subshader
    {
        Tags { "Queue" = "Transparent"}
        LOD 3000
        Pass //Rendering Outlines 
        {
            Zwrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag 

            half4 frag(v2f i) : COLOR
            {
                return _OutlineColor;
            }
            ENDCG
        }
        
        Pass // Normal Render
        {
            ZWrite On
            
            Material
            {
                Diffuse[_Color]
                Ambient[_Color]
            }
                
            Lighting On

            SetTexture[_MainTex]
            {
                ConstantColor[_Color]
            }
            
            SetTexture[_MainTex]
            {
                Combine previous * primary DOUBLE
            }

        }
    }

}