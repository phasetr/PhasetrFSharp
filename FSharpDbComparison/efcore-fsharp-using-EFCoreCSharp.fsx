#r "nuget: Microsoft.EntityFrameworkCore.Sqlite"
#r "nuget: Microsoft.EntityFrameworkCore.Design"
#r "nuget: System.Linq"
#r "nuget: EntityFrameworkCore.FSharp"
#r "nuget: FSharp.Control.AsyncSeq"
#r "EFCoreCSharp/bin/Debug/net8.0/EFCoreCSharp.dll"

open EntityFrameworkCore.FSharp.DbContextHelpers
open FSharp.Control
open EFCoreCSharp.Data
open EFCoreCSharp.Entities

let initializeDatabase () =
  use context = new AppDbContext()
  context.Database.EnsureDeleted() |> ignore
  context.Database.EnsureCreated() |> ignore

  let user = User(Name = "John Doe", UserCourses = ResizeArray())

  let course =
    Course(
      Title = "EF Core Course",
      UserCourses = ResizeArray(),
      CourseChapters = ResizeArray()
    )

  let chapter1 = Chapter(Title = "Introduction", CourseChapters = ResizeArray())
  let chapter2 = Chapter(Title = "Advanced Topics", CourseChapters = ResizeArray())

  addEntity context user
  addEntity context course
  addEntityRange context [ chapter1; chapter2 ]
  saveChanges context
  printfn "Main Data initialized"

  // relationship data
  let userCourse =
    UserCourse(
      UserId = user.Id,
      User = user,
      CourseId = course.Id,
      Course = course
    )

  let courseChapter1 =
    CourseChapter(
      CourseId = course.Id,
      Course = course,
      ChapterId = chapter1.Id,
      Chapter = chapter1
    )

  let courseChapter2 =
    CourseChapter(
      CourseId = course.Id,
      Course = course,
      ChapterId = chapter2.Id,
      Chapter = chapter2
    )

  addEntity context userCourse
  addEntityRange context [ courseChapter1; courseChapter2 ]
  saveChanges context

let displayData () =
  use context = new AppDbContext()

  query {
    for user in context.Users do
      select user
  }
  |> AsyncSeq.ofSeq
  |> AsyncSeq.toListAsync
  |> Async.RunSynchronously
  |> printfn "%A"

  query {
    for user in context.Users do
      join userCourse in context.UserCourses on (user.Id = userCourse.UserId)
      select (user, userCourse)
  }
  |> AsyncSeq.ofSeq
  |> AsyncSeq.toListAsync
  |> Async.RunSynchronously
  |> printfn "%A"

  query {
    for course in context.Courses do
      join courseChapter in context.CourseChapters
                              on
                              (course.Id = courseChapter.CourseId)

      select (course, courseChapter)
  }
  |> AsyncSeq.ofSeq
  |> AsyncSeq.toListAsync
  |> Async.RunSynchronously
  |> printfn "%A"

// データベースの初期化とデータの表示
initializeDatabase ()
displayData ()
