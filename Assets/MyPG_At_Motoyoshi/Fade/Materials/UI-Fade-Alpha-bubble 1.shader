// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
 The MIT License (MIT)

Copyright (c) 2013 yamamura tatsuhiko

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
Shader "UI/Fade Alpha Bubble"
{
	Properties
	{

		[PerRendererData] _MaskTex("Mask Texture", 2D) = "white" {}
		[PerRendererData] _Color ("Tint", Color) = (1,0,1,1)
		_Range("Range", Range (0, 1)) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				float4 screenCoord : TEXCOORD2;

			};

			fixed4 _Color;
			// fixed4 _TextureSampleAdd;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPosition = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;

				#ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
				#endif
				OUT.screenCoord.xy = ComputeScreenPos(OUT.vertex);

				OUT.color = IN.color * _Color;
				return OUT;
			}

			sampler2D _MaskTex;
			// sampler2D _SubTex;
			float _Range;

			uniform fixed4     fragColor;
			uniform fixed      iChannelTime[4];// channel playback time (in seconds)
			uniform fixed3     iChannelResolution[4];// channel resolution (in pixels)
			uniform fixed4     iMouse;// mouse pixel coords. xy: current (if MLB down), zw: click
			uniform fixed4     iDate;// (year, month, day, time in seconds)
			uniform fixed      iSampleRate;// sound sample rate (i.e., 44100)

			fixed4 render(fixed2 uv)
			{
				uv.x -= 0.5f;
				uv.x *=  _ScreenParams.x / _ScreenParams.y;
				uv.x += 0.5f;
    // background
	fixed3 color = fixed3(0.8 + 0.2*uv.y,0.8 + 0.2*uv.y,0.8 + 0.2*uv.y);

    // bubbles
	[unroll(100)]
for( int i=0; i<40; i++ )
	{
        // bubble seeds
		fixed pha =      sin(fixed(i)*546.13+1.0)*0.5 + 0.5;
		fixed siz = pow( sin(fixed(i)*651.74+5.0)*0.5 + 0.5, 4.0 );
		fixed pox =      sin(fixed(i)*321.55+4.1) * 1 / 1;

        // buble size, position and color
		fixed rad = 0.1 + 0.5*siz;
		fixed2  pos = fixed2( pox, -1.0-rad + (2.0+2.0*rad)*fmod(pha+0.1*_Time.y*(0.2+0.8*siz),1.0));
		fixed dis = length( uv - pos );
		fixed3  col = lerp( fixed3(0.94,0.3,0.0), fixed3(0.1,0.4,0.8), 0.5+0.5*sin(fixed(i)*1.2+1.9));
		//    col+= 8.0*smoothstep( rad*0.95, rad, dis );

        // render
		fixed f = length(uv-pos)/rad;
		f = sqrt(clamp(1.0-f*f,0.0,1.0));
		color -= col.zyx *(1.0-smoothstep( rad*0.95, rad, dis )) * f;
	}

    // vigneting
	color *= sqrt(1.5-0.5*length(uv));

	return  fixed4(color,1.0);

			}

			fixed4 frag(v2f IN) : SV_Target
			{
				float2 uv = IN.texcoord;
				half4 color = _Color;
				half mask = tex2D(_MaskTex, uv).r/*  - (-1 + _Range * 2) */;

				color.a = step(_Range, mask);
				half4 main = render(uv) * color;
				// main *= color;
				// clip(mask - 0.001);

				return main;
			}
		ENDCG
		}
	}
}
