module Routes.Home

open Route
open Elmish
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma
open Helpers.Basic

    type Model
        = { Count : int }

    let init() =
        { Count = 0 }, Cmd.none

    type Msg =
        | ChangeRoute of Route
        | Increment
        | Decrement

    let update msg model =
        match msg with
        | Increment -> { model with Count = model.Count + 1 }, Cmd.none
        | Decrement -> { model with Count = model.Count - 1 }, Cmd.none
        | _ -> model, Cmd.none
    let counterParts model dispatch =  
        [ button "+" (fun _ -> dispatch Increment) 
          Notification.notification [Notification.Color IsInfo] [str (string model.Count)]
          button "-" (fun _ -> dispatch Decrement)
        ]

    let counter model dispatch =
        let parts = counterParts model dispatch
        Level.level
            []
            (parts |> List.map (Level.item [] << fun x -> [x]))

    let title = bigTitleS "Summit Investment Technologies" "The Grease on the Cogs of Capitalism"

    let view (model : Model) (dispatch : Msg -> Unit) routeDispatch =
        Hero.hero []
            [
                Hero.head [] []
                Hero.body []
                    [ title; counter model dispatch ]
                Hero.foot [] [] 
            ]



       
