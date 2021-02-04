// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Underground"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 16.5
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_RenderTexture("RenderTexture", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "bump" {}
		_TextureSample3("Texture Sample 3", 2D) = "bump" {}
		_Height("Height", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Height;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform sampler2D _RenderTexture;
		uniform float4 _RenderTexture_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _TessValue;

		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 uv_TextureSample2 = v.texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 tex2DNode4 = tex2Dlod( _TextureSample2, float4( uv_TextureSample2, 0, 0.0) );
			v.vertex.xyz += ( float3(0,1,0) * _Height * ( 1.0 - tex2DNode4.r ) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample3 = i.uv_texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 tex2DNode4 = tex2D( _TextureSample2, uv_TextureSample2 );
			float3 lerpResult7 = lerp( UnpackNormal( tex2D( _TextureSample3, uv_TextureSample3 ) ) , UnpackNormal( tex2D( _TextureSample1, uv_TextureSample1 ) ) , tex2DNode4.r);
			o.Normal = lerpResult7;
			float2 uv_RenderTexture = i.uv_texcoord * _RenderTexture_ST.xy + _RenderTexture_ST.zw;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 lerpResult1 = lerp( tex2D( _RenderTexture, uv_RenderTexture ) , tex2D( _TextureSample0, uv_TextureSample0 ) , tex2DNode4.r);
			o.Albedo = lerpResult1.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16900
7;194;1352;692;1335.866;-121.2021;1.3;True;False
Node;AmplifyShaderEditor.SamplerNode;4;-1125.126,306.0427;Float;True;Property;_TextureSample2;Texture Sample 2;7;0;Create;True;0;0;False;0;None;af3eeb99b99e02b469ff670332297c59;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-1045.43,-147.5674;Float;True;Property;_RenderTexture;RenderTexture;6;0;Create;True;0;0;False;0;819119e79292c9f4b833b2aed1cc2290;819119e79292c9f4b833b2aed1cc2290;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-1152.403,493.8936;Float;True;Property;_TextureSample3;Texture Sample 3;9;0;Create;True;0;0;False;0;4d25d3561f3abe747b3b9f0bfa0930bf;4d25d3561f3abe747b3b9f0bfa0930bf;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-1049.848,674.7261;Float;True;Property;_TextureSample1;Texture Sample 1;8;0;Create;True;0;0;False;0;ba0e38348a07a7246adc4bb2dc19e7b2;ba0e38348a07a7246adc4bb2dc19e7b2;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;11;-489.0471,666.7768;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1130.219,42.3545;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;2dfd1728ee9d4184484fef5909211715;2dfd1728ee9d4184484fef5909211715;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-504.1518,582.6406;Float;False;Property;_Height;Height;10;0;Create;True;0;0;False;0;1;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;17;-566.5657,381.842;Float;False;Constant;_Vector0;Vector 0;7;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;1;-487.6075,6.801606;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;7;-514.6559,239.4485;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-179.5739,492.3091;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Underground;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;16.5;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;4;1
WireConnection;1;0;3;0
WireConnection;1;1;2;0
WireConnection;1;2;4;1
WireConnection;7;0;6;0
WireConnection;7;1;5;0
WireConnection;7;2;4;1
WireConnection;10;0;17;0
WireConnection;10;1;8;0
WireConnection;10;2;11;0
WireConnection;0;0;1;0
WireConnection;0;1;7;0
WireConnection;0;11;10;0
ASEEND*/
//CHKSM=4BC4F43D41372658D9FD080FCF55498EE15B7695