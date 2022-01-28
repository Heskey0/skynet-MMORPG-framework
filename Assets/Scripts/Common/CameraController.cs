using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //���� ���������ƶ���
    private static CameraController _Instance;
    public static CameraController Instance => _Instance;

    public Transform TheTransform => m_cameraTransform;

    //�������
    public Transform m_followTarget = null;
    public float m_smoothFactor = 0.01f;
    public Vector3 m_cameraRotOffset;
    public Vector3 m_cameraPosOffset;

    private Transform m_cameraTransform;

    private void Awake()
    {
        _Instance = this;

        m_cameraTransform = transform;
        SetCameraRotation();
    }

    private void LateUpdate()
    {
        if (m_followTarget != null)
        {
#if UNITY_EDITOR
            SetCameraRotation();    //��������Ƕ�
#endif
            Vector3 targetPos = m_followTarget.position + m_cameraPosOffset;
            m_cameraTransform.position =
                Vector3.Lerp(m_cameraTransform.position, targetPos, Time.deltaTime * m_smoothFactor);
        }
    }

    private void SetCameraRotation()
    {
        m_cameraTransform.localEulerAngles = m_cameraRotOffset;
    }
    
}