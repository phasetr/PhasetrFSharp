namespace EFCoreCSharp.Entities;

public class Chapter
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public ICollection<CourseChapter> CourseChapters { get; set; } = default!;
}
