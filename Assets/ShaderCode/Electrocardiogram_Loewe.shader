
Shader "ShaderMan/Electrocardiogram_Loewe"
	{

	Properties{
	//Properties
	}

	SubShader
	{
	Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

	Pass
	{
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha

	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"

	struct VertexInput {
    fixed4 vertex : POSITION;
	fixed2 uv:TEXCOORD0;
    fixed4 tangent : TANGENT;
    fixed3 normal : NORMAL;
	//VertexInput
	};


	struct VertexOutput {
	fixed4 pos : SV_POSITION;
	fixed2 uv:TEXCOORD0;
	//VertexOutput
	};

	//Variables

	



	VertexOutput vert (VertexInput v)
	{
	VertexOutput o;
	o.pos = UnityObjectToClipPos (v.vertex);
	o.uv = v.uv;
	//VertexFactory
	return o;
	}
	fixed4 frag(VertexOutput i) : SV_Target
	{
	
	fixed2 uv = (-1 + 2.0 * i.uv) / 1;
    fixed2 uv2 = uv;
    //Asin a + B sin 2a +C sin 3a +D sin 4a
    uv2.x += 1/1;
    uv2.x -= 2.0*fmod(_Time.y,1.0*1/1);
    fixed width = -(1.0/(25.0*uv2.x));
   	fixed3 l = fixed3(width , width* 1.9, width * 1.5);
    
    uv.y *= 2.0;
    fixed xx = abs(1.0/(20.0*max(abs(uv.x),0.3)));
    
    uv.x *=3.0;
    uv.y -= xx*(sin(uv.x)+3.0*sin(2.0*uv.x)+2.0*sin(3.0*uv.x)+sin(4.0*uv.x));//0.3*sin(uv.x)+0.2*sin(uv.x*2.0)+0.1*sin(uv.x*3.0)+0.1*sin(uv.x*4.0);
    fixed3 col = lerp(fixed3(1,1,1),fixed3(0,0,0),smoothstep(0.02,0.03,abs(uv.y)));
	return fixed4(col*l,1.0);

	}
	ENDCG
	}
  }
}

