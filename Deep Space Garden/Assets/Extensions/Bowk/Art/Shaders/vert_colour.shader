//$-----------------------------------------------------------------------------
//#  simple vert colour
//&-----------------------------------------------------------------------------

Shader "bowk/vert_colour"
{
	Properties 
   	{
    	_colour ( "Color", Color )	= ( 1, 1, 1, 1 )  
   	}
 
	//=========================================================================
	SubShader 
	{
		Tags { "Queue" = "Geometry" "IgnoreProjector"="True" }

    	Pass 
		{    
      	 	ZWrite On
      	 	ZTest LEqual
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
