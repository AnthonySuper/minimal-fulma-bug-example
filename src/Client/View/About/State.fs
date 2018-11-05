module View.About.State
open View.About.Types

open Elmish


let init() : Model * Cmd<unit> =
    (), Cmd.none

let update msg model = model, Cmd.none
