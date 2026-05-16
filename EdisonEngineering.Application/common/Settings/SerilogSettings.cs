namespace EdisonEngineering.Application
    .Common.Settings;

public class SerilogSettings
{
    public required string Path { get; set; }

    public int RetainedFileCountLimit
    {
        get;
        set;
    }
}

