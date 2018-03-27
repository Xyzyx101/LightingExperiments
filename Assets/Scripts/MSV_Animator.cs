using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class MSV_Animator : MSV_Component
{
    [SerializeField]
    AnimationClip[] Clips;
    Animator Anim;
    PlayableGraph PlayableGraph;
    Dictionary<string, AnimationClipPlayable> ClipPlayables;

    override public int Priority {
        get {
            return -1000;
        }
    }

    override public MSV_Awake AwakeDelegate {
        get {
            return new MSV_Awake(AnimatorAwake);
        }
    }

    private void AnimatorAwake() {
        throw new NotImplementedException();
    }
}
