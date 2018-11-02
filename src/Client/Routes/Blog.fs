module Routes.Blog
open Elmish
open Fulma
open Helpers.Basic

type Model = unit 
type Msg = unit 

let init() : Model * Cmd<unit> =
    (), Cmd.none

let update msg model = model, Cmd.none

let private hero = bigHeroS "Blog" "What we've been doing"

let view (model : Model) (dispatch : Msg -> Unit) =
    hero
        