Shader "Custom/Alpha Mask"
{
	Properties
	{
	   //_Color("Mask Color", Color) = (1, 1, 1, 1)
	   _MainTex("Base (RGB)", 2D) = "white" {}
	   _Mask("Culling Mask", 2D) = "white" {}
	   _Cutoff("Alpha cutoff", Range(0,1)) = 0.1
	}

	SubShader
	{
		Tags {"Queue" = "Transparent"}
		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaTest GEqual[_Cutoff]
		Pass
		{
			SetTexture[_Mask] {
		        /*constantColor[_Color]
			    Combine texture * constant*/
				Combine texture
			}
			SetTexture[_MainTex] {combine texture, previous}
		}
	}
}
