#r "nuget: Expecto"
open System.Threading.Tasks
open Expecto

let simpleTest =
  testCase "A simple test" <| fun () ->
    let expected = 4
    Expect.equal expected (2+2) "2+2 = 4"

runTestsWithCLIArgs [] [||] simpleTest

// テストのグループ化
let tests =
  testList "A test group" [
    test "one test" {
      Expect.equal (2+2) 4 "2+2"
    }

    test "another test that fails" {
      Expect.equal (3+3) 5 "3+3"
    }

    testAsync "this is an async test" {
      let! x = async { return 4 }
      Expect.equal x (2+2) "2+2"
    }

    testTask "this is a task test" {
      let! n = Task.FromResult 2
      Expect.equal n 2 "n=2"
    }
  ]
  |> testLabel "samples"
runTestsWithCLIArgs [] [||] tests
