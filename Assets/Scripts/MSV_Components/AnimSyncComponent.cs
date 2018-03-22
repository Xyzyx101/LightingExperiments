using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSyncComponent : MSV_Component
{
    [System.Serializable]
    private class AnimSyncLayer
    {
        public string Name;
        public Texture2D Tex;
        public int SortOrder;
        public SpriteRenderer Rend;
    }

    [SerializeField]
    private List<AnimSyncLayer> SyncLayers;

    private SpriteRenderer MasterRenderer;

    private void Awake() {
        MasterRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(MasterRenderer != null, "AnimSyncComponent must be on an object with a SpriteRenderer. Sprite Sync will fail.");
    }

    override public int Priority {
        get {
            return -1000;
        }
    }

    override public MSV_Start StartDelegate {
        get {
            return new MSV_Start(AnimStart);
        }
    }

    override public MSV_Update UpdateDelegate {
        get {
            return new MSV_Update(AnimUpdate);
        }
    }

    public void AddLayer(string name, Texture2D tex, int sortOrder = 0) {
        var layerIdx = SyncLayers.FindIndex(datum => datum.Name == name);
        if( layerIdx == -1 ) {
            var newLayer = new AnimSyncLayer() { Name = name, Tex = tex, SortOrder = sortOrder };
            CreateSyncLayer(newLayer);
            SyncLayers.Add(newLayer);
        } else {
            Debug.LogError("Layer " + name + " already exist.  Use SetLayerTexture if you want tot change the texture.");
        }
    }

    public void SetLayerTexture(string name, Texture2D tex) {
        var layerIdx = SyncLayers.FindIndex(datum => datum.Name == name);
        if( layerIdx == -1 ) {
            Debug.LogError("Sync layer " + name + " is not found.  Anim sync is broken.");
        } else {
            var oldSprite = SyncLayers[layerIdx].Rend.sprite;
            SyncLayers[layerIdx].Rend.sprite = CreateSprite(tex);
        }
    }

    private void AnimStart() {
        for( int i = 0; i < SyncLayers.Count; ++i ) {
            if( SyncLayers[i].Rend == null ) {
                CreateSyncLayer(SyncLayers[i]);
            }
        }
    }

    private void AnimUpdate() {
        for( int i = 0; i < SyncLayers.Count; ++i ) {
            if( SyncLayers[i].Rend != null ) {
                SyncLayers[i].Rend.sprite = CreateSprite(SyncLayers[i].Tex);
            }
        }
    }

    private void CreateSyncLayer(AnimSyncLayer layer) {
        var go = GameObject.Instantiate(new GameObject(), transform);
        go.name = "AnimSyncLayer--" + layer.Name;
        layer.Rend = go.AddComponent<SpriteRenderer>();
        layer.Rend.material = MasterRenderer.material;
        layer.Rend.sortingLayerID = MasterRenderer.sortingLayerID;
        layer.Rend.sortingOrder = layer.SortOrder;
        layer.Rend.sprite = CreateSprite(layer.Tex);
    }

    private Sprite CreateSprite(Texture2D tex) {
        Debug.Assert(MasterRenderer != null, "AnimSyncComponent must be on an object with a SpriteRenderer. Sprite Sync will fail.");
        var pivot = MasterRenderer.sprite.pivot;
        var rect = MasterRenderer.sprite.rect;
        var scaledPivot = new Vector2(pivot.x / rect.width, pivot.y / rect.height);
        return Sprite.Create(tex, rect, scaledPivot, MasterRenderer.sprite.pixelsPerUnit, 0, SpriteMeshType.Tight);
    }
}
