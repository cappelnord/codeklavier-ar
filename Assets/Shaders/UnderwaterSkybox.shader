Shader "Custom/Underwater-World"
{
Properties {
 _YGradient ("Y Gradient", 2D) = "white" {}
 _MoldMap ("Mold Map", 2D) = "white" {}

 _MoldMapBlurred ("Mold Map Blurred", 2D) = "white" {}
}


SubShader {
 Tags { "Queue"="Background"  }

 Pass {
    ZWrite Off
    Cull Off

    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    // User-specified uniforms
    sampler2D _YGradient;
    sampler2D _MoldMap;
    sampler2D _MoldMapBlurred;


    struct vertexInput {
       float4 vertex : POSITION;
       float3 texcoord : TEXCOORD0;
    };

    struct vertexOutput {
       float4 vertex : SV_POSITION;
       float3 texcoord : TEXCOORD0;
    };

    vertexOutput vert(vertexInput input)
    {
       vertexOutput output;
       output.vertex = UnityObjectToClipPos(input.vertex);
       output.texcoord = input.texcoord;
       return output;
    }

    fixed4 frag (vertexOutput input) : COLOR
    {
      // main color gradient
       float x = input.texcoord.x;
       float y = input.texcoord.y;
       float z = input.texcoord.z;

       float xn =  x * 0.5 + 0.5;
       float yn =  y * 0.5 + 0.5;
       float zn =  z * 0.5 + 0.5;

       float4 mainColor = tex2D(_YGradient, float2(0.5, yn));

       // additives
       float t = _Time.x;
       float sineA = abs(sin(t * 4.0 + x + y + z)) * 0.1;
       float sineB = abs(cos(t * 7.0 + x*2 + y + z)) * 0.07;

       float moldA = tex2D(_MoldMapBlurred, float2(yn + x * sin(t * 4.0 * z * 2.0) * 0.1, z)) * 0.2;
       float moldB = (1.0 - tex2D(_MoldMapBlurred, float2(xn + x * sin(t * 1.0 * z * 3.0) * 0.1, z))) * 0.15;

       float moldC = tex2D(_MoldMap, float2(yn + z * sin(t * 3.14 * y * 4.0) * 0.001, z * 0.1 + sin(t * 0.02))) * 0.1 * smoothstep(0.35, 0.55, yn);


       mainColor = mainColor + sineA + sineB + (moldA *  yn) - (moldB * (1.0-yn)) - moldC;

       return mainColor;
    }
    ENDCG
 }
}
}
