module View.Blog.View

open View.Blog.Types
open Fulma
open Helpers.Basic




let private hero = bigHeroS "Blog" "What we've been doing"

let view (model : Model) (dispatch : Msg -> Unit) =
    hero
