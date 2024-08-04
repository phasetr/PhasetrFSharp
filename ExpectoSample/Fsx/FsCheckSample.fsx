#r "nuget: FsCheck"
#r "nuget: Expecto"
#r "nuget: Expecto.FsCheck"

open Expecto
open Expecto.FsCheck

let config =
  { FsCheckConfig.defaultConfig with
      maxTest = 10000 }

let properties =
  testList
    "FsCheck samples"
    [ testProperty "Addition is commutative" <| fun a b -> a + b = b + a

      testProperty "Reverse of reverse of a list is the original list"
      <| fun (xs: list<int>) -> List.rev (List.rev xs) = xs

      // you can also override the FsCheck config
      testPropertyWithConfig config "Product is distributive over addition"
      <| fun a b c -> a * (b + c) = a * b + a * c ]

Tests.runTestsWithCLIArgs [] [||] properties
