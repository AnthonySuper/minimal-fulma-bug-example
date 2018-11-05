module View.Main.View

open View.Main.Types

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma
open Route






// Which page should we display?
// In this case, it's quite simple.
// If we have a HomeModel as our PageModel, display the home page.
// If we have a ServicesModel as our PageModel, display the Services page.
// You probably get the idea!
// This is also very boiler-plate-ish but it's not too bad.
let viewRoute model dispatch =
    match model.Route with
    | Home -> View.Home.View.view () (dispatch << HomeMsg)
    | About -> View.About.View.view () (dispatch << AboutMsg)
    | Contact -> View.Contact.View.view model.ContactModel (dispatch << ContactMsg)
    | Blog -> View.Blog.View.view () (dispatch << BlogMsg)

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