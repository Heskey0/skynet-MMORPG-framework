Shader "Custom/CubeCartoon"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_RampThreshold("Lambert Threshold",Range(0,1))=0.315
		_RampSmooth("Lambert Smooth",Range(0,1))=0.05

		_Color("Color",Color)=(1,1,1,1)
		_ColorWeight("ColorWeight",Range(-3,3))=1
		_HColor ("Highlight Color", Color) = (0.8, 0.8, 0.8, 1.0)
        _SColor ("Shadow Color", Color) = (0.2, 0.2, 0.2, 1.0)

        _SpecularColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _SpecThreshold ("Specular Threshold", Range(0, 1)) = 0.34
        _SpecSmooth ("Specular Smooth", Range(0, 1)) = 0.06
        _Shininess ("Shininess", Range(0.001, 10)) = 7
        
        _FresnelIntensity("FresnelIntensity", Range(-1,1))=-0.04
        _FresnelThreshold("FresnelThreshold", Range(0, 1))=1
        _FresnelSmooth("FresnelSmooth", Range(0,1))=0.15
        _FresnelColor("FresnelColor",Color) = (1,1,1,1)
        
        _OutLineColor("OutLineColor",Color)=(0,0,0,1)
        _Factor("factor",Range(0,0.1)) = 0.0167
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

        Pass
        {

			Cull Front
			Offset -5,0
			CGPROGRAM
			
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

			sampler2D _MainTex;
	        float _Factor;
	        half4 _OutLineColor;
 

		    struct v2f
	        {
        		float4 vertex :POSITION;
		        float4 uv:TEXCOORD0;
            };
            
			v2f vert(appdata_base v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
            /*
	        v2f vert(appdata_full i)
	        {
	        	v2f o;
	        	i.vertex.xyz += i.normal * _Factor;
	        	o.vertex = UnityObjectToClipPos(i.vertex);
 
	        	return o;
	        }
            */
	        half4 frag(v2f v) :COLOR
            {
	            return _OutLineColor;
    	    }
    	        
			ENDCG
        }

		Pass
		{
		    Cull Back
			Tags {"LightMode"="ForwardBase"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal:NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal:TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _ColorWeight;
			float4 _SColor;
			float4 _HColor;
			float _RampThreshold;
			float _RampSmooth;

			float4 _SpecularColor;
			float _SpecThreshold;
			float _SpecSmooth;
			float _Shininess;
			
			float _FresnelIntensity;
			float _FresnelThreshold;
			float _FresnelSmooth;
			float4 _FresnelColor;
			
			float4 _OutLineColor;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal=mul(v.normal,(float3x3)unity_WorldToObject);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed3 lightDir=normalize(_WorldSpaceLightPos0.xyz);
				fixed3 viewDir=normalize(_WorldSpaceCameraPos.xyz);
				fixed3 halfDir=normalize(lightDir+viewDir);
                float3 normal=normalize(i.normal);

                
                //Lambert
                float4 SrcAlbedo=tex2D(_MainTex, i.uv)*_Color*_ColorWeight;
                //SrcAlbedo.a=1;
                float Lambert=saturate(dot(normal,lightDir));
                float LambertStep=smoothstep(
                    _RampThreshold-_RampSmooth
                    ,_RampThreshold+_RampSmooth
                    ,Lambert);
                //SrcAlbedo=_Color;
                _SColor=lerp(_HColor,_SColor,_SColor.a);
                float4 LambertColor=lerp(_SColor,_HColor,LambertStep)*SrcAlbedo;
                
                //Specular
                //srcAlbedo.a*(N,L)^shininess
                float Specular=saturate(pow(dot(normal,halfDir),_Shininess*128)*SrcAlbedo.a);
                float SpecularStep=smoothstep(
                    _SpecThreshold-_SpecSmooth
                    ,_SpecThreshold+_SpecSmooth
                    ,Specular);
                float4 SpecularColor=lerp(LambertColor,_SpecularColor,SpecularStep);
                float4 finalColor=1*LambertColor/*+SpecularColor*/;

                //Rim Fresnel
                float Fresnel=saturate(1-dot(normal,viewDir)+_FresnelIntensity);
                float FresnelStep=smoothstep(
                        _FresnelThreshold-_FresnelSmooth
                        ,_FresnelThreshold+_FresnelSmooth
                        ,Fresnel);
                float4 OutputColor=lerp(finalColor,_FresnelColor,FresnelStep);

                return OutputColor;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}

