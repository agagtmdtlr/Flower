using System;
using System.Collections;
using System.Collections.Generic;
using Obi;
using UnityEngine;

public class PetalRope : MonoBehaviour
{
    public ObiRope rope;
    public PlayerBone bone;

    public Vector3 refPosition { get; private set; }
    
    [Range(0f, 1f)] public float minIndex = 0.1f;
    [Range(0f, 1f)] public float maxIndex = 0.8f;

    public float normalizePosition;

    private void Start()
    {
        rope.OnSimulationEnd += UpdatePetalFollowPosition;
    }

    private void Update()
    {
        UpdateAttachmentPosition();
        
        UpdateRopeStretchScale();
        
    }

    private void UpdateRopeStretchScale()
    {
        rope.stretchingScale = Mathf.Lerp(
            PetalSettings.Instance.minRopeStretchScale ,
            PetalSettings.Instance.maxRopeStretchScale ,
            PlayerController.LocalPlayer.NormalizedSpeed);  
    }

    void UpdatePetalFollowPosition(ObiActor actor, float simulatedTime, float substepTime)
    {
        if (!rope.isLoaded)
        {
            Debug.Log("No rope loaded");
            return;
        }

        var localPosition = GetRefPositionInRope();
        var localRotation = GetRefRotationInRope();
        var worldPosition = LocalToWorldPosition(localPosition, rope.solver.transform);

        refPosition = worldPosition;
    }

    void UpdateAttachmentPosition()
    {
        // update attachment position;
        var localPosition = transform.localPosition;
        if (bone.attached is not null)
            localPosition.z = 
                -(bone.transform.position - bone.attached.transform.position).magnitude 
                * normalizePosition;
        else
            localPosition.z = -bone.movementController.CurBoneDistance * normalizePosition;
                
        transform.localPosition = localPosition;
    }
    
    private Quaternion GetRefRotationInRope()
    {
        var endElement = rope.elements[0];
        int endParticleIndex = endElement.particle1;
        return rope.solver.orientations[endParticleIndex];
    }

    private Vector3 GetRefPositionInRope()
    {
        // 플레이어의 속도에 따라 파티클 위치가 정해진다.
        // 속도가 증가하면 중심에 가까워지고
        // 속도가 감소하면 중심에서 멀어진다.
        var t = PlayerController.LocalPlayer.NormalizedSpeed;
        float indexRange = Mathf.Lerp(minIndex, maxIndex, t);
        int elementIndex = (int)Mathf.Floor(Mathf.Lerp(0, rope.elements.Count - 1, indexRange));
        
        var element = rope.elements[elementIndex];
        var particleInex = element.particle1;
        
        return rope.solver.positions[particleInex];
    }

    // 로컬 좌표를 월드 좌표로 변환하는 함수
    private Vector3 LocalToWorldPosition(Vector4 localPosition, Transform solverTransform)
    {
        return solverTransform.TransformPoint(new Vector3(localPosition.x, localPosition.y, localPosition.z));
    }

}
