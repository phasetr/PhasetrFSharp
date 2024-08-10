#r "nuget: Dapper.FSharp"
#r "nuget: Microsoft.Data.Sqlite"

open Microsoft.Data.Sqlite
open Dapper
open Dapper.FSharp.SQLite

// SQLite接続文字列
let connStr = "Data Source=dapper.db"

OptionTypes.register()

type User = {
  Id: int
  Name: string
}
type Course = {
  Id: int
  Title: string
}
type Chapter = {
  Id: int
  Title: string
}
type UserCourse = {
  UserId: int
  CourseId: int
}
type CourseChapter = {
  CourseId: int
  ChapterId: int
}

let userTable = table<User>
let courseTable = table<Course>
let chapterTable = table<Chapter>
let userCourseTable = table<UserCourse>
let courseChapterTable = table<CourseChapter>

// テーブル作成スクリプト
let initializeDatabase() =
    use connection = new SqliteConnection(connStr)
    connection.Open()
    let sql = """
CREATE TABLE IF NOT EXISTS User (
    Id INTEGER PRIMARY KEY,
    Name TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS Course (
    Id INTEGER PRIMARY KEY,
    Title TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS Chapter (
    Id INTEGER PRIMARY KEY,
    Title TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS UserCourse (
    UserId INTEGER NOT NULL,
    CourseId INTEGER NOT NULL,
    PRIMARY KEY (UserId, CourseId),
    FOREIGN KEY (UserId) REFERENCES User(Id),
    FOREIGN KEY (CourseId) REFERENCES Course(Id)
);
CREATE TABLE IF NOT EXISTS CourseChapter (
    CourseId INTEGER NOT NULL,
    ChapterId INTEGER NOT NULL,
    PRIMARY KEY (CourseId, ChapterId),
    FOREIGN KEY (CourseId) REFERENCES Course(Id),
    FOREIGN KEY (ChapterId) REFERENCES Chapter(Id)
);
    """
    connection.Execute(sql) |> ignore

let insertData() =
    use conn = new SqliteConnection(connStr)
    conn.Open()

    let users = [
        {Id = 1; Name = "John Doe"}
        {Id = 2; Name = "Jane Doe"}
    ]
    let userSql = "INSERT INTO User (Id, Name) VALUES (@Id, @Name)"
    users |> List.iter (fun user -> conn.Execute(userSql, user) |> ignore)

    let course1 = {Id = 1; Title = "EF Core Course"}
    let course2 = {Id = 2; Title = "F# Course"}
    let courses = [ course1; course2   ]
    let courseSql = "INSERT INTO Course (Id, Title) VALUES (@Id, @Title)"
    courses |> List.iter (fun course -> conn.Execute(courseSql, course) |> ignore)

    let chapter1 = {Id = 1; Title = "Introduction"}
    let chapter2 =    {Id = 2; Title = "Advanced Topics"}
    let chapters = [chapter1;chapter2    ]
    let chapterSql = "INSERT INTO Chapter (Id, Title) VALUES (@Id, @Title)"
    chapters |> List.iter (fun chapter -> conn.Execute(chapterSql, chapter) |> ignore)

    let userCourses = [
        {UserId = 1; CourseId = 1}
    ]
    let userCourseSql = "INSERT INTO UserCourse (UserId, CourseId) VALUES (@UserId, @CourseId)"
    userCourses |> List.iter (fun uc -> conn.Execute(userCourseSql, uc) |> ignore)

    let courseChapters = [
        {CourseId = 1; ChapterId = 1}
        {CourseId = 1; ChapterId = 2}
    ]
    let courseChapterSql = "INSERT INTO CourseChapter (CourseId, ChapterId) VALUES (@CourseId, @ChapterId)"
    courseChapters |> List.iter (fun cc -> conn.Execute(courseChapterSql, cc) |> ignore)

initializeDatabase()
insertData()

let conn = new SqliteConnection(connStr)
async {
    let! users =
        conn.QueryAsync<User>("SELECT * FROM User")
        |> Async.AwaitTask
    return users
}
|> Async.RunSynchronously
|> printfn "%A"

// 上記の処理をfor式で書き換える
async {
    return! select {
        for u in userTable do
        orderBy u.Id
    }
    |> conn.SelectAsync<User>
    |> Async.AwaitTask
}
|> Async.RunSynchronously
|> printfn "%A"

async {
    return! select {
        for u in userTable do
        innerJoin uc in userCourseTable on (u.Id = uc.UserId)
        orderBy u.Id
    }
    |> conn.SelectAsyncOption<User, UserCourse>
    |> Async.AwaitTask
}
|> Async.RunSynchronously
|> printfn "%A"

async {
    return! select {
        for u in userTable do
        where (u.Id = 100)
    }
    |> conn.SelectAsync<User>
    |> Async.AwaitTask
}
|> Async.RunSynchronously
|> printfn "%A"

// 発行されるSQLとパラメータを表示
let sql, values =
    insert {
        into userTable
        value {
            Id = 3
            Name = "Alice"
        }
    } |> Deconstructor.insert
printfn $"%s{sql}"
printfn $"%A{values}"
