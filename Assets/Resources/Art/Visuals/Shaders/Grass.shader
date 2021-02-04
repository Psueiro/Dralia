// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Grass"
{
	Properties
	{
		[NoScaleOffset]_GrassTexture("Grass Texture", 2D) = "white" {}
		[NoScaleOffset]_GrassNormal("Grass Normal", 2D) = "bump" {}
		[NoScaleOffset]_HeightMap("HeightMap", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _GrassTexture;
		uniform sampler2D _GrassNormal;
		uniform sampler2D _HeightMap;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (20.0).xx;
			float2 uv_TexCoord15 = i.uv_texcoord * temp_cast_0;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float2 Offset14 = ( ( tex2D( _HeightMap, uv_TexCoord15 ).r - 1 ) * ase_worldViewDir.xy * 0.1 ) + uv_TexCoord15;
			o.Normal = UnpackNormal( tex2D( _GrassNormal, Offset14 ) );
			float4 color12 = IsGammaSpace() ? float4(0.3625751,1,0.2028302,0) : float4(0.1081327,1,0.03399345,0);
			o.Albedo = ( color12 * tex2D( _HeightMap, Offset14 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16900
7;7;1352;692;1031.914;1218.267;2.31142;True;False
Node;AmplifyShaderEditor.RangedFloatNode;18;-949.241,-66.57401;Float;False;Constant;_Tiling;Tiling;3;0;Create;True;0;0;False;0;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-738.343,-83.83971;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-485.5668,-345.4148;Float;True;Property;_HeightMap;HeightMap;2;1;[NoScaleOffset];Create;True;0;0;False;0;None;0d1f27c8e0410db4da2591347333704d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;19;-457.4277,250.8673;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;17;-535.1646,83.08865;Float;False;Constant;_ParallaxValue;Parallax Value;3;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;14;-212.9313,28.42721;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;12;-487.0331,-689.9869;Float;False;Constant;_Color0;Color 0;3;0;Create;True;0;0;False;0;0.3625751,1,0.2028302,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;20;-114.5678,-472.1622;Float;True;Property;_TextureSample0;Texture Sample 0;2;1;[NoScaleOffset];Create;True;0;0;False;0;None;0d1f27c8e0410db4da2591347333704d;True;0;False;white;Auto;False;Instance;11;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-874.4099,-334.9881;Float;False;Constant;_tint;tint;2;0;Create;True;0;0;False;0;0,0,0,0.5450981;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;34.9896,-36.39628;Float;True;Property;_GrassNormal;Grass Normal;1;1;[NoScaleOffset];Create;True;0;0;False;0;fe7238ccb497913429c0a36e3c0c5061;fe7238ccb497913429c0a36e3c0c5061;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;200.1044,-643.6982;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;767.2751,-427.2505;Float;True;Property;_GrassTexture;Grass Texture;0;1;[NoScaleOffset];Create;True;0;0;True;0;61e475ae3bbee4f4e8ea6a27bd0561b5;61e475ae3bbee4f4e8ea6a27bd0561b5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;596.8664,-199.937;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Grass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;18;0
WireConnection;11;1;15;0
WireConnection;14;0;15;0
WireConnection;14;1;11;1
WireConnection;14;2;17;0
WireConnection;14;3;19;0
WireConnection;20;1;14;0
WireConnection;4;1;14;0
WireConnection;13;0;12;0
WireConnection;13;1;20;0
WireConnection;0;0;13;0
WireConnection;0;1;4;0
ASEEND*/
//CHKSM=138E05FC7273A2CA8452100B764085E1A675F77C