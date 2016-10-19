//$-----------------------------------------------------------------------------
//#  Vert colour with additive alpha
//&-----------------------------------------------------------------------------

Shader "bowk/vert_colour_alpha_additive"
{
	Properties 
   	{
    	_colour ( "Color", Color )	= ( 1, 1, 1, 1 )  
   	}
 
	//=========================================================================
	SubShader 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

    	Pass 
		{    
      	 	ZWrite On
      	 	Blend SrcAlpha One
      		Cull Back
     		
			CGPROGRAM
			
 			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			float4 _colour;

         	struct VertInput
            {
                float4 vertex	: POSITION;
                float4 color	: COLOR;
            };

           	struct VertOutput
            {
                half4 pos		: SV_POSITION;
                fixed4 vcolor   : TEXCOORD0;
            };

 			//=============================================
			VertOutput vert ( VertInput vi )
			{
				VertOutput vo;
				
    			vo.pos			= mul ( UNITY_MATRIX_MVP, vi.vertex );
				vo.vcolor		= vi.color * _colour;

				return vo;
			}
 	
 			//=============================================
			fixed4 frag ( VertOutput v ):COLOR
			{			
    			return v.vcolor;
			}

			ENDCG
		}
 	}
}
