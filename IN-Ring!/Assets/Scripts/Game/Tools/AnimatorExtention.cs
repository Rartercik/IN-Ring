using UnityEngine;

namespace Game.Tools
{
    public static class AnimatorExtention
    {
        public static bool Contains(this Animator animator, AnimationClip targetClip)
        {
            var clips = animator.runtimeAnimatorController.animationClips;

            foreach (var clip in clips)
            {
                if (clip == targetClip)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
