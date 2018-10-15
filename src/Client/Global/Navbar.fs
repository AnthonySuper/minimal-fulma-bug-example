module Global.Navbar

open Elmish
open Fulma
open Fable.Helpers.React.Props
open Fable.Helpers.React
open Route
open Fulma

type Model
    = { HamburgerOpen : bool }

type Msg =
    | OpenHamburger
    | CloseHamburger
    | ToggleHamburger


let init () = { HamburgerOpen = false }, Cmd.none

let update msg model =
    match msg with
    | OpenHamburger -> { model with HamburgerOpen = true }, Cmd.none
    | CloseHamburger -> { model with HamburgerOpen = false }, Cmd.none
    | ToggleHamburger -> 
        { model with HamburgerOpen = not model.HamburgerOpen }, Cmd.none

let routes =
    [
        { Text = "Home"; Link = "#"; Route = Home };
        { Text = "Services"; Link = "#Services"; Route = Services };
        { Text = "About Us"; Link = "#About"; Route = About };
        { Text = "Contact"; Link = "#Contact"; Route = Contact };
        { Text = "Blog"; Link = "#Blog"; Route = Blog };
    ]

let private sameLink a b = 
    a = b

let navBrand imgSrc =
   Navbar.Brand.div [ ]
            [ Navbar.Item.a [ Navbar.Item.Props [ Href "#" ] ]
                [ img [ Style [ Width "15rem"]
                        Src imgSrc ] ] ] 

let navLink dispatch current link =
    let isActive = sameLink current link.Route 
    Navbar.Item.a 
        [ Navbar.Item.Props [Href link.Link; OnClick (fun _ -> dispatch link.Route)];
          Navbar.Item.IsActive isActive;
          Navbar.Item.IsTab ]
        [ str link.Text ]


let hiddenLinks links = 
    Navbar.Brand.div [ Modifiers [Modifier.IsHidden (Screen.Tablet, false)] ] links

let burgerOptions dispatch model = List.ofSeq (seq {
        if model.HamburgerOpen then
            yield! [CustomClass "is-active"]
        yield Fulma.Common.Props [ (OnClick (fun _ -> dispatch ToggleHamburger)) ]
})
  

let brandLogo = navBrand "images/summit_logo.png"
let view dispatch model dispatchRoute currentRoute =
    let links = (List.map (navLink dispatchRoute currentRoute) routes)
    Navbar.navbar []
        [  Navbar.Brand.div
            [CustomClass "force-flex"]
            [ Navbar.Item.a [ Navbar.Item.Props [ Href "#" ] ]
                [img [Src "Images/summit_logo.png"]]
              Navbar.burger (burgerOptions dispatch model) 
                [ span [] []
                  span [] []
                  span [] [] ]
            ]
           Navbar.menu [Navbar.Menu.IsActive model.HamburgerOpen]
            links
        ]

