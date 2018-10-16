module Routes.Services
    open Elmish
    open Fulma
    open Fable.Helpers.React
    open Helpers.Basic

    type Model = unit 
    type Msg = unit 

    let init() : Model * Cmd<unit> =
        (), Cmd.none

    let update msg model = model, Cmd.none
    let hero =
        bigHeroS "Services" "How we can help you"

    let view (model : Model) (dispatch : Msg -> Unit) (routeDispatch) =
        div [] [hero]
            