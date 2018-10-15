module Routes.About
    open Elmish
    open Fulma
    open Helpers.Basic

    type Model = unit 
    type Msg = unit 

    let init() : Model * Cmd<unit> =
        (), Cmd.none

    let update msg model = model

    let private title = bigTitleS "About" "Who we are"

    let view (model : Model) (dispatch : Msg -> Unit) (routeDispatch) =
        Hero.hero []
            [
                Hero.head [] []
                Hero.body [] [title]
                Hero.foot [] []
            ]
            