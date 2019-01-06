Shader "Custom/Poem Shader" {
	Properties{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
	}

	SubShader{
		Tags {"RenderType" = "Transparent" "Queue" = "Transparent"}
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		sampler2D _Mask;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Mask;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 c2 = tex2D(_Mask, IN.uv_Mask);
			o.Albedo = c.rgb;
			o.Alpha = c.a*c2.a;
		}
		ENDCG
	}

	Fallback "Transparent/VertexLit"
}
