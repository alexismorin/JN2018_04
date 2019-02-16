// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:7051,x:32868,y:32712,varname:node_7051,prsc:2|custl-3772-OUT,olwid-1970-OUT,olcol-9401-RGB;n:type:ShaderForge.SFN_LightVector,id:2034,x:31461,y:32782,varname:node_2034,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:6252,x:31645,y:32930,varname:node_6252,prsc:2;n:type:ShaderForge.SFN_LightColor,id:5947,x:32443,y:32664,varname:node_5947,prsc:2;n:type:ShaderForge.SFN_Dot,id:6267,x:31645,y:32782,varname:node_6267,prsc:2,dt:0|A-2034-OUT,B-9300-OUT;n:type:ShaderForge.SFN_NormalVector,id:9300,x:31461,y:32904,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:7823,x:31961,y:32785,varname:node_7823,prsc:2|A-6267-OUT,B-6252-OUT;n:type:ShaderForge.SFN_Append,id:8609,x:32101,y:32785,varname:node_8609,prsc:2|A-7823-OUT,B-7823-OUT;n:type:ShaderForge.SFN_Tex2d,id:2594,x:32273,y:32785,ptovrint:False,ptlb:Ramp,ptin:_Ramp,varname:node_2594,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:181ec605fb3b4314bbee61db18ab41c0,ntxv:0,isnm:False|UVIN-8609-OUT;n:type:ShaderForge.SFN_Tex2d,id:1614,x:32273,y:32597,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_1614,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:704,x:32443,y:32787,varname:node_704,prsc:2|A-1614-RGB,B-2594-RGB;n:type:ShaderForge.SFN_Blend,id:3772,x:32619,y:32787,varname:node_3772,prsc:2,blmd:10,clmp:True|SRC-5947-RGB,DST-704-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1970,x:32619,y:33032,ptovrint:False,ptlb:Outline Width,ptin:_OutlineWidth,varname:node_1970,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.01;n:type:ShaderForge.SFN_Color,id:9401,x:32619,y:33113,ptovrint:False,ptlb:Outline Colour,ptin:_OutlineColour,varname:node_9401,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;proporder:2594-1614-1970-9401;pass:END;sub:END;*/

Shader "Custom/Sh_ToonRamp" {
    Properties {
        _Ramp ("Ramp", 2D) = "white" {}
        _Diffuse ("Diffuse", 2D) = "white" {}
        _OutlineWidth ("Outline Width", Float ) = 0.01
        _OutlineColour ("Outline Colour", Color) = (0,0,0,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _OutlineWidth;
            uniform float4 _OutlineColour;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(0)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*_OutlineWidth,1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                return fixed4(_OutlineColour.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Ramp; uniform float4 _Ramp_ST;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float node_7823 = (dot(lightDirection,i.normalDir)*attenuation);
                float2 node_8609 = float2(node_7823,node_7823);
                float4 _Ramp_var = tex2D(_Ramp,TRANSFORM_TEX(node_8609, _Ramp));
                float3 finalColor = saturate(( (_Diffuse_var.rgb*_Ramp_var.rgb) > 0.5 ? (1.0-(1.0-2.0*((_Diffuse_var.rgb*_Ramp_var.rgb)-0.5))*(1.0-_LightColor0.rgb)) : (2.0*(_Diffuse_var.rgb*_Ramp_var.rgb)*_LightColor0.rgb) ));
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Ramp; uniform float4 _Ramp_ST;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float node_7823 = (dot(lightDirection,i.normalDir)*attenuation);
                float2 node_8609 = float2(node_7823,node_7823);
                float4 _Ramp_var = tex2D(_Ramp,TRANSFORM_TEX(node_8609, _Ramp));
                float3 finalColor = saturate(( (_Diffuse_var.rgb*_Ramp_var.rgb) > 0.5 ? (1.0-(1.0-2.0*((_Diffuse_var.rgb*_Ramp_var.rgb)-0.5))*(1.0-_LightColor0.rgb)) : (2.0*(_Diffuse_var.rgb*_Ramp_var.rgb)*_LightColor0.rgb) ));
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
