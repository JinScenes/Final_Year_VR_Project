using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnGrab : GrabbableEvents {

    public AudioClip SoundToPlay;

    public override void OnGrab(Grabber grabber) {

        // Play Sound
        if(SoundToPlay) {
            XRManager.Instance.PlaySpatialClipAt(SoundToPlay, transform.position, 1f, 1f);
        }

        base.OnGrab(grabber);
    }
}