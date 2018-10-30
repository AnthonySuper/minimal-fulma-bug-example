module Route

open Elmish
open Fulma
open Fable.Helpers.React.Props
open Fable.Helpers.React

type BlogId = string

type Route
    = Home
    | Services 
    | About
    | Contact
    | Blog

let showRoute route = 
    match route with
    | Home -> "Home"
    | Services
    | About -> "About"
    | Contact -> "Contact"
    | _ -> "Blog"

