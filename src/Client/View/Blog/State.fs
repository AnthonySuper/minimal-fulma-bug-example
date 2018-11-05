module View.Blog.State

open View.Blog.Types
open Elmish
let init() : Model * Cmd<unit> =
    (), Cmd.none

let update msg model = model, Cmd.none