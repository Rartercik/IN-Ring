using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game.Utilities
{
    public class AvatarMaker
    {
        private static readonly Dictionary<string, string> BoneNames = new Dictionary<string, string>()
        {
            { "Head", "Head" },
            { "Hips", "Pelvis" },
            { "Chest", "Spine2" },
            { "Spine", "Spine1" },
            { "LeftUpperArm", "LeftUpArm" },
            { "LeftLowerArm", "LeftDownArm" },
            { "LeftHand", "LeftHand" },
            { "RightUpperArm", "RightUpArm" },
            { "RightLowerArm", "RightDownArm" },
            { "RightHand", "RightHand" },
            { "LeftUpperLeg", "LeftUpLeg" },
            { "LeftLowerLeg", "LeftDownLeg" },
            { "LeftFoot", "LeftFoot" },
            { "RightUpperLeg", "RightUpLeg" },
            { "RightLowerLeg", "RightDownLeg" },
            { "RightFoot", "RightFoot" }
        };

        private static readonly string[] AdditionalBoneNames = new string[]
        {
            "Legs"
        };

        private static readonly int RECURSION_DEPTH_LIMIT = 50;

        [MenuItem("CustomTools/MakeHumanAvatar")]
        private static void MakeHumanAvatar()
        {
            var activeGameObject = Selection.activeGameObject;
            if (activeGameObject == null) return;

            var humanBones = CreateHumanBones(BoneNames.Keys);
            var boneNames = BoneNames.Values.ToList();
            var skeletonBones = CreateSkeletonBones(humanBones.Length, activeGameObject, boneNames, AdditionalBoneNames);

            var humanDescription = new HumanDescription
            {
                upperArmTwist = 0.5f,
                lowerArmTwist = 0.5f,
                upperLegTwist = 0.5f,
                lowerLegTwist = 0.5f,
                armStretch = 0.5f,
                legStretch = 0.5f,
                feetSpacing = 0.0f,
                hasTranslationDoF = false,
                human = humanBones,
                skeleton = skeletonBones
            };

            var avatar = AvatarBuilder.BuildHumanAvatar(activeGameObject, humanDescription);
            avatar.name = activeGameObject.name;
            Debug.Log("Avatar is valid: " + avatar.isValid);
            Debug.Log(avatar.isHuman ? "is human" : "is generic");

            var path = string.Format("Assets/{0}.ht", avatar.name.Replace(':', '_'));
            AssetDatabase.CreateAsset(avatar, path);
        }

        private static HumanBone[] CreateHumanBones(IEnumerable<string> humanNames)
        {
            var humanBones = new List<HumanBone>();

            foreach(var humanName in humanNames)
            {
                var humanBone = new HumanBone
                {
                    boneName = BoneNames[humanName],
                    humanName = humanName
                };

                humanBones.Add(humanBone);
            }

            return humanBones.ToArray();
        }

        private static SkeletonBone[] CreateSkeletonBones(int humanBonesCount, GameObject root,
            List<string> boneNames, IEnumerable<string> additionalBones)
        {
            var skeletonBones = new List<SkeletonBone>();
            var rootBone = new SkeletonBone
            {
                name = root.name,
                position = Vector3.zero,
                rotation = Quaternion.identity,
                scale = Vector3.one
            };
            skeletonBones.Add(rootBone);

            for (var i = 0; i < humanBonesCount; i++)
            {
                var boneName = boneNames[i];
                GetBoneOffsetFromRoot(root, boneName, out var position, out var rotation, out var scale);

                var bone = new SkeletonBone
                {
                    name = boneName,
                    position = position,
                    rotation = rotation,
                    scale = scale
                };
                skeletonBones.Add(bone);
            }

            foreach (var boneName in additionalBones)
            {
                GetBoneOffsetFromRoot(root, boneName, out var position, out var rotation, out var scale);

                var bone = new SkeletonBone
                {
                    name = boneName,
                    position = position,
                    rotation = rotation,
                    scale = scale
                };
                skeletonBones.Add(bone);
            }

            return skeletonBones.ToArray();
        }

        private static void GetBoneOffsetFromRoot(GameObject rootGameObject, string boneName,
            out Vector3 position,
            out Quaternion rotation,
            out Vector3 scale
        )
        {
            Debug.Log(boneName);
            var boneTransform = RecursiveFindChild(rootGameObject.transform, boneName);
            var boneLocalPosition = boneTransform.localPosition;
            var boneLocalRotation = boneTransform.localRotation;
            var boneLocalScale = boneTransform.localScale;

            GetGameObjectOffsetFrom(
                rootGameObject.transform,
                boneTransform,
                in boneLocalPosition,
                in boneLocalRotation,
                in boneLocalScale,
                out position,
                out rotation,
                out scale,
                0
            );
        }

        private static void GetGameObjectOffsetFrom(
            Transform ancestorTransform,
            Transform gameObjectTransform,
            in Vector3 inputPosition,
            in Quaternion inputRotation,
            in Vector3 inputScale,
            out Vector3 position,
            out Quaternion rotation,
            out Vector3 scale,
            int recursionDepth
        )
        {
            if (recursionDepth > RECURSION_DEPTH_LIMIT)
            {
                throw new UnityException(
                    "Making a human avatar failed due to incorrect" +
                    " structure of the GameObject's hierarchy"
                );
            }

            if (ancestorTransform == gameObjectTransform)
            {
                position = inputPosition;
                rotation = inputRotation;
                scale = inputScale;
                return;
            }
            else
            {
                var parentTransform = gameObjectTransform.parent;
                var newPosition = inputPosition + parentTransform.localPosition;
                var newRotation = inputRotation * parentTransform.localRotation;
                var newScale = inputScale;
                newScale.Scale(parentTransform.localScale);
                GetGameObjectOffsetFrom(
                    ancestorTransform,
                    parentTransform,
                    in newPosition,
                    in newRotation,
                    in newScale,
                    out position,
                    out rotation,
                    out scale,
                    recursionDepth
                );
                return;
            }
        }

        private static Transform RecursiveFindChild(Transform parent, string childName)
        {
            foreach (Transform child in parent)
            {
                if (child.name.Equals(childName))
                {
                    return child;
                }
                else
                {
                    var found = RecursiveFindChild(child, childName);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }
            return null;
        }
    }
}
