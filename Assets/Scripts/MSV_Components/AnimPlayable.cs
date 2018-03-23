using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class AnimPlayable : MSV_Component
{

    [SerializeField]
    private AnimationClip clip;
    PlayableGraph playableGraph;

    override public int Priority {
        get {
            return -800;
        }
    }

    override public MSV_Start StartDelegate {
        get {
            return new MSV_Start(AnimStart);
        }
    }


    //override public MSV_Start StartDelegate {
    //    get {
    //        return new MSV_Start(AnimStart);
    //    }
    //}

    //override public MSV_Update UpdateDelegate {
    //    get {
    //        return new MSV_Update(AnimUpdate);
    //    }
    //}

    private void AnimStart() {
        AnimationPlayableUtilities.PlayClip(GetComponent<Animator>(), clip, out playableGraph);

    }

    //private void AnimUpdate() {
    //    throw new NotImplementedException();
    //}
}
