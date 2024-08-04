#r "nuget: Expecto"
#r "nuget: Expecto.FsCheck"
#r "nuget: FsCheck"
open Expecto
open FsCheck
open Expecto.FsCheck

type User = {
    Id : int
    FirstName : string
    LastName : string
}

type UserGen() =
   static member User() : Arbitrary<User> =
       let genFirsName = Gen.elements ["Don"; "Henrik"; null]
       let genLastName = Gen.elements ["Syme"; "Feldt"; null]
       let createUser id firstName lastName =
           {Id = id; FirstName = firstName ; LastName = lastName}
       let getId = Gen.choose(0,1000)
       let genUser =
           createUser <!> getId <*> genFirsName <*> genLastName
       genUser |> Arb.fromGen

let config = { FsCheckConfig.defaultConfig with arbitrary = [typeof<UserGen>] }

let properties =
  testList "FsCheck samples" [

    // you can also override the FsCheck config
    testPropertyWithConfig config "User with generated User data" <|
      fun x ->
        Expect.isNotNull x.FirstName "First Name should not be null"
  ]

Tests.runTestsWithCLIArgs [] [||] properties
