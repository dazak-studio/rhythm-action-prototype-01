public partial class LoadManager
{
    private enum LoadFlag
    {
        Bgm,
       // Map,
        
        Count
    }

    private static int GetLoadFlagValue(LoadFlag loadFlag)
    {
        return 1 << (int)loadFlag;
    }
}