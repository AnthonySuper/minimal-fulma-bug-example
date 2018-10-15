module Client

open Summit
open Elmish
open Elmish.React


open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch
open Route
open Shared

open Fulma

type PageModel =
    | HomeModel of Routes.Home.Model
    | ServicesModel of Routes.Services.Model
    | AboutModel of Routes.About.Model
    | ContactModel of Routes.Contact.Model 

// The model holds data that you want to keep track of while the application is running
// in this case, we are keeping track of a counter
// we mark it as optional, because initially it will not be available from the client
// the initial value will be requested from server
type Model 
    = { Route: Route
        PageModel: PageModel } 

// The Msg type defines what events/actions can occur while the application is running
// the state of the application changes *only* in reaction to these events
type Msg =
| ChangeRoute of Route
| HomeMsg of Routes.Home.Msg
| ServicesMsg of Routes.Services.Msg
| AboutMsg of Routes.About.Msg
| ContactMsg of Routes.Contact.Msg 

// defines the initial state and initial command (= side-effect) of the application
let init () : Model * Cmd<Msg> =
    let home, homeCmd = Routes.Home.init ()
    let initialModel = { Route = Home; PageModel = HomeModel home }
    initialModel, Cmd.none

let private routeData r =
    match r with
    | Services ->
        let (m, c) = Routes.Services.init ()
        ServicesModel m, Cmd.none
    | About -> 
        let (m, c) = Routes.About.init ()
        AboutModel m, Cmd.none
    | Contact ->
        let (m, c) = Routes.Contact.init ()
        ContactModel m, Cmd.none
    | _ ->
        // By default, go to the home page 
        let (m, c) = Routes.Home.init ()
        HomeModel m, Cmd.none

    
let private changeRoute r model =
    let (d, cmd) = routeData r 
    {model with Route = r; PageModel = d}, cmd 

// The update function computes the next state of the application based on the current state and the incoming events/messages
// It can also run side-effects (encoded as commands) like calling the server via Http.
// these commands in turn, can dispatch messages to which the update function will react.
let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match (currentModel.PageModel, msg) with
    | (_, ChangeRoute r) -> changeRoute r currentModel 
    | (HomeModel hm, HomeMsg m) ->
        let (d, c) = Routes.Home.update m hm
        {currentModel with PageModel = (HomeModel d)}, Cmd.none
    | _ -> currentModel, Cmd.none

let viewRoute model dispatch = 
    match model.PageModel with
    | HomeModel m -> Routes.Home.view m (dispatch << HomeMsg)
    | ServicesModel m -> Routes.Services.view m (dispatch << ServicesMsg)
    | AboutModel m -> Routes.About.view m (dispatch << AboutMsg)
    | ContactModel m -> Routes.Contact.view m (dispatch << ContactMsg)
    | _ -> (fun _ -> div [] [])

let safeComponents =
    let components =
        span [ ]
           [
             a [ Href "https://saturnframework.github.io" ] [ str "Saturn" ]
             str ", "
             a [ Href "http://fable.io" ] [ str "Fable" ]
             str ", "
             a [ Href "https://elmish.github.io/elmish/" ] [ str "Elmish" ]
             str ", "
             a [ Href "https://mangelmaxime.github.io/Fulma" ] [ str "Fulma" ]
           ]

    p [ ]
        [ strong [] [ str "Built using SAFE Stack" ]
          str " powered by: "
          components ]
let view (model : Model) (dispatch : Msg -> unit) =
    div [ ClassName "overall-container" ]
        [ navDisp (dispatch << ChangeRoute) model.Route;
          div [ ClassName "main-content"]
             [ viewRoute model dispatch (dispatch << ChangeRoute) ]
          div [] [ showRoute model.Route |> str ]
        //   Container.container []
        //       [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
        //             [ Heading.h3 [] [ str ("Press buttons to manipulate counter: " + show model) ] ]
        //         Columns.columns []
        //             [ Column.column [] [ button "-" (fun _ -> dispatch Decrement) ]
        //               Column.column [] [ button "+" (fun _ -> dispatch Increment) ] ] ]

          Footer.footer [ ]
                [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                    [ safeComponents ] ] ]


#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
|> Program.run
