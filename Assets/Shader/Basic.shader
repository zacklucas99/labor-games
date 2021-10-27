Shader "Unlit/Basic"
{
    Properties
    {
        _Color("Main color", Color) = (1,1,1,0)
        _MainTex ("Texture", 2D) = "white" {}
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
    
    ENDCG

    Subshader
    {
        Tags { "Queue" = "Transparent"}
        LOD 3000
        
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