module Routes.Services

open Elmish
open Fulma
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Helpers.Basic

type Model = unit 
type Msg = unit 

let init() : Model * Cmd<unit> =
    (), Cmd.none

let update msg model = model, Cmd.none
let hero =
    bigHeroS "Services" "How we can help you"

let titleSub title subtitle = 
    [ h1 [ ClassName "title"] [ str title ]
      h2 [ ClassName "subtitle" ] [ str subtitle ] ]; 


let view (model : Model) (dispatch : Msg -> Unit) =
    div [] 
        [ hero
          Section.section [Section.Props [ Props.Id "integration" ]]
            [ yield! titleSub "Integration" "Making Connections" ] 
          Section.section [Section.Props [ Props.Id "automation"]]
            [yield! titleSub "Automation" "Making Work Disappear"]
        ]