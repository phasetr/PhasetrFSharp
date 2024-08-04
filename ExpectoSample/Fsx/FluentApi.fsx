#r "nuget: Expecto"
#r "nuget: Expecto.Flip"

open Expecto
open Expecto.Flip

let compute (multiplier: int) = 42 * multiplier

test "yup yup" {
  compute 1 |> Expect.equal "x1 = 42" 42
  compute 2 |> Expect.equal "x2 = 82" 84
}
|> runTestsWithCLIArgs [] [||]
