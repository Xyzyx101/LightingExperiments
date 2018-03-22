using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderComponent : MSV_Component
{
    override public int Priority {
        get {
            return 100;
        }
    }

    override public MSV_Start StartDelegate {
        get {
            return new MSV_Start(PathStart);
        }
    }

    private void PathStart() {
        Debug.Log("Pathfinding should run go in the middle");
    }
}
