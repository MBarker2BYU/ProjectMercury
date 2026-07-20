namespace Mercury.Demo.WinForms.Demo;

internal readonly struct DemoLogEntry(string level, string entry)
{
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.Now;

    public string Level { get; } = level;

    public string Entry { get; } = entry;
}