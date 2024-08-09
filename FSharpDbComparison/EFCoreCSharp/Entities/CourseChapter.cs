namespace EFCoreCSharp.Entities;

public class CourseChapter
{
    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;
    public int ChapterId { get; set; }
    public Chapter Chapter { get; set; } = default!;
}
