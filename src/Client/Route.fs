module Route

open Elmish
open Fulma
open Fable.Helpers.React.Props
open Fable.Helpers.React

type BlogId = string

type Route =
    | Home
    | About
    | Contact
    | Blog 

let showRoute route = 
    match route with
    | Home -> "Home"
    | About -> "About"
    | Contact -> "Contact"
    | Blog -> "Blog"

