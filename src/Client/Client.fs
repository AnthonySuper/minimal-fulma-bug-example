module Client

open Summit
open Elmish
open Elmish.React
open Elmish.Browser
open Elmish.Browser.Navigation


open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch
open Route
open Shared
open Global.Navbar
open Fulma
open Routes

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



// Our model is very simple, and consists of the model of the Global.Navbar state, plus the page state
type Model 
    = { Route: Route
        ContactModel: Contact.Model
        NavbarModel: Global.Navbar.Model } 

// Our messages are also very simple. We can either change the route, change the Global.Navbar state,
// or update the model of the current page. Thus: 
type Msg =
| HomeMsg of Routes.Home.Msg
| ServicesMsg of Routes.Services.Msg
| AboutMsg of Routes.About.Msg
| ContactMsg of Routes.Contact.Msg 
| BlogMsg of Routes.Blog.Msg
| NavbarMsg of Global.Navbar.Msg


let urlUpdate (result: Route option) model =
    match result with
    | None ->
        (model, Navigation.Navigation.modifyUrl "#")
    | Some route ->
        { model with Model.Route = route }, Cmd.none
    
let route (st: UrlParser.State<(Route -> Route)>)=
    let map = UrlParser.map
    let s = UrlParser.s
    let top = UrlParser.top
    UrlParser.oneOf 
        [ map Home top 
          map About (s "about")
          map Contact (s "contact")
          map Blog (s "blog") ]
        st

// The initial state and the initial effectful action.
// In this case, we don't have any effectful action, and we start on the home page.
let init r : Model * Cmd<Msg> =
    let route = Option.defaultValue Home r 
    let cm, _ = Contact.init () 
    let navModel, navCmd = Global.Navbar.init ()
    let initialModel = { NavbarModel = navModel; Route = route; ContactModel = cm }
    initialModel, Cmd.none



// In this function we write a bunch of boilerplate!
// More specifically, we map each message to its data type.
// So we map home mesages to the HomeModel, ServicesMsgs to the ServicesModel, and so on.
// So far this has been what I like the least about Elmish: this is quite a bit of code that is
// essentially just copy/paste. Ah well, though!
// Note that our design ensures that we can only recieve the proper type of message for
// the current page, as other pages can't see or know about other messages. So,
// the catch-all at the bottom just makes the compiler happy.
let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | (ContactMsg cm) ->
        let d, c = Contact.update cm currentModel.ContactModel
        {currentModel with ContactModel = d}, Cmd.map ContactMsg c 
    | (NavbarMsg m) ->
        let (d, c) = Global.Navbar.update m currentModel.NavbarModel
        {currentModel with NavbarModel = d}, Cmd.map NavbarMsg c
    | _ -> currentModel, Cmd.none

// Which page should we display?
// In this case, it's quite simple.
// If we have a HomeModel as our PageModel, display the home page.
// If we have a ServicesModel as our PageModel, display the Services page.
// You probably get the idea!
// This is also very boiler-plate-ish but it's not too bad.
let viewRoute model dispatch = 
    match model.Route with
    | Home -> Routes.Home.view () (dispatch << HomeMsg)
    | About -> Routes.About.view () (dispatch << AboutMsg)
    | Contact -> Routes.Contact.view model.ContactModel (dispatch << ContactMsg)
    | Blog -> Routes.Blog.view () (dispatch << BlogMsg)

let view (model : Model) (dispatch : Msg -> unit) =
    div [ ClassName "overall-container" ]
        [ Global.Navbar.view (dispatch << NavbarMsg) model.NavbarModel model.Route
          div [ ClassName "main-content"]
             [ viewRoute model dispatch ]
             
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
|> Program.toNavigable (UrlParser.parseHash route) urlUpdate
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
|> Program.run