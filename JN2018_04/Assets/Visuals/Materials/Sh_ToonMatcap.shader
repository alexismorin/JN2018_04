// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Sh_ToonMatCap"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (0.5,0.4546506,0,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.01
		_MatCap("MatCap", 2D) = "white" {}
		_Diffuse("Diffuse", 2D) = "white" {}
		[HDR]_BaseColour("Base Colour", Color) = (1,1,1,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		uniform half4 _ASEOutlineColor;
		uniform half _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float4 _BaseColour;
		uniform sampler2D _Diffuse;
		uniform float4 _Diffuse_ST;
		uniform sampler2D _MatCap;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float2 uv_Diffuse = i.uv_texcoord * _Diffuse_ST.xy + _Diffuse_ST.zw;
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float3 worldToViewDir40 = mul( UNITY_MATRIX_V, float4( ase_normWorldNormal, 0 ) ).xyz;
			float3 temp_cast_5 = (-1.0).xxx;
			float3 temp_cast_6 = (1.0).xxx;
			float3 temp_cast_7 = (0.0).xxx;
			float3 temp_cast_8 = (1.0).xxx;
			float4 temp_output_37_0 = ( _BaseColour * tex2D( _Diffuse, uv_Diffuse ) * tex2D( _MatCap, ((temp_cast_7 + (worldToViewDir40 - temp_cast_5) * (temp_cast_8 - temp_cast_7) / (temp_cast_6 - temp_cast_5))).xy ) );
			c.rgb = temp_output_37_0.rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			float2 uv_Diffuse = i.uv_texcoord * _Diffuse_ST.xy + _Diffuse_ST.zw;
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float3 worldToViewDir40 = mul( UNITY_MATRIX_V, float4( ase_normWorldNormal, 0 ) ).xyz;
			float3 temp_cast_0 = (-1.0).xxx;
			float3 temp_cast_1 = (1.0).xxx;
			float3 temp_cast_2 = (0.0).xxx;
			float3 temp_cast_3 = (1.0).xxx;
			float4 temp_output_37_0 = ( _BaseColour * tex2D( _Diffuse, uv_Diffuse ) * tex2D( _MatCap, ((temp_cast_2 + (worldToViewDir40 - temp_cast_0) * (temp_cast_3 - temp_cast_2) / (temp_cast_1 - temp_cast_0))).xy ) );
			o.Emission = temp_output_37_0.rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
1200;162;1906;1010;-28.76611;666.4492;1;True;True
Node;AmplifyShaderEditor.WorldNormalVector;8;-815.0229,-525.6675;Float;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TransformDirectionNode;40;-626.5976,-525.0159;Float;False;World;View;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;43;-557.3625,-332.6767;Float;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-556.0756,-256.7325;Float;False;Constant;_Float1;Float 1;3;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-552.2138,-178.2142;Float;False;Constant;_Float2;Float 2;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;41;-356.5614,-342.9742;Float;False;5;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;46;-128.8388,-361.6531;Float;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;28;119.8192,-581.3368;Float;True;Property;_Diffuse;Diffuse;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;38;194.7328,-756.0135;Float;False;Property;_BaseColour;Base Colour;2;1;[HDR];Create;True;0;0;False;0;1,1,1,0;2,1.013518,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;117.6328,-375.9313;Float;True;Property;_MatCap;MatCap;0;0;Create;True;0;0;False;0;f14ccbc313d2d2148b293cb872d43251;0565438a1d8676f4ebc6107043d9ada6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;645.0658,-340.89;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OutlineNode;24;958.1971,-166.8643;Float;False;0;True;None;0;0;Front;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1151.37,-486.8097;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Sh_ToonMatCap;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;True;0.01;0.5,0.4546506,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;40;0;8;0
WireConnection;41;0;40;0
WireConnection;41;1;43;0
WireConnection;41;2;44;0
WireConnection;41;3;45;0
WireConnection;41;4;44;0
WireConnection;46;0;41;0
WireConnection;10;1;46;0
WireConnection;37;0;38;0
WireConnection;37;1;28;0
WireConnection;37;2;10;0
WireConnection;0;2;37;0
WireConnection;0;13;37;0
WireConnection;0;11;24;0
ASEEND*/
//CHKSM=B0269880B2268ED367DEF46AD08225E75C2AB16A