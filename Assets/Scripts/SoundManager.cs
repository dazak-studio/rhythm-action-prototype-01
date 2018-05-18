using UnityEngine;

public partial class SoundManager : Singleton<SoundManager>
{             
    public void Initialize(int stage)
    {
        BgmAudioSource = GetComponent<AudioSource>();
        
        SetBgm(stage);
    }

    private void SetBgm(int stage)
    {          
        BgmInfo = LoadManager.GetInstance.LoadBgm(1);

        BgmAudioSource.clip = BgmInfo.AudioClip;        
        BgmAudioSource.Play();
    }                

    public void CleanUp()
    { 
        BgmAudioSource.Stop();
    }

    public AudioSource BgmAudioSource { get; private set; }

    public Bgm BgmInfo { get; private set; }        
}
