using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CustomAnimPlayableBehaviour : PlayableBehaviour
{
    public Animator newExposedReference;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }
}
