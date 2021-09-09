

Shader "CustomRenderTexture/GrassSingleUpdateShader"
{
	Properties
	{
		[HideInInspector]
		_ObjData("ObjData", Vector) = (0,0,0,0)
		[HideInInspector]
		_PlayerData("PlayerData", Vector) = (0,0,0,0)

		_Stiffness("Stiffness", Float) = .5
		_Damping("Damping", Float) = .75
		_SpeedDown("SpeedDown", Float) = 20

		_Strength("Strength", Float) = .05
		[HideInInspector]
		_Scale("Scale", Vector) = (.2,.2,0,0)

		_Wind("Wind", Vector) = (0,0,0,0)

		[HideInInspector]
		_SpriteScale("SpriteScale", Vector) = (0,0,0,0)
		[HideInInspector]
		_Tex("InputTex", 2D) = "white" {}
	}

		SubShader
	{
	   Lighting Off
	   Blend One Zero

	   Pass
	   {
		   CGPROGRAM
		   #include "UnityCustomRenderTexture.cginc"
		   #include "NodeFcts.cginc"
		   #pragma vertex CustomRenderTextureVertexShader
		   #pragma fragment frag
		   #pragma target 3.0

		   //Properties:
		   float _Stiffness;
		   float _Damping;
		   float _SpeedDown;
		  
		   //Input:
		   float _Strength;
		   float4 _Scale;

		   //Wind:
		   float4 _Wind;
		   

		   //Transform:
		   float4 _ObjData;
		   float4 _PlayerData;

		   float4 _SpriteScale;
		   sampler2D   _Tex;

		   float4 frag(v2f_customrendertexture IN) : COLOR
		   {
			   //hole daten von der rendertex:
			   float4 prev = tex2D(_SelfTexture2D, IN.localTexcoord.xy);
			   float2 displ_prev = prev.xy;
			   float2 vel_prev = prev.zw;

			   float2 posRelativ = (_PlayerData.xy - _ObjData.xy) / _SpriteScale.xy + float2(.5, .5);//in Bereich: 0-1


			   //Position:
			   float2 delta = IN.localTexcoord.xy - posRelativ;//anstatt IN.globalTexcoord
			   //Capsule(vertical):
			   float y_pos = max(abs(delta.y), _Scale.y) - _Scale.y;
			   //Circle:
			   float radius = 1 - clamp(length(float2(delta.x, y_pos)) * 2 / _Scale.x, 0, 1);//Radialscale = .2
			   //Effect:
			   float2 effect = radius * _PlayerData.zw;


			   float2 displ_diff = (displ_prev - float2(.5, .5)) * _Stiffness;


			   float2 displ_nxt = displ_prev - (displ_diff + vel_prev - float2(.5,.5)) / _SpeedDown;


			   //Wind:
			   float Deg2Rad = .0174533;//PI/180
			   float2 windDir = float2(cos(_Wind.y * Deg2Rad), sin(_Wind.y * Deg2Rad));

			   float2 vel_nxt = clamp(vel_prev //gespeicherte geschw.
				   + displ_diff * (1-_Damping) //gedämpfte hinzugefügte geschw.
				   + effect * _Strength //Berührung
				   + _ObjData.zw * _Strength//Trägheit
				   + _Wind.x * windDir * Unity_SimpleNoise_float(IN.localTexcoord.xy + _Time.y * windDir * _Wind.z, _Wind.w)//wind
				   , 0, 1);


			   return float4(displ_nxt, vel_nxt);
		   }
		   ENDCG
	   }
	}
}
