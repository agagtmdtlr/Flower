using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawnPetal : MonoBehaviour
{
    [System.Serializable]
    public enum TriggerType
    {
        Unknown,
        PetalYellow,
        PetalOrange,
        PetalRed,
        PetalPurple
    }
    public TriggerType triggerType;
    TriggerSensor trigger;
    
    private void Awake()
    {
        trigger = GetComponentInParent<TriggerSensor>(true);
        if (trigger == null)
        {
            Debug.LogError("TriggerSpawnPetal: No TriggerItem component attached");
        }
        trigger.callbacks.OnTriggerd += SpawnPetal;
    }

    private void SpawnPetal()
    {
        var g = PetalGenerator.Instance as PetalGenerator;
        g.GeneratePetal(TypeMapper.MapPetalType(triggerType), transform.position);
    }
}
