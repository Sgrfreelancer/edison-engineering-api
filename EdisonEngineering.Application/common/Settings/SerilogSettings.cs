namespace EdisonEngineering.Application
    .Common.Settings;

public class SerilogSettings
{
    public string Path { get; set; }

    public int RetainedFileCountLimit
    {
        get;
        set;
    }
}
