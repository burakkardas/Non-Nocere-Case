Shader "ORamaVR/XrayShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0, 1, 1, 1)
        _RimColor ("Rim Color", Color) = (0, 0, 0, 1)
        _RimPower ("Rim Power", Range(0.1, 8.0)) = 8.0
        _Transparency ("Transparency", Range(0, 1)) = 1.0
        _Metallic ("Metallic", Range(0,1)) = 1
        _Smoothness ("Smoothness", Range(0,1)) = 1
        _ShadowIntensity ("Shadow Intensity", Range(0, 1)) = 1
        _IsInteracting ("Is Interacting", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite On
        Cull Off

        Pass
        {
            Tags
            {
                "LightMode"="ForwardBase"
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float3 worldPos : TEXCOORD3;
            };

            float4 _BaseColor;
            float4 _RimColor;
            float _RimPower;
            float _Transparency;
            float _Metallic;
            float _Smoothness;
            float _ShadowIntensity;
            float _IsInteracting;


            static const float4 _InteractBaseColor = float4(186 / 255.0, 105 / 255.0, 255 / 255.0, 1);
            static const float4 _InteractRimColor = float4(1, 1, 1, 1);
            static const float _InteractRimPower = 1.57;
            static const float _InteractTransparency = 0.193;


            static const float4 _DefaultRimColor = float4(0, 0, 0, 1);
            static const float _DefaultRimPower = 8.0;
            static const float _DefaultTransparency = 1.0;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 currentBaseColor = _IsInteracting > 0.5 ? _InteractBaseColor : _BaseColor;
                float4 currentRimColor = _IsInteracting > 0.5 ? _InteractRimColor : _DefaultRimColor;
                float currentRimPower = _IsInteracting > 0.5 ? _InteractRimPower : _DefaultRimPower;
                float currentTransparency = _IsInteracting > 0.5 ? _InteractTransparency : _DefaultTransparency;


                float rim = 1.0 - saturate(dot(i.viewDir, i.normal));
                rim = pow(rim, currentRimPower);


                float shadow = dot(i.normal, _WorldSpaceLightPos0.xyz);
                shadow = lerp(0.5, 1.0, saturate(shadow));
                shadow *= _ShadowIntensity;


                float metalEffect = lerp(0.2, 1.0, _Metallic);
                float gloss = lerp(0.1, 1.0, _Smoothness);


                float4 finalColor = currentBaseColor * metalEffect * shadow;
                finalColor.rgb += currentRimColor.rgb * rim * gloss;
                finalColor.a = currentTransparency;

                return finalColor;
            }
            ENDCG
        }
    }
}