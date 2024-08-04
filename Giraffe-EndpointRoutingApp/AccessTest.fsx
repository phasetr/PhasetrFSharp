#r "nuget: FsHttp"
open FsHttp
let root = "http://localhost:5000"
let accessUrl url =
  http { GET url }
  |> Request.send
  |> fun x -> x.content.ReadAsStringAsync().Result
  |> printfn "%A"

[
  $"{root}/"
  $"{root}/foo"
  $"{root}/foo/bar"
  $"{root}/foo/bar/1"
  $"{root}/foo/1"
  $"{root}/a/b/c/1"
  $"{root}/"
  $"{root}/x"
  $"{root}/abc"
  $"{root}/123"
]
|> List.map accessUrl
