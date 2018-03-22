using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIComponent : MSV_Component
{
    override public int Priority {
        get {
            return 1000;
        }
    }

    override public MSV_Start StartDelegate {
        get {
            return new MSV_Start(AIStart);
        }
    }

    private void AIStart() {
        Debug.Log("AI should run first");
    }
}
