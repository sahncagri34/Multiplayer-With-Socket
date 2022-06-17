using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseManager {

    public AudioManager(GameFacade facade) : base(facade) { }
    private const string Sound_Prefix = "Sounds/";
    public const string Sound_Alert = "Alert";
    public const string Sound_BulletShoot = "BulletShoot";
    public const string Sound_Bg_Fast = "Bg(fast)";
    public const string Sound_Bg_Moderate = "Bg(moderate)";
    public const string Sound_ButtonClick = "ButtonClick";
    public const string Sound_Miss = "Miss";
    public const string Sound_ShootPerson = "ShootPerson";
    public const string Sound_Timer = "Timer";

    public string testStr = "123130";

    private AudioSource bgAudioSource;
    private AudioSource normalAudioSource;

    public override void OnInit() {
        GameObject audioSourceGO = new GameObject("AudioSource(GameObject)");
        bgAudioSource = audioSourceGO.AddComponent<AudioSource>();
        normalAudioSource = audioSourceGO.AddComponent<AudioSource>();
    }

    public void PlayBgSound(string soundName) {
       
    }
    public void PlayNormalSound(string soundName) {
        
    }

    private void PlaySound(AudioSource audioSource, AudioClip clip, float volume, bool loop = false) {
       
    }
  
}
