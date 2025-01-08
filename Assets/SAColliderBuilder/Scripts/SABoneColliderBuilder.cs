//----------------------------------------------
// SABoneCollider
// Copyright (c) 2014 Stereoarts Nora
//----------------------------------------------
using UnityEngine;
using ColliderProperty = SAColliderBuilderCommon.ColliderProperty;
using ReducerProperty = SAColliderBuilderCommon.ReducerProperty;
using RigidbodyProperty = SAColliderBuilderCommon.RigidbodyProperty;
using SABoneColliderBuilderProperty = SABoneColliderCommon.SABoneColliderBuilderProperty;
using SplitProperty = SABoneColliderCommon.SplitProperty;

public class SABoneColliderBuilder : MonoBehaviour
{
    public SABoneColliderBuilderProperty boneColliderBuilderProperty = new SABoneColliderBuilderProperty();

    [System.NonSerialized]
    public SABoneColliderBuilderProperty edittingBoneColliderBuilderProperty = null;

    [System.NonSerialized]
    public bool cleanupModified = false;

    [System.NonSerialized]
    public bool isDebug = false;

    public SplitProperty splitProperty
    { get { return (boneColliderBuilderProperty != null) ? boneColliderBuilderProperty.splitProperty : null; } }
    public ReducerProperty reducerProperty
    { get { return (boneColliderBuilderProperty != null) ? boneColliderBuilderProperty.reducerProperty : null; } }
    public ColliderProperty colliderProperty
    { get { return (boneColliderBuilderProperty != null) ? boneColliderBuilderProperty.colliderProperty : null; } }
    public RigidbodyProperty rigidbodyProperty
    { get { return (boneColliderBuilderProperty != null) ? boneColliderBuilderProperty.rigidbodyProperty : null; } }
}