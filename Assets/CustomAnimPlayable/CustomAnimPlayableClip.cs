using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CustomAnimPlayableClip : PlayableAsset, ITimelineClipAsset
{
    public CustomAnimPlayableBehaviour template = new CustomAnimPlayableBehaviour ();
    public ExposedReference<Animator> newExposedReference;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Looping | ClipCaps.SpeedMultiplier; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CustomAnimPlayableBehaviour>.Create (graph, template);
        CustomAnimPlayableBehaviour clone = playable.GetBehaviour ();
        clone.newExposedReference = newExposedReference.Resolve (graph.GetResolver ());
        return playable;
    }
}
