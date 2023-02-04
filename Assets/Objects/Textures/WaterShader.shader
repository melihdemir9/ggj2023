// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WaterShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_DistortionTexture("DistortionTexture", 2D) = "white" {}
		_WaterTexture("WaterTexture", 2D) = "white" {}
		_BaseColor("BaseColor", Color) = (0,0,0,0)
		_DistortionSpeed("DistortionSpeed", Float) = 0.05
		_DistortionPower("DistortionPower", Vector) = (0,0,0,0)
		_DeepPower("DeepPower", Float) = 0
		_WavePower("WavePower", Float) = 1
		_WaveDepth("WaveDepth", Float) = 0.1
		_FoamColor("Foam Color", Color) = (1,1,1,1)
		_WaveColor("Wave Color", Color) = (1,1,1,1)
		_ShorePower("ShorePower", Range( 0 , 6)) = 3

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		
		Pass
		{
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_COLOR


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform half _ShorePower;
			uniform sampler2D _DistortionTexture;
			uniform half _DistortionSpeed;
			uniform half4 _DistortionTexture_ST;
			uniform half _DeepPower;
			uniform half _WaveDepth;
			uniform half4 _FoamColor;
			uniform half4 _WaveColor;
			uniform sampler2D _WaterTexture;
			uniform half2 _DistortionPower;
			uniform half4 _WaterTexture_ST;
			uniform half _WavePower;
			uniform half4 _BaseColor;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				half mulTime19 = _Time.y * _DistortionSpeed;
				half2 uv_DistortionTexture = IN.texcoord.xy * _DistortionTexture_ST.xy + _DistortionTexture_ST.zw;
				half4 tex2DNode24 = tex2D( _DistortionTexture, ( frac( mulTime19 ) + uv_DistortionTexture ) );
				half temp_output_110_0 = ( _ShorePower * tex2DNode24.r * ( 1.0 - IN.color.a ) );
				half temp_output_386_0 = step( temp_output_110_0 , 1.0 );
				half4 appendResult112 = (half4(1.0 , 1.0 , 1.0 , ( temp_output_386_0 * pow( IN.color.a , _DeepPower ) )));
				half temp_output_459_0 = ( temp_output_386_0 - step( temp_output_110_0 , ( 1.0 - _WaveDepth ) ) );
				half4 appendResult409 = (half4(temp_output_459_0 , temp_output_459_0 , temp_output_459_0 , 0.0));
				half4 appendResult476 = (half4(_FoamColor.r , _FoamColor.g , _FoamColor.b , 0.0));
				half4 appendResult478 = (half4(_WaveColor.r , _WaveColor.g , _WaveColor.b , 0.0));
				half2 uv_WaterTexture = IN.texcoord.xy * _WaterTexture_ST.xy + _WaterTexture_ST.zw;
				half temp_output_330_0 = ( tex2D( _WaterTexture, ( ( tex2DNode24.g * _DistortionPower ) + uv_WaterTexture ) ).r * _WavePower );
				half4 appendResult127 = (half4(temp_output_330_0 , temp_output_330_0 , temp_output_330_0 , 0.0));
				
				fixed4 c = ( appendResult112 * ( ( appendResult409 * appendResult476 ) + ( appendResult478 * appendResult127 ) + ( IN.color * _BaseColor ) ) );
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18909
1959;45;1920;1023;1834.485;1115.73;2.644862;True;False
Node;AmplifyShaderEditor.CommentaryNode;65;-1679.056,-1023.285;Inherit;False;1134.661;409.4323;Distortion;7;24;21;20;19;1;18;473;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1636.676,-945.1395;Inherit;False;Property;_DistortionSpeed;DistortionSpeed;3;0;Create;True;0;0;0;False;0;False;0.05;-0.015;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;19;-1454.677,-937.3397;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-1638.411,-859.2211;Inherit;True;Property;_DistortionTexture;DistortionTexture;0;0;Create;True;0;0;0;False;0;False;None;66d49682907f22141a3e32a31d312c7c;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.FractNode;20;-1286.977,-934.7393;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-1392.352,-801.0231;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;294;-964.5165,-519.1213;Inherit;False;1330.536;469.1179;SeaReflection;9;33;127;330;2;36;14;13;31;29;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;473;-1026.866,-852.7753;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;151;-468.5373,-1046.948;Inherit;False;838.0031;459.4628;ShoreLine;5;338;386;110;109;75;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;467;-64.6223,-1487.489;Inherit;False;916.9884;364.4462;Front Foam;5;409;459;458;460;440;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;29;-883.9175,-328.6707;Inherit;True;Property;_WaterTexture;WaterTexture;1;0;Create;True;0;0;0;False;0;False;None;48709f3ac7166f94c8561aa994d85efd;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.VertexColorNode;75;-396.317,-799.4369;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;33;-876.4503,-469.9718;Inherit;False;Property;_DistortionPower;DistortionPower;4;0;Create;True;0;0;0;False;0;False;0,0;0.2,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;24;-860.127,-917.7615;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-625.6979,-248.02;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;338;-353.8755,-980.0391;Inherit;False;Property;_ShorePower;ShorePower;17;0;Create;True;0;0;0;False;0;False;3;4.12;0;6;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-555.2163,-382.7569;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;109;-209.7084,-746.0073;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;440;-35.62609,-1273.942;Inherit;False;Property;_WaveDepth;WaveDepth;12;0;Create;True;0;0;0;False;0;False;0.1;0.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-395.7524,-266.2408;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;460;121.1939,-1347.193;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;0.985625,-892.0323;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;387;485.1286,-1000.927;Inherit;False;749.6;290.4;Overall Alpha;4;112;353;348;370;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;2;-257.2657,-330.5872;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;528;950.441,-1480.791;Inherit;False;465.3266;373.0175;Foam Color;3;476;474;547;;1,1,1,1;0;0
Node;AmplifyShaderEditor.StepOpNode;386;185.871,-763.534;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;458;280.8102,-1349.303;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-118.118,-140.0788;Inherit;False;Property;_WavePower;WavePower;11;0;Create;True;0;0;0;False;0;False;1;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;330;40.32529,-275.1925;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;474;1000.441,-1430.791;Inherit;False;Property;_FoamColor;Foam Color;13;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;459;429.2514,-1348.276;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;477;431.9319,-443.2221;Inherit;False;Property;_WaveColor;Wave Color;16;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;370;539.3185,-794.8311;Inherit;False;Property;_DeepPower;DeepPower;8;0;Create;True;0;0;0;False;0;False;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;295;471.4753,-77.2727;Inherit;False;394.3999;391.2;Base;3;129;3;131;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;476;1254.768,-1419.075;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;478;647.5618,-412.5015;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.VertexColorNode;131;560.3776,-41.31913;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;127;201.9726,-301.5373;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PowerNode;348;714.3364,-910.7221;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0.05;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;409;575.0734,-1379.354;Inherit;False;FLOAT4;4;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;3;493.9063,128.5351;Inherit;False;Property;_BaseColor;BaseColor;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,0.8235294;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;547;1259.003,-1240.743;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;353;894.6492,-891.9831;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;479;806.5421,-287.8911;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;738.8543,35.6178;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;47;-471.9563,418.8063;Inherit;False;1184.982;407.5775;Sun;10;103;104;57;55;53;52;50;51;49;48;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;112;1067.347,-943.6993;Inherit;False;FLOAT4;4;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;505;1462.78,-525.7908;Inherit;False;3;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;487;2181.624,-1468.029;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;522;2366.837,784.4908;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;52;-49.80758,469.5062;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.75;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;545;1676.554,591.0958;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;104;289.3561,692.8491;Inherit;False;Property;_SunWavePower;SunWavePower;7;0;Create;True;0;0;0;False;0;False;1;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;538;1768.272,-79.45346;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;502.317,463.4195;Inherit;True;3;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;55;96.18128,538.2743;Float;False;Property;_SunMargin;SunMargin;6;0;Create;True;0;0;0;False;0;False;0;0.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;535;1399.854,74.74957;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenPosInputsNode;48;-454.2253,465.9996;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;49;-264.7615,581.966;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;51;-280.9094,682.6889;Inherit;False;Property;_SunCenter;SunCenter;5;0;Create;True;0;0;0;False;0;False;0.5,0.8;0.5,0.9;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;539;1430.578,-129.3798;Inherit;False;Property;_Color0;Color 0;15;0;Create;True;0;0;0;False;0;False;1,1,1,1;0.4295283,0.3042453,0.5,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;544;1362.356,565.944;Inherit;False;Property;_Float1;Float 1;10;0;Create;True;0;0;0;False;0;False;0;0.501;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;541;1409.513,295.5885;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;542;1640.608,138.7456;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;520;1767.208,-1008.085;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;53;88.49934,468.0239;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;546;2125.825,-812.0082;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PowerNode;543;1811.059,490.2639;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;540;1900.346,348.2414;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;489;802.1024,-641.0771;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;519;1498.819,-1006.203;Inherit;False;Property;_Float0;Float 0;9;0;Create;True;0;0;0;False;0;False;0;2.71;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;517;1697.981,-1207.223;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;508;1867.075,-1290.419;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;518;1904.522,-1016.883;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;486;1936.298,-1503.745;Inherit;False;Property;_ColorGroundShadow;Color Ground Shadow ;14;0;Create;True;0;0;0;False;0;False;1,1,1,1;0.4295283,0.3042453,0.5,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;524;2044.473,-1190.324;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;57;250.8084,462.5422;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.2;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;1731.195,-626.5489;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;50;-179.0795,472.2784;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;46;2146.607,-479.3124;Half;False;True;-1;2;ASEMaterialInspector;0;8;WaterShader;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;3;1;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;19;0;18;0
WireConnection;20;0;19;0
WireConnection;21;2;1;0
WireConnection;473;0;20;0
WireConnection;473;1;21;0
WireConnection;24;0;1;0
WireConnection;24;1;473;0
WireConnection;13;2;29;0
WireConnection;31;0;24;2
WireConnection;31;1;33;0
WireConnection;109;0;75;4
WireConnection;14;0;31;0
WireConnection;14;1;13;0
WireConnection;460;0;440;0
WireConnection;110;0;338;0
WireConnection;110;1;24;1
WireConnection;110;2;109;0
WireConnection;2;0;29;0
WireConnection;2;1;14;0
WireConnection;386;0;110;0
WireConnection;458;0;110;0
WireConnection;458;1;460;0
WireConnection;330;0;2;1
WireConnection;330;1;36;0
WireConnection;459;0;386;0
WireConnection;459;1;458;0
WireConnection;476;0;474;1
WireConnection;476;1;474;2
WireConnection;476;2;474;3
WireConnection;478;0;477;1
WireConnection;478;1;477;2
WireConnection;478;2;477;3
WireConnection;127;0;330;0
WireConnection;127;1;330;0
WireConnection;127;2;330;0
WireConnection;348;0;75;4
WireConnection;348;1;370;0
WireConnection;409;0;459;0
WireConnection;409;1;459;0
WireConnection;409;2;459;0
WireConnection;547;0;409;0
WireConnection;547;1;476;0
WireConnection;353;0;386;0
WireConnection;353;1;348;0
WireConnection;479;0;478;0
WireConnection;479;1;127;0
WireConnection;129;0;131;0
WireConnection;129;1;3;0
WireConnection;112;3;353;0
WireConnection;505;0;547;0
WireConnection;505;1;479;0
WireConnection;505;2;129;0
WireConnection;487;0;486;1
WireConnection;487;1;486;2
WireConnection;487;2;486;3
WireConnection;487;3;524;0
WireConnection;52;0;50;0
WireConnection;52;1;51;0
WireConnection;545;0;544;0
WireConnection;538;0;539;1
WireConnection;538;1;539;2
WireConnection;538;2;539;3
WireConnection;538;3;542;0
WireConnection;103;0;127;0
WireConnection;103;1;57;0
WireConnection;103;2;104;0
WireConnection;49;0;48;2
WireConnection;542;0;540;0
WireConnection;542;1;541;4
WireConnection;520;0;519;0
WireConnection;53;0;52;0
WireConnection;546;0;487;0
WireConnection;546;1;112;0
WireConnection;543;0;535;4
WireConnection;543;1;545;0
WireConnection;540;0;543;0
WireConnection;518;0;517;4
WireConnection;518;1;520;0
WireConnection;524;0;508;4
WireConnection;524;1;518;0
WireConnection;57;0;53;0
WireConnection;57;1;55;0
WireConnection;84;0;112;0
WireConnection;84;1;505;0
WireConnection;50;0;48;1
WireConnection;50;1;49;0
WireConnection;46;0;84;0
ASEEND*/
//CHKSM=0CB43414F1F9CA66F6064154925BE83E2210B4B3