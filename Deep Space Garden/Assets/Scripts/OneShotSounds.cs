using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OneShotSounds : MonoBehaviour {
	public void PlaySound (AudioClip clip, Vector3 pos, float pitch) {
		if (clip == null)
			return;
		AudioSource s = GetAudioSource (pos);
		s.clip = clip;
		s.loop = false;
		s.pitch = pitch;
		s.Play ();
		StartCoroutine (PoolSound (s));
	}

	public void PlaySound (AudioClip clip, Vector3 pos) {
		PlaySound (clip, pos, 1);
	}

	AudioSource GetAudioSource (Vector3 position) {
		AudioSource ret;
		GameObject g;
		if (pool.Count > 0) {
			ret = pool.Pop ();
			g = ret.gameObject;
		} else {
			g = new GameObject ();
			g.transform.SetParent (transform);
			ret = g.AddComponent<AudioSource> ();
		}
		g.SetActive (true);
		g.transform.position = position;
		return ret;
	}

	IEnumerator PoolSound (AudioSource source) {
		yield return new WaitForSeconds (source.clip.length);
		PoolSoundForced (source);
	}

	void PoolSoundForced (AudioSource source) {
		source.gameObject.SetActive (false);
		pool.Push (source);
	}

	Stack<AudioSource> pool = new Stack<AudioSource> ();

	public Action PlayLoopingSoundWithStopCallback (AudioClip clip, Vector3 pos) {
		if (clip == null)
			return null;
		AudioSource s = GetAudioSource (pos);
		s.clip = clip;
		s.loop = true;
		s.Play ();
		return () => PoolSoundForced (s);
	}
}
