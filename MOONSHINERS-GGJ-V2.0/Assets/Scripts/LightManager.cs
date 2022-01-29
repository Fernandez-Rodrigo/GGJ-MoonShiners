using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightManager : MonoBehaviour
{
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;
  

    private void Start()
    {
       
    }

    private void Update()
    {
        if (Preset == null)
            return;


        if (Application.isPlaying)
        {
            if (Input.GetKey(KeyCode.T))
            {
                TimeOfDay += Time.deltaTime*10;
                TimeOfDay %= 24;
                UpdateLighting(TimeOfDay / 24f);
            }
            else
            {
                TimeOfDay += Time.deltaTime*0.7f;
                TimeOfDay %= 24;
                UpdateLighting(TimeOfDay / 24f);
            }


        }

        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }


    }


    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);


        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }


    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;


        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }

        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}
