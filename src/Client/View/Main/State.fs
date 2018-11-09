module View.Main.State

open View.Main.Types
open Route
open Elmish.Browser
open Elmish


let urlUpdate (result: Route option) model =
    match result with
    | None ->
        (model, Navigation.Navigation.modifyUrl "#")
    | Some route ->
        { model with Model.Route = route }, Cmd.none



// The initial state and the initial effectful action.
// In this case, we don't have any effectful action, and we start on the home page.
let init r : Model * Cmd<Msg> =
    let route = Option.defaultValue Contact r
    let cm, _ = View.Contact.State.init ()
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
        let d, c = View.Contact.State.update cm currentModel.ContactModel
        {currentModel with ContactModel = d}, Cmd.map ContactMsg c
    | (NavbarMsg m) ->
        let (d, c) = Global.Navbar.update m currentModel.NavbarModel
        {currentModel with NavbarModel = d}, Cmd.map NavbarMsg c
    | _ -> currentModel, Cmd.none