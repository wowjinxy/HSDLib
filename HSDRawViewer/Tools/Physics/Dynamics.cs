using HSDRaw.Common;
using HSDRaw.Melee.Pl;
using HSDRawViewer.Rendering;
using HSDRawViewer.Rendering.Models;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace HSDRawViewer.Tools.Physics
{
    public class Dynamics
    {
        public int ApplyNum;

        public int BoneCount => Bones.Count;

        public int StartBone;

        public readonly List<DynamicJoint> Bones = new();

        public Vector3 Gravity = new(0, -1, 0);

        public Vector3 Wind = Vector3.Zero;

        public float DampingMultiplier;
        public float StiffnessMultiplier;
        public float BoneLengthScaleFactor;

        #region Initialization

        public void InitBones(LiveJObj root, int startIndex, int count)
        {
            Bones.Clear();

            LiveJObj live = root.GetJObjAtIndex(startIndex);

            if (live?.Desc == null)
                return;

            HSD_JOBJ desc = live.Desc;

            for (int i = 0; i < count && desc != null; i++)
            {
                DynamicJoint bone = new(live)
                {
                    WorldPosition = live.WorldTransform.ExtractTranslation()
                };

                Bones.Add(bone);
                live = live.Child;
            }

            CalculateBoneLengthsAndAxes();
        }

        public void CalculateBoneLengthsAndAxes()
        {
            for (int i = 0; i < Bones.Count - 1; i++)
            {
                DynamicJoint current = Bones[i];
                DynamicJoint next = Bones[i + 1];

                current.BoneLength =
                    Vector3.Distance(current.WorldPosition, next.WorldPosition);

                current.InverseBoneLength =
                    current.BoneLength <= 0
                    ? 0
                    : BoneLengthScaleFactor / current.BoneLength;

                Vector3 t = next.Translation;

                float ax = Math.Abs(t.X);
                float ay = Math.Abs(t.Y);
                float az = Math.Abs(t.Z);

                current.DominantAxis =
                    ax >= ay && ax >= az ? 0 :
                    ay >= az ? 1 :
                    2;
            }
        }

        public void InitParams(SBM_DynamicDesc desc)
        {
            StartBone               = desc.BoneIndex;
            DampingMultiplier       = desc.DragMultiplier;
            StiffnessMultiplier     = desc.StiffnessMultiplier;
            BoneLengthScaleFactor   = desc.GravityLengthCompensation;

            for (int i = 0; i < Bones.Count && i < desc.Parameters.Length; i++)
            {
                DynamicJoint bone = Bones[i];
                SBM_DynamicParams p = desc.Parameters[i];

                bone.FollowDamping = p.FollowDamping;

                bone.RestPoseStiffness =
                    p.Stiffness;

                bone.RestRotation = new Vector3(
                    p.RotX,
                    p.RotY,
                    p.RotZ);

                bone.RotationLimit =
                    p.RotationLimit;

                bone.InertiaDamping =
                    p.InertiaDamping;

                bone.Resistance =
                    p.Resistance;

                bone.InverseBoneLength =
                    bone.BoneLength <= 0
                    ? 0
                    : BoneLengthScaleFactor / bone.BoneLength;
            }
        }

        #endregion

        #region Simulation

        public void Think(
            LiveJObj model,
            IEnumerable<SBM_DynamicHitBubble> hitBubbles)
        {
            if (ApplyNum > 0x100 || Bones.Count <= 1)
                return;

            Matrix4 parentMatrix = Bones[0].Live.Parent.WorldTransform;

            for (int i = ApplyNum; i < Bones.Count - 1; i++)
            {
                SimulateBone(
                    Bones[i],
                    Bones[i + 1],
                    ref parentMatrix,
                    model,
                    hitBubbles);
            }

            UpdateFinalBonePosition(parentMatrix);

            ApplyRotations();
        }

        private void SimulateBone(
            DynamicJoint bone,
            DynamicJoint child,
            ref Matrix4 parentMatrix,
            LiveJObj model,
            IEnumerable<SBM_DynamicHitBubble> hitBubbles)
        {
            Matrix4 translationMatrix =
                Matrix4.CreateTranslation(bone.Translation);

            Matrix4 restRotationMatrix =
                Math3D.CreateMatrix4FromEuler(bone.RestRotation);

            Matrix4 currentRotationMatrix =
                Math3D.CreateMatrix4FromEuler(bone.Rotation);

            Matrix4 scaleMatrix =
                Matrix4.CreateScale(bone.Scale);

            Matrix4 restMatrix =
                scaleMatrix *
                (restRotationMatrix *
                (translationMatrix * parentMatrix));

            Matrix4 currentMatrix =
                scaleMatrix *
                (currentRotationMatrix *
                (translationMatrix * parentMatrix));

            bone.WorldPosition =
                currentMatrix.ExtractTranslation();

            Vector3 restDirection =
                GetChildDirection(restMatrix, child);

            Vector3 currentDirection =
                GetChildDirection(currentMatrix, child);

            Vector3 simulatedDirection =
                (child.WorldPosition - bone.WorldPosition)
                .Normalized();

            Vector3 previousDirection =
                simulatedDirection;

            ApplyDamping(
                bone,
                ref simulatedDirection,
                currentDirection);

            ApplyGravity(
                bone,
                ref simulatedDirection);

            //ApplyWind(
            //    bone,
            //    ref simulatedDirection);

            ApplyAngularVelocity(
                bone,
                ref simulatedDirection);

            ApplyMaxAngleChange(
                bone,
                previousDirection,
                ref simulatedDirection);

            ApplyRestPoseConstraint(
                bone,
                restDirection,
                ref simulatedDirection);

            ApplyRotationLimit(
                bone,
                restDirection,
                ref simulatedDirection);

            if (hitBubbles != null)
            {
                ApplyHitBubbleCollision(
                    bone,
                    ref simulatedDirection,
                    model,
                    hitBubbles);
            }

            UpdateAngularVelocity(
                bone,
                previousDirection,
                simulatedDirection);

            UpdateJointRotation(
                bone,
                parentMatrix,
                currentDirection,
                simulatedDirection);

            DampRotation(
                bone,
                bone.DominantAxis);

            parentMatrix =
                scaleMatrix *
                (Math3D.CreateMatrix4FromEuler(bone.Rotation) *
                (translationMatrix * parentMatrix));
        }

        #endregion

        #region Physics

        private void ApplyDamping(
            DynamicJoint bone,
            ref Vector3 direction,
            Vector3 targetDirection)
        {
            float strength =
                bone.FollowDamping * DampingMultiplier;

            if (strength >= 1f)
                return;

            RotateToward(
                ref direction,
                targetDirection,
                1f - strength);
        }

        private void ApplyWind(
            DynamicJoint bone,
            ref Vector3 direction)
        {
            if (Wind.LengthSquared <= float.Epsilon)
                return;

            Vector3 wind = Wind; //.Normalized();

            float angle =
                Vector3.CalculateAngle(wind, direction);

            if (angle <= 0)
                return;

            float gravityAngle =
                Math.Abs(
                    MathF.Sin(angle) *
                    bone.InverseBoneLength);

            Vector3 rotationAxis =
                Vector3.Cross(direction, wind)
                .Normalized();

            RotateAboutUnitAxis(
                ref direction,
                gravityAngle,
                rotationAxis);
        }

        private void ApplyGravity(
            DynamicJoint bone,
            ref Vector3 direction)
        {
            float angle =
                Vector3.CalculateAngle(Gravity, direction);

            if (angle <= 0)
                return;

            float gravityAngle =
                Math.Abs(
                    MathF.Sin(angle) *
                    bone.InverseBoneLength);

            Vector3 rotationAxis =
                Vector3.Cross(direction, Gravity)
                .Normalized();

            RotateAboutUnitAxis(
                ref direction,
                gravityAngle,
                rotationAxis);
        }

        private void ApplyAngularVelocity(
            DynamicJoint bone,
            ref Vector3 direction)
        {
            if (bone.AngularVelocity == 0)
                return;

            RotateAboutUnitAxis(
                ref direction,
                bone.AngularVelocity,
                bone.AngularAxis);
        }

        private void ApplyMaxAngleChange(
            DynamicJoint bone,
            Vector3 previousDirection,
            ref Vector3 direction)
        {
            if (bone.BoneLength <= 0)
                return;

            float angle =
                Vector3.CalculateAngle(
                    previousDirection,
                    direction);

            if (angle <= bone.Resistance)
                return;

            RotateToward(
                ref direction,
                previousDirection,
                bone.Resistance / angle);
        }

        private void ApplyRestPoseConstraint(
            DynamicJoint bone,
            Vector3 restDirection,
            ref Vector3 direction)
        {
            if (bone.RestPoseStiffness <= 0)
                return;

            float angle =
                Vector3.CalculateAngle(
                    direction,
                    restDirection);

            if (angle <= bone.RestPoseStiffness)
            {
                direction = restDirection;
                return;
            }

            RotateToward(
                ref direction,
                restDirection,
                bone.RestPoseStiffness / angle);
        }

        private void ApplyRotationLimit(
            DynamicJoint bone,
            Vector3 restDirection,
            ref Vector3 direction)
        {
            float angle =
                Vector3.CalculateAngle(
                    direction,
                    restDirection);

            if (angle <= bone.RotationLimit)
                return;

            RotateToward(
                ref direction,
                restDirection,
                (angle - bone.RotationLimit) / angle);
        }

        #endregion

        #region Collision

        private void ApplyHitBubbleCollision(
            DynamicJoint bone,
            ref Vector3 direction,
            LiveJObj model,
            IEnumerable<SBM_DynamicHitBubble> hitBubbles)
        {
            foreach (SBM_DynamicHitBubble hb in hitBubbles)
            {
                Vector3 end =
                    direction * bone.BoneLength +
                    bone.WorldPosition;

                Vector3 bubblePosition =
                    (Matrix4.CreateTranslation(
                        hb.X,
                        hb.Y,
                        hb.Z)
                    *
                    model.GetJObjAtIndex(hb.BoneIndex)
                    .WorldTransform)
                    .ExtractTranslation();

                Vector3 toBubble =
                    bubblePosition - bone.WorldPosition;

                float distance =
                    toBubble.LengthFast;

                if (distance <= hb.Size)
                    continue;

                float angle =
                    Vector3.CalculateAngle(
                        direction,
                        toBubble);

                if (angle <= 0)
                    continue;

                float radius =
                    0.1f + hb.Size;

                float threshold =
                    new Vector2(distance, radius)
                    .LengthFast;

                float reflect =
                    Math.Abs(
                        MathF.Atan2(radius, threshold))
                    - angle;

                if (reflect <= 0)
                    continue;

                Vector3 rotationAxis =
                    Vector3.Cross(toBubble, direction)
                    .Normalized();

                RotateAboutUnitAxis(
                    ref direction,
                    reflect,
                    rotationAxis);
            }
        }

        #endregion

        #region Rotation / Angular Velocity

        private void UpdateAngularVelocity(
            DynamicJoint bone,
            Vector3 previousDirection,
            Vector3 currentDirection)
        {
            bone.AngularVelocity =
                Vector3.CalculateAngle(
                    previousDirection,
                    currentDirection);

            if (bone.AngularVelocity > 0)
            {
                bone.AngularAxis =
                    Vector3.Cross(
                        previousDirection,
                        currentDirection)
                    .Normalized();
            }

            float decay =
                bone.InertiaDamping;

            if (bone.AngularVelocity > decay)
                bone.AngularVelocity -= decay;
            else if (bone.AngularVelocity < -decay)
                bone.AngularVelocity += decay;
            else
                bone.AngularVelocity = 0;
        }

        private void UpdateJointRotation(
            DynamicJoint joint,
            Matrix4 parentMatrix,
            Vector3 fromDirection,
            Vector3 toDirection)
        {
            float angle =
                Vector3.CalculateAngle(
                    fromDirection,
                    toDirection);

            if (Math.Abs(angle) <= 1e-5f)
                return;

            Vector3 rotationAxis =
                Vector3.Cross(
                    fromDirection,
                    toDirection)
                .Normalized();

            Matrix4 inverse =
                parentMatrix;

            inverse.Transpose();

            Vector3 localAxis =
                Vector3.TransformPosition(
                    rotationAxis,
                    inverse);

            Quaternion delta =
                Quaternion.FromAxisAngle(
                    localAxis,
                    angle);

            Quaternion current =
                Math3D.EulerToQuat(
                    joint.Rotation.X,
                    joint.Rotation.Y,
                    joint.Rotation.Z);

            Quaternion result =
                delta * current;

            Matrix4 rotation =
                Matrix4.CreateFromQuaternion(result);

            joint.Rotation =
                rotation.ExtractRotationEuler();
        }

        private static void DampRotation(
            DynamicJoint joint,
            int axis)
        {
            switch (axis)
            {
                case 0:
                    joint.Rotation.X *= 0.9f;
                    break;

                case 1:
                    joint.Rotation.Y *= 0.9f;
                    break;

                case 2:
                    joint.Rotation.Z *= 0.9f;
                    break;
            }
        }

        #endregion

        #region Utilities

        private static Vector3 GetChildDirection(
            Matrix4 matrix,
            DynamicJoint child)
        {
            return (
                Vector3.TransformPosition(
                    child.Translation,
                    matrix)
                - matrix.ExtractTranslation())
                .Normalized();
        }

        private static void RotateToward(
            ref Vector3 direction,
            Vector3 targetDirection,
            float amount)
        {
            float angle =
                Vector3.CalculateAngle(
                    direction,
                    targetDirection);

            if (angle <= 0)
                return;

            Vector3 rotationAxis =
                Vector3.Cross(
                    direction,
                    targetDirection);

            if (rotationAxis.LengthSquared <= 0)
                return;

            rotationAxis.Normalize();

            RotateAboutUnitAxis(
                ref direction,
                angle * amount,
                rotationAxis);
        }

        private void UpdateFinalBonePosition(
            Matrix4 matrix)
        {
            DynamicJoint joint = Bones[^1];

            matrix =
                Matrix4.CreateScale(joint.Scale) *
                (Math3D.CreateMatrix4FromEuler(joint.Rotation) *
                (Matrix4.CreateTranslation(joint.Translation) *
                matrix));

            joint.WorldPosition =
                matrix.ExtractTranslation();
        }

        private void ApplyRotations()
        {
            for (int i = ApplyNum; i < Bones.Count - 1; i++)
            {
                Bones[i].Live.Rotation =
                    new Vector4(
                        Bones[i].Rotation,
                        0);
            }
        }

        public static void RotateAboutUnitAxis(
            ref Vector3 direction,
            float angle,
            Vector3 rotationAxis)
        {
            float sin =
                MathF.Sin(angle);

            float cos =
                MathF.Cos(angle);

            Vector3 rotated =
                direction * cos +
                Vector3.Cross(rotationAxis, direction) * sin +
                rotationAxis *
                Vector3.Dot(rotationAxis, direction) *
                (1f - cos);

            direction = rotated;
        }

        #endregion
    }
}