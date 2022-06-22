namespace Sample.DigitalNotice.Common.Entities;

/// <summary>
/// 
/// </summary>
public class Diary : Notice
{
    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<Note> Notes { get; set; }
}
