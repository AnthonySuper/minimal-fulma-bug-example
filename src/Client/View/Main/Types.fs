module View.Main.Types

open Route

open Elmish.Browser


// Each page of this little app has its own data model.
// We define this so we can keep track of the data of the page we're currently on.
// Elmish requires us to maintain a single global state, so this has to be a definition
// at the top-level. However, it's defined in terms of types defined in the modules, so
// this isn't really that bad.
type PageModel =
    | HomeModel of View.Home.Types.Model
    | AboutModel of View.About.Types.Model
    | ContactModel of View.Contact.Types.Model
    | BlogModel of View.Blog.Types.Model

// Change from a PageModel over to a type that merely describes which route we're on
// This allows us to properly display the route tabs as "active" when need be

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

// Our model is very simple, and consists of the model of the Global.Navbar state, plus the page state
type Model
    = { Route: Route
        ContactModel: View.Contact.Types.Model
        NavbarModel: Global.Navbar.Model }

// Our messages are also very simple. We can either change the route, change the Global.Navbar state,
// or update the model of the current page. Thus:
type Msg =
| HomeMsg of View.Home.Types.Msg
| AboutMsg of View.About.Types.Msg
| ContactMsg of View.Contact.Types.Msg
| BlogMsg of View.Blog.Types.Msg
| NavbarMsg of Global.Navbar.Msg

