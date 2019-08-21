Shader "Custom/SineStripes"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_BackgroundColor ("Background Color", Color) = (0, 0, 0, 1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

		_Phase("Phase", Float) = 0.0
		_PhaseFrequency("Phase Frequency", Vector) = (10,10,10,10)
		_PhaseCoordOffset("Phase Coord Offset", Vector) = (1,1,1,1)
	}
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		static const float PI = 3.14159265f;


        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		fixed4 _BackgroundColor;


		float4 _PhaseFrequency;
		float4 _PhaseCoordOffset;

		float _Phase;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)

        {
            // Albedo comes from a texture tinted by color
			float c1 = sin((IN.uv_MainTex.x + _Phase + (_PhaseCoordOffset[0] * IN.uv_MainTex.y)) * _PhaseFrequency[0] * 2.0 * PI) + sin((IN.uv_MainTex.y + _Phase + (_PhaseCoordOffset[1] * IN.uv_MainTex.x)) * _PhaseFrequency[1] * 2.0 * PI);
			float c2 = sin((IN.uv_MainTex.x + _Phase + (_PhaseCoordOffset[2] * IN.uv_MainTex.y)) * _PhaseFrequency[2] * 2.0 * PI) + sin((IN.uv_MainTex.y + _Phase + (_PhaseCoordOffset[3] * IN.uv_MainTex.x)) * _PhaseFrequency[3] * 2.0 * PI);
			float c = (c1 + c2) * 0.5;
			c = max(0.0, c);
			o.Albedo = lerp(_BackgroundColor.rgb, _Color.rgb, c);

            // Metallic and smoothness come from slider variables

            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;

			o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
