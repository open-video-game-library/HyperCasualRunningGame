using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

public enum DeployTarget
{
    [InspectorName("対象を直接指定")] Direct,
    [InspectorName("子オブジェクトを整列")] Children
}
public enum DeployMode
{
    [InspectorName("一様分布")] Linear,
    [InspectorName("ランダム分布")] Random,
}
public enum DeployType
{
    [InspectorName("範囲")] Range,
    [InspectorName("球体")] Sphere,
    [InspectorName("軸ごとに指定")] Individual
}
public enum DeployAxis
{
    [InspectorName("X軸_位置")] PosX,
    [InspectorName("X軸_回転")] AngleX,
    [InspectorName("X軸_スケール")] ScaleX,

    [InspectorName("Y軸_位置")] PosY,
    [InspectorName("Y軸_回転")] AngleY,
    [InspectorName("Y軸_スケール")] ScaleY,

    [InspectorName("Z軸_位置")] PosZ,
    [InspectorName("Z軸_回転")] AngleZ,
    [InspectorName("Z軸_スケール")] ScaleZ,
}

public class TransformDeployer : MonoBehaviour
{
    public DeployTarget deployTarget;
    public DeployMode deployMode;
    public DeployType deployType;
    public DeployAxis deployAxis;

    public List<Transform> targetObjects_transform;
    public Transform targetObjectsParent_transform;

    public Vector3 startPosition = new Vector3(0, 0, 0);
    public Vector3 startAngle = new Vector3(0, 0, 0);
    public Vector3 startScale = new Vector3(1, 1, 1);
    public Vector3 endPosition = new Vector3(0, 0, 0);
    public Vector3 endAngle = new Vector3(0, 0, 0);
    public Vector3 endScale = new Vector3(1, 1, 1);

    public Vector3 centerPosition;
    public float diameter;

    public float startValue;
    public float endValue;

    void OnDrawGizmosSelected()
    {
        if (deployType == DeployType.Range)
        {
            Vector3 size = endPosition - startPosition;
            Vector3 posOffset = size / 2;
            Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
            Gizmos.DrawCube(startPosition + posOffset, size);
            Gizmos.color = new Color(0f, 1f, 0f, 1f);
            Gizmos.DrawWireCube(startPosition + posOffset, size);
        }
        else if (deployType == DeployType.Sphere)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
            Gizmos.DrawSphere(centerPosition, diameter / 2);
            Gizmos.color = new Color(0f, 1f, 0f, 1f);
            Gizmos.DrawWireSphere(centerPosition, diameter / 2);
        }
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    public void Deploy()
    {
        List<Transform> targets_transform = new List<Transform>();
        if (deployTarget == DeployTarget.Direct) targets_transform = targetObjects_transform;
        else if (deployTarget == DeployTarget.Children)
        {
            for (int i = 0; i < targetObjectsParent_transform.childCount; i++)
            {
                Transform target_transform = targetObjectsParent_transform.GetChild(i);
                targets_transform.Add(target_transform);
            }
        }

        if (deployMode == DeployMode.Linear)
        {
            if (deployType == DeployType.Range)
            {
                for (int i = 0; i < targets_transform.Count; i++)
                {
                    targets_transform[i].transform.position = Vector3.Lerp(startPosition, endPosition, (float)i / (float)(targets_transform.Count - 1));
                    targets_transform[i].transform.localEulerAngles = Vector3.Lerp(startAngle, endAngle, (float)i / (float)(targets_transform.Count - 1));
                    targets_transform[i].transform.localScale = Vector3.Lerp(startScale, endScale, (float)i / (float)(targets_transform.Count - 1));
                }
            }
            else if (deployType == DeployType.Sphere)
            {
                for (int i = 0; i < targets_transform.Count; i++)
                {
                    float radius = diameter / 2;

                    float radiansV = Mathf.Lerp(-Mathf.PI / 2, Mathf.PI / 2, (float)i / (float)(targets_transform.Count - 1));
                    float radiansU = Mathf.Lerp(0, Mathf.PI * 2, (float)i / (float)(targets_transform.Count - 1));
                    float posX = radius * Mathf.Cos(radiansV) * Mathf.Cos(radiansU);
                    float posY = radius * Mathf.Cos(radiansV) * Mathf.Sin(radiansU);
                    float posZ = radius * Mathf.Sin(radiansV);

                    targets_transform[i].transform.position = new Vector3(posX, posY, posZ) + centerPosition;
                    Debug.Log(Vector3.Distance(targets_transform[i].transform.position, centerPosition));
                }
            }
            else if (deployType == DeployType.Individual)
            {
                for (int i = 0; i < targets_transform.Count; i++)
                {
                    float ratio = (float)i / (float)(targets_transform.Count - 1);
                    float value = Mathf.Lerp(startValue, endValue, ratio);

                    Vector3 pos = targets_transform[i].transform.position;
                    Vector3 angle = targets_transform[i].transform.localEulerAngles;
                    Vector3 scale = targets_transform[i].transform.localScale;

                    if (deployAxis == DeployAxis.PosX) pos.x = value;
                    else if (deployAxis == DeployAxis.AngleX) angle.x = value;
                    else if (deployAxis == DeployAxis.ScaleX) scale.x = value;

                    else if (deployAxis == DeployAxis.PosY) pos.y = value;
                    else if (deployAxis == DeployAxis.AngleY) angle.y = value;
                    else if (deployAxis == DeployAxis.ScaleY) scale.y = value;

                    else if (deployAxis == DeployAxis.PosZ) pos.z = value;
                    else if (deployAxis == DeployAxis.AngleZ) angle.z = value;
                    else if (deployAxis == DeployAxis.ScaleZ) scale.z = value;

                    targets_transform[i].transform.position = pos;
                    targets_transform[i].transform.localEulerAngles = angle;
                    targets_transform[i].transform.localScale = scale;
                }
            }
        }

        else if (deployMode == DeployMode.Random)
        {
            if (deployType == DeployType.Range)
            {
                for (int i = 0; i < targets_transform.Count; i++)
                {
                    float ratio = Random.Range(0f, 1f);
                    float posX = Mathf.Lerp(startPosition.x, endPosition.x, ratio);
                    ratio = Random.Range(0f, 1f);
                    float posY = Mathf.Lerp(startPosition.y, endPosition.y, ratio);
                    ratio = Random.Range(0f, 1f);
                    float posZ = Mathf.Lerp(startPosition.z, endPosition.z, ratio);
                    targets_transform[i].transform.position = new Vector3(posX, posY, posZ);

                    ratio = Random.Range(0f, 1f);
                    float angleX = Mathf.Lerp(startAngle.x, endAngle.x, ratio);
                    ratio = Random.Range(0f, 1f);
                    float angleY = Mathf.Lerp(startAngle.y, endAngle.y, ratio);
                    ratio = Random.Range(0f, 1f);
                    float angleZ = Mathf.Lerp(startAngle.z, endAngle.z, ratio);
                    targets_transform[i].transform.localEulerAngles = new Vector3(angleX, angleY, angleZ);

                    ratio = Random.Range(0f, 1f);
                    float scaleX = Mathf.Lerp(startScale.x, endScale.x, ratio);
                    ratio = Random.Range(0f, 1f);
                    float scaleY = Mathf.Lerp(startScale.y, endScale.y, ratio);
                    ratio = Random.Range(0f, 1f);
                    float scaleZ = Mathf.Lerp(startScale.z, endScale.z, ratio);
                    targets_transform[i].transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                }
            }
            else if (deployType == DeployType.Sphere)
            {
                for (int i = 0; i < targets_transform.Count; i++)
                {
                    float radius = Random.Range(0, diameter / 2);

                    float radiansV = Random.Range(-Mathf.PI / 2, Mathf.PI / 2);
                    float radiansU = Random.Range(0, Mathf.PI * 2);
                    float posX = radius * Mathf.Cos(radiansV) * Mathf.Cos(radiansU);
                    float posY = radius * Mathf.Cos(radiansV) * Mathf.Sin(radiansU);
                    float posZ = radius * Mathf.Sin(radiansV);

                    targets_transform[i].transform.position = new Vector3(posX, posY, posZ) + centerPosition;
                }
            }
            else if (deployType == DeployType.Individual)
            {
                for (int i = 0; i < targets_transform.Count; i++)
                {
                    float ratio = Random.Range(0f, 1f);
                    float value = Mathf.Lerp(startValue, endValue, ratio);

                    Vector3 pos = targets_transform[i].transform.position;
                    Vector3 angle = targets_transform[i].transform.localEulerAngles;
                    Vector3 scale = targets_transform[i].transform.localScale;

                    if (deployAxis == DeployAxis.PosX) pos.x = value;
                    else if (deployAxis == DeployAxis.AngleX) angle.x = value;
                    else if (deployAxis == DeployAxis.ScaleX) scale.x = value;

                    else if (deployAxis == DeployAxis.PosY) pos.y = value;
                    else if (deployAxis == DeployAxis.AngleY) angle.y = value;
                    else if (deployAxis == DeployAxis.ScaleY) scale.y = value;

                    else if (deployAxis == DeployAxis.PosZ) pos.z = value;
                    else if (deployAxis == DeployAxis.AngleZ) angle.z = value;
                    else if (deployAxis == DeployAxis.ScaleZ) scale.z = value;

                    targets_transform[i].transform.position = pos;
                    targets_transform[i].transform.localEulerAngles = angle;
                    targets_transform[i].transform.localScale = scale;
                }
            }
        }
    }

    public void Reset()
    {
        List<Transform> targets_transform = new List<Transform>();
        if (deployTarget == DeployTarget.Direct) targets_transform = targetObjects_transform;
        else if (deployTarget == DeployTarget.Children)
        {
            for (int i = 0; i < targetObjectsParent_transform.childCount; i++)
            {
                Transform target_transform = targetObjectsParent_transform.GetChild(i);
                targets_transform.Add(target_transform);
            }
        }

        for (int i = 0; i < targets_transform.Count; i++)
        {
            targets_transform[i].transform.position = new Vector3(0, 0, 0);
            targets_transform[i].transform.localEulerAngles = new Vector3(0, 0, 0);
            targets_transform[i].transform.localScale = new Vector3(0, 0, 0);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TransformDeployer))]
public class TransformDeployerEditor : Editor
{
    ReorderableList targetObjects_reorderableList;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        TransformDeployer transformDeployer = (TransformDeployer)target;

        transformDeployer.deployTarget = (DeployTarget)EditorGUILayout.EnumPopup("ターゲット設定", transformDeployer.deployTarget);

        if (transformDeployer.deployTarget == DeployTarget.Direct)
        {
            SerializedProperty list_serializedProperty = serializedObject.FindProperty("targetObjects_transform");
            if (targetObjects_reorderableList == null) targetObjects_reorderableList = new ReorderableList(serializedObject, list_serializedProperty);
            targetObjects_reorderableList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "対象のオブジェクト");
            targetObjects_reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var elementProperty = list_serializedProperty.GetArrayElementAtIndex(index);
                rect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, elementProperty, new GUIContent("要素" + index));
            };
            targetObjects_reorderableList.DoLayoutList();
        }
        else if (transformDeployer.deployTarget == DeployTarget.Children)
        {
            transformDeployer.targetObjectsParent_transform = (Transform)EditorGUILayout.ObjectField(label: "ターゲットの親", obj: transformDeployer.targetObjectsParent_transform, objType: typeof(Transform), allowSceneObjects: true);
        }

        EditorGUILayout.Space();

        transformDeployer.deployMode = (DeployMode)EditorGUILayout.EnumPopup("配置モード", transformDeployer.deployMode);

        EditorGUILayout.Space();

        transformDeployer.deployType = (DeployType)EditorGUILayout.EnumPopup("種類", transformDeployer.deployType);

        EditorGUILayout.Space();

        if (transformDeployer.deployType == DeployType.Range)
        {
            EditorGUILayout.LabelField("開始");
            transformDeployer.startPosition = EditorGUILayout.Vector3Field("Position", transformDeployer.startPosition);
            transformDeployer.startAngle = EditorGUILayout.Vector3Field("Rotation", transformDeployer.startAngle);
            transformDeployer.startScale = EditorGUILayout.Vector3Field("Scale", transformDeployer.startScale);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("終了");
            transformDeployer.endPosition = EditorGUILayout.Vector3Field("Position", transformDeployer.endPosition);
            transformDeployer.endAngle = EditorGUILayout.Vector3Field("Rotation", transformDeployer.endAngle);
            transformDeployer.endScale = EditorGUILayout.Vector3Field("Scale", transformDeployer.endScale);
        }
        else if (transformDeployer.deployType == DeployType.Sphere)
        {
            transformDeployer.centerPosition = EditorGUILayout.Vector3Field("中心", transformDeployer.centerPosition);
            transformDeployer.diameter = EditorGUILayout.FloatField("直径", transformDeployer.diameter);
        }
        else if (transformDeployer.deployType == DeployType.Individual)
        {
            transformDeployer.deployAxis = (DeployAxis)EditorGUILayout.EnumPopup("軸", transformDeployer.deployAxis);

            EditorGUILayout.Space();

            transformDeployer.startValue = EditorGUILayout.FloatField("開始値", transformDeployer.startValue);
            transformDeployer.endValue = EditorGUILayout.FloatField("終了値", transformDeployer.endValue);
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        if (GUILayout.Button("配置"))
        {
            transformDeployer.Deploy();
        }
        if (GUILayout.Button("リセット"))
        {
            transformDeployer.Reset();
        }
    }
}
#endif