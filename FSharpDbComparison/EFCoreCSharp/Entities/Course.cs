namespace EFCoreCSharp.Entities;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public ICollection<UserCourse> UserCourses { get; set; } = default!;
    public ICollection<CourseChapter> CourseChapters { get; set; } = default!;
}
