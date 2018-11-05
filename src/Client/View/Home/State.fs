module View.Home.State

open View.Home.Types
open Elmish

// To start off, our counter is at zero, and we do not do any side-effects
let init (a:unit) =
    (), Cmd.none

// We update by either incrementing or decrementing our Int
let update msg model = (), Cmd.none