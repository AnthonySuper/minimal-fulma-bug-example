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

// Each page of this little app has its own data model.
// We define this so we can keep track of the data of the page we're currently on.
// Elmish requires us to maintain a single global state, so this has to be a definition
// at the top-level. However, it's defined in terms of types defined in the modules, so
// this isn't really that bad.
type PageModel =
    | HomeModel of Routes.Home.Model
    | ServicesModel of Routes.Services.Model
    | AboutModel of Routes.About.Model
    | ContactModel of Routes.Contact.Model
    | BlogModel of Routes.Blog.Model

// Change from a PageModel over to a type that merely describes which route we're on
// This allows us to properly display the route tabs as "active" when need be
let getRoute r =
    match r with
    | HomeModel _ -> Home
    | ServicesModel _ -> Services
    | AboutModel _ -> About
    | ContactModel _ -> Contact
    | BlogModel _ -> Blog 


// Our model is very simple, and only consists of the model for each individual page
type Model 
    = { PageModel: PageModel } 

// Our messages are also very simple. We can either change the route, or update the model
// of the current page. Thus: 
type Msg =
| ChangeRoute of Route
| HomeMsg of Routes.Home.Msg
| ServicesMsg of Routes.Services.Msg
| AboutMsg of Routes.About.Msg
| ContactMsg of Routes.Contact.Msg 
| BlogMsg of Routes.Blog.Msg

// The initial state and the initial effectful action.
// In this case, we don't have any effectful action, and we start on the home page.
let init () : Model * Cmd<Msg> =
    let home, homeCmd = Routes.Home.init ()
    let initialModel = { PageModel = HomeModel home }
    initialModel, Cmd.none

// How should we respond to a page-change request?
// Quite easily: just re-load that page
let private routeData r =
    match r with
    | Services ->
        let (m, c) = Routes.Services.init ()
        ServicesModel m, Cmd.map ServicesMsg c 
    | About -> 
        let (m, c) = Routes.About.init ()
        AboutModel m, Cmd.map AboutMsg c
    | Contact ->
        let (m, c) = Routes.Contact.init ()
        ContactModel m, Cmd.map ContactMsg c
    | Blog ->
        let (m, c) = Routes.Blog.init () 
        BlogModel m, Cmd.map BlogMsg c 
    | _ ->
        // By default, go to the home page 
        let (m, c) = Routes.Home.init ()
        HomeModel m, Cmd.map HomeMsg c

    
let private changeRoute r model =
    let (d, cmd) = routeData r 
    {model with PageModel = d}, cmd 

// In this function we write a bunch of boilerplate!
// More specifically, we map each message to its data type.
// So we map home mesages to the HomeModel, ServicesMsgs to the ServicesModel, and so on.
// So far this has been what I like the least about Elmish: this is quite a bit of code that is
// essentially just copy/paste. Ah well, though!
// Note that our design ensures that we can only recieve the proper type of message for
// the current page, as other pages can't see or know about other messages. So,
// the catch-all at the bottom just makes the compiler happy.
let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match (currentModel.PageModel, msg) with
    | (_, ChangeRoute r) -> changeRoute r currentModel 
    | (HomeModel hm, HomeMsg m) ->
        let (d, c) = Routes.Home.update m hm
        {currentModel with PageModel = (HomeModel d)}, Cmd.none
    | (ServicesModel sm, ServicesMsg m) ->
        let (d, c) = Routes.Services.update m sm
        {currentModel with PageModel = (ServicesModel d)}, Cmd.map ServicesMsg c
    | (AboutModel am, AboutMsg m) ->
        let (d, c) = Routes.About.update m am
        {currentModel with PageModel = (AboutModel d)}, Cmd.map AboutMsg c 
    | (ContactModel cm, ContactMsg m) ->
        let (d, c) = Routes.Contact.update m cm
        {currentModel with PageModel = (ContactModel d)}, Cmd.map ContactMsg c
    | (BlogModel bm, BlogMsg m) ->
        let (d, c) = Routes.Blog.update m bm
        {currentModel with PageModel = (BlogModel d)}, Cmd.map BlogMsg c 
    | _ -> currentModel, Cmd.none

// Which page should we display?
// In this case, it's quite simple.
// If we have a HomeModel as our PageModel, display the home page.
// If we have a ServicesModel as our PageModel, display the Services page.
// You probably get the idea!
// This is also very boiler-plate-ish but it's not too bad.
let viewRoute model dispatch = 
    match model.PageModel with
    | HomeModel m -> Routes.Home.view m (dispatch << HomeMsg)
    | ServicesModel m -> Routes.Services.view m (dispatch << ServicesMsg)
    | AboutModel m -> Routes.About.view m (dispatch << AboutMsg)
    | ContactModel m -> Routes.Contact.view m (dispatch << ContactMsg)
    | BlogModel m -> Routes.Blog.view m (dispatch << BlogMsg)
    | _ -> (fun _ -> div [] [])

let view (model : Model) (dispatch : Msg -> unit) =
    div [ ClassName "overall-container" ]
        [ navDisp (dispatch << ChangeRoute) (getRoute model.PageModel);
          div [ ClassName "main-content"]
             [ viewRoute model dispatch (dispatch << ChangeRoute) ]
             // this dispatch << ChangeRoute bit here is a way to more easily
             // change the page from within the body of a page.
             // Essentially, it's a function that lets child pages dispatch
             // `ChangeRoute` messages more easily.
        ]

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
