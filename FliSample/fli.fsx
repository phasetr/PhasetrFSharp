#r "nuget: Fli"
open Fli

cli {
    Exec "git"
    Arguments ["log"; "-n"; "2"]
}
|> Command.execute
