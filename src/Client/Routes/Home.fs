module Routes.Home

open Route
open Elmish
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma
open Helpers.Basic

// A less-than-pretty basic counter serves as the gadget on the home page.
// So, our model consists of one Int
type Model
    = { Count : int }

// We can increment or decrement our int
type Msg =
    | Increment
    | Decrement

// To start off, our counter is at zero, and we do not do any side-effects
let init (a:unit) =
    { Count = 0 }, Cmd.none

// We update by either incrementing or decrementing our Int
let update msg model =
    match msg with
    | Increment -> { model with Count = model.Count + 1 }, Cmd.none
    | Decrement -> { model with Count = model.Count - 1 }, Cmd.none

// Separate out the counter buttons and display from the header for readability 
let counterParts model dispatch =  
    [ button "+" (fun _ -> dispatch Increment) 
      str (string model.Count)
      button "-" (fun _ -> dispatch Decrement)
    ]

// Try to align the buttons and the counter so they look nice,
// with absolutely no success 
let counter model dispatch =
    let parts = counterParts model dispatch
    Section.section []
        [ Level.level
            []
            [Level.left [] (parts |> List.map (Level.item [] << fun x -> [x]))] ]

// A nice hero title kinda thing
let hero = bigHeroS "Summit Investment Technologies" "The Grease on the Cogs of Capitalism"

let view (model : Model) (dispatch : Msg -> Unit) routeDispatch =
    div []
        [ hero; counter model dispatch ]