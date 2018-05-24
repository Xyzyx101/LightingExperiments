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
    [SerializeField]
    int _Priority = -1000;

    Animator Anim;
    PlayableGraph PlayableGraph;
    PlayableOutput PlayableOutput;
    Dictionary<string, AnimationClipPlayable> ClipPlayables;

    override public int Priority {
        get {
            return _Priority;
        }
    }

    override public MSV_Awake AwakeDelegate {
        get {
            return new MSV_Awake(AnimatorAwake);
        }
    }

    public void Play(string name) {
        PlayableGraph.Stop();
        AnimationClipPlayable clipPlayable;
        if( ClipPlayables.TryGetValue(name, out clipPlayable) ) {
            //Debug.Log("Play : " + name);
            PlayableOutput.SetSourcePlayable(clipPlayable);
            PlayableGraph.Play();
        } else {
            Debug.LogWarning("Attempting to play animation " + name + " failed.  Cannot be found.");
        }
    }

    public void PauseAnim(string name) {
        AnimationClipPlayable clipPlayable;
        if( ClipPlayables.TryGetValue(name, out clipPlayable) ) {
            Debug.Log("Pause : " + name);
            clipPlayable.Pause();
        } else {
            Debug.LogWarning("Attempting to pause animation " + name + " failed.  Cannot be found.");
        }
    }

    // Set time in seconds
    public void SetTime(string name, float time) {
        AnimationClipPlayable clipPlayable;
        if( ClipPlayables.TryGetValue(name, out clipPlayable) ) {
            Debug.Log("Pause : " + name);
            clipPlayable.SetTime(time);
        } else {
            Debug.LogWarning("Attempting to set time on " + name + " failed.  Cannot be found.");
        }
    }

    // Set time percentage 0 to 1
    public void SetTime01(string name, float alpha) {
        AnimationClipPlayable clipPlayable;
        if( ClipPlayables.TryGetValue(name, out clipPlayable) ) {
            Debug.Log("Pause : " + name);
            double duration = clipPlayable.GetAnimationClip().length;
            clipPlayable.SetTime(alpha * duration);
        } else {
            Debug.LogWarning("Attempting to set time on " + name + " failed.  Cannot be found.");
        }
    }

    private void AnimatorAwake() {
        PlayableGraph = PlayableGraph.Create();
        Anim = GetParentActor().GetComponentInChildren<Animator>();
        Debug.Assert(Anim != null, "Animator not found.  Player Animation will not work.");
        PlayableOutput = AnimationPlayableOutput.Create(PlayableGraph, "Animation", Anim);
        CreateAllClipPlayables();
        PlayableOutput.SetSourcePlayable(ClipPlayables["sideJump"]);
        PlayableGraph.Play();
        GraphVisualizerClient.Show(PlayableGraph, "MSV_Animator");
    }

    private void CreateAllClipPlayables() {
        ClipPlayables = new Dictionary<string, AnimationClipPlayable>();
        foreach( AnimationClip clip in Clips ) {
            var clipPlayable = AnimationClipPlayable.Create(PlayableGraph, clip);
            ClipPlayables.Add(GetName(clip.name), clipPlayable);
        }
    }

    private string GetName(string name) {
        string[] words = name.Split('_');
        if( words.Length == 0 ) {
            return "";
        } else if( words.Length == 1 ) {
            return words[0];
        } else {
            return words[1];
        }
    }

    void OnDisable() {
        PlayableGraph.Destroy();
    }
}
