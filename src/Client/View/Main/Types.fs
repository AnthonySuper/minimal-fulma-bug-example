module View.Main.Types

open Route

open Elmish.Browser




// Change from a PageModel over to a type that merely describes which route we're on
// This allows us to properly display the route tabs as "active" when need be

let route (st: UrlParser.State<(Route -> Route)>)=
    let map = UrlParser.map
    let s = UrlParser.s
    let top = UrlParser.top
    UrlParser.oneOf
        [ map Contact top
          map Contact (s "about")
          map Contact (s "contact")
          map Contact (s "blog") ]
        st

// Our model is very simple, and consists of the model of the Global.Navbar state, plus the page state
type Model
    = { Route: Route
        ContactModel: View.Contact.Types.Model
        NavbarModel: Global.Navbar.Model }

// Our messages are also very simple. We can either change the route, change the Global.Navbar state,
// or update the model of the current page. Thus:
type Msg =
| ContactMsg of View.Contact.Types.Msg
| NavbarMsg of Global.Navbar.Msg

