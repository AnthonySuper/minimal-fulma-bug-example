module Routes.Contact
    open Elmish
    open Fulma
    open Helpers.Basic

    type Model = unit 
    type Msg = unit 

    let init() : Model * Cmd<unit> =
        (), Cmd.none

    let update msg model = model

    let private title = bigTitleS "Contact" "Let's get in touch"

    let view (model : Model) (dispatch : Msg -> Unit) (routeDispatch) =
        Hero.hero []
            [
                Hero.head [] []
                Hero.body [] [title]
                Hero.foot [] []
            ]
            