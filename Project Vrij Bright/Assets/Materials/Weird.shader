Shader "Unlit/Weird"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	_TintTex("Tint Texture", 2D) = "white" {}
	_ScrollSpeeds("Scroll Speeds", vector) = (-5.0, -20.0, 0, 0)
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
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
		sampler2D _MainTex;
		float4 _MainTex_ST;

		// Declare our second texture sampler and its Scale/Translate values
		sampler2D _TintTex;
		float4 _TintTex_ST;

		float4 _ScrollSpeeds;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};



			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex); 
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				float2 polar = float2(
					atan2(i.uv.y, i.uv.x) / (2.0f * 3.141592653589f), // angle
					log(dot(i.uv, i.uv)) * 0.5f                       // log-radius
					);

			// Check how much our texture sampling point changes between
			// neighbouring pixels to the sides (ddx) and above/below (ddy)
			float4 gradient = float4(ddx(polar), ddy(polar));

			// If our angle wraps around between adjacent samples,
			// discard one full rotation from its value and keep the fraction.
			gradient.xz = frac(gradient.xz + 1.5f) - 0.5f;

			// Copy the polar coordinates before we scale & shift them,
			// so we can scale & shift the tint texture independently.
			float2 tintUVs = polar * _TintTex_ST.xy;
			tintUVs += _ScrollSpeeds.zw * _Time.x;

			polar *= _MainTex_ST.xy;
			polar += _ScrollSpeeds.xy * _Time.x;

			// Sample with our custom gradients.
			fixed4 col = tex2Dgrad(_MainTex, polar,
				_MainTex_ST.xy * gradient.xy,
				_MainTex_ST.xy * gradient.zw
			);

			// Since our tint texture has its own scale,
			// its gradients also need to be scaled to match.
			col *= tex2Dgrad(_TintTex, tintUVs,
				_TintTex_ST.xy * gradient.xy,
				_TintTex_ST.xy * gradient.zw
			);

			UNITY_APPLY_FOG(i.fogCoord, col);
			return col;
			}
			ENDCG
		}
	}
}
