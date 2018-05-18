using UnityEngine;

public partial class SoundManager
{
    public class Bgm
    {
        public Bgm()
        {
            AudioClip = null;
            Offset = 0;
            Bpm = 0;
        }
        
        public Bgm(AudioClip audioClip, float bpm, float offset)
        {                
            AudioClip = audioClip;
            Bpm = bpm;
            Offset = offset;
        }

        public readonly AudioClip AudioClip;
        public readonly float Bpm;
        public readonly float Offset;
    }
}