module Route

open Elmish
open Fulma
open Fable.Helpers.React.Props
open Fable.Helpers.React

type BlogId = string

type Route =
    | Contact

let showRoute route =
    match route with
    | Contact -> "Contact"


