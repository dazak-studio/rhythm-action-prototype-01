using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bgm = SoundManager.Bgm;

public partial class LoadManager : Singleton<LoadManager>
{             
    // 로드 매니저는 씬 로드에서 메서드 트리거 역할을 담고 있다.
    // The load manager contains a method trigger role in the scene load.
    protected override void Awake()
    {            
        // 로드 시 이 매니저를 해체하지 않음
        DontDestroyOnLoad(this);
        
        base.Awake();                        
        Initialize();

        StartCoroutine(LoadResourcesCoroutine()); 

        // @TODO: 임시로 만들어 둔 로드 확인 방법인데, 이후 해시 연산을 통해 비교하도록 하고 싶다.
        // @TODO: This is a load check method that I made temporarily, but I want to compare it with a hash operation later.
       // if (_tileMapInfos.Count == 0 ||
        //    _backgroudMusics.Count == 0)
       // {                                
       //     DL.Write(DL.Type.Execute);
       //     StartCoroutine(LoadResourcesCoroutine());                
       // }
        // 무결성이 보장되었고, 모든 리소스의 로드가 완료된 경우,
        // If integrity is guaranteed and all resources have been loaded,
      //  else
      //  {
       //     DL.Write(DL.Type.Branch);
       //     GameManager.GetInstance.Initialize();                
      //  }                        
    }

    private void Initialize()
    {
        // @TODO: 최적화 이후 무결성을 보장할 수 있는 방법을 강구해야만한다.
        _backgroudMusics = _backgroudMusics ?? new List<Bgm>();
        //_tileMapInfos = _tileMapInfos ?? new List<TileMapInfo>();            
    }
    
    private IEnumerator LoadResourcesCoroutine()
    {
        // 로드 플래그를 아무것도 로드되지 않은 상태로 설정
        // Set the load flag to nothing loaded
        _loadFlags = GetLoadFlagValue(LoadFlag.Count) - 1;
        
        // BGM 리소스 로드
        // Resource load: BGM
        StartCoroutine(LoadBackgroundMusicCoroutine());
        // 맵 리소스 로드
        // Resource load: MAP
        StartCoroutine(LoadMapCoroutine());
        
        // 모든 리소스가 로드될 때까지 기다린다.
        // Wait for those coroutine have been ready.
        while (_loadFlags > 0 || _errorFlags != 0)
        {
            // @TODO # 1: 로드 현황을 사용자에게 보여줄 필요가 있음.
            // @TODO # 1: I need to show the load status to the user.
            // @TODO # 2: 에러 플래그가 올라간 경우 처리할 방법에 대해서 강구할 필요가 있음.
            // @TODO # 2: You need to find out how to handle the error flag when it goes up.
            yield return new WaitForSeconds(0.5f);
        }
        

        Debug.Log("All Resources are loaded..");

        // 모든 리소스가 로드된 경우,
        // All resources are loaded, 
        SoundManager.GetInstance.Initialize(stage: 1);
        RhythmManager.GetInstance.Initialize();
    }

    private IEnumerator LoadBackgroundMusicCoroutine()
    {
        // 사용하는 백그라운드 뮤직들에 대한 리스트
        var bgmExtenders = new List<BgmExtender>
        {
            new BgmExtender("All_of_us", 120f, 0f), 
            new BgmExtender("Night_in_Katakum", 115f, 0f)

            //new BgmExtender("We're_the_Resistors", 117f, 0.15f)
        };

        // 각각의 뮤직 파일들을 차례차례 리스트에 로드
        foreach (var bgmExtender in bgmExtenders)
        {
            // 정적 파일 할당
            var staticMusicFile =
                new WWW(@"file://" + GetPlatformDataPath() + 
                        @"/Resources/Sounds/Musics/" + bgmExtender.Name + @".mp3");
            
            // 정적 파일 할당을 기다림
            while (!staticMusicFile.isDone)
            {
                yield return new WaitForSeconds(1.0f);
            }
            
            // 정적으로 할당된 파일을 동적으로 로드
            var audioClip = (AudioClip)Resources.Load("Sounds/Musics/" + bgmExtender.Name, typeof(AudioClip));
            
            // 동적으로 로드한 파일의 값이 없는 경우
            if (audioClip == null)
            {              
                _errorFlags |= GetLoadFlagValue(LoadFlag.Bgm);
                _backgroudMusics.Add(null);
            }
            // 동적으로 성공적으로 로드에 성공한 경우
            else
            {                
                _backgroudMusics.Add(new Bgm(audioClip, bgmExtender.Bpm, bgmExtender.Offset));
            }                
        }
        
        // 로드 완료를 알리는 플래그 업
        _loadFlags ^= GetLoadFlagValue(LoadFlag.Bgm);
        
        yield return null;
    }

    private IEnumerator LoadMapCoroutine()
    {
        yield return null;
    }
    
    // @TODO: 스테이지/라운드 별 오디오 클립 로드 방법에 대해서 강구해봐야함.
    public Bgm LoadBgm(int musicNumber)
    {
        return _backgroudMusics[musicNumber];
    }
    
    

    // 플랫폼 별 리소스 위치가 다르므로 고유 리소스 위치를 참조
    private static string GetPlatformDataPath()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                return Application.persistentDataPath;
            default:
                return Application.dataPath;
        }
    }
    
    private struct BgmExtender
    {
        public BgmExtender(string name, float bpm, float offset)
        {
            Name = name;
            Bpm = bpm;
            Offset = offset;
        }
        
        public readonly string Name;
        public readonly float Bpm;
        public readonly float Offset;
    }
    
    // 맵 별 타일 정보
    //private List<TileMapInfo> _tileMapInfos;
    // 음악 클립 정보
    private List<Bgm> _backgroudMusics;
    
    // 모든 리소스가 로드되었는 지 확인하는 로드 플래그 (비동기)        
    private int _loadFlags;
    // 리소스 로딩 중 문제가 생긴 부분을 찾기 위한 에러 플래그
    private int _errorFlags;
}