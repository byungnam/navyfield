/*
 * Copyright (c) 2016, Ivo van der Marel
 * Released under MIT License (= free to be used for anything)
 * Enjoy :)
 */

using UnityEngine;

public interface IRTSSelectable {

    void OnEnable();

    void OnDisable();

    bool IsSelected {
        get;
        set;
    }

    Transform transform {
        get;
    }
}

public class RTSSelectable : IRTSSelectable {
    public RTSSelectable(Transform transform) {
        this.transform = transform;
    }

    public bool IsSelected {
        get;
        set;
    }

    public Transform transform {
        get;
    }

    public void OnEnable() {
        RTSSelection.selectables.Add(this);
    }

    public void OnDisable() {
        RTSSelection.selectables.Remove(this);
    }
}