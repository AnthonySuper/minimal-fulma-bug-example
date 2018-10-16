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

type LinkText = 
    { Text: string
      Link: string
      Route: Route
      Sublinks : LinkText list }



let routes =
    [
        { Text = "Home"; Link = "#"; Route = Home; Sublinks = [] };
        { Text = "Services"; Link = "#Services"; Route = Services; Sublinks = [
            { Text = "Integration"; Link = "#Services-Integration"; Route = Services; Sublinks = [] }
            { Text = "Automation"; Link = "#Services-Automation"; Route = Services; Sublinks = [] }
        ] };
        { Text = "About Us"; Link = "#About"; Route = About; Sublinks = [] };
        { Text = "Contact"; Link = "#Contact"; Route = Contact; Sublinks = [] };
        { Text = "Blog"; Link = "#Blog"; Route = Blog; Sublinks = [] };
    ]

let hashToLoc hash = 
    List.filter (fun i -> i.Link.StartsWith(hash)) routes
    |> List.map (fun i -> i.Route)
    |> List.tryHead
    |> Option.defaultValue Home

let private sameLink a b = 
    a = b

let navBrand imgSrc =
   Navbar.Brand.div [ ]
            [ Navbar.Link.a [ Navbar.Link.Props [ Href "#" ] ]
                [ img [ Style [ Width "15rem"]
                        Src imgSrc ] ] ] 

let rec navLink dispatch current link =
    let isActive = sameLink current link.Route
    let item = 
        Navbar.Item.a 
            [ Navbar.Item.Props [Href link.Link; OnClick (fun _ -> dispatch link.Route)];
              Navbar.Item.IsActive isActive;
              Navbar.Item.IsTab ]
            [ str link.Text ]
    let linkItem = 
        Navbar.Link.a
            [ Navbar.Link.Props [Href link.Link; OnClick (fun _ -> dispatch link.Route)]
              Navbar.Link.IsActive isActive; ]
            [ str link.Text ]
    match link.Sublinks with
    | [] -> item
    | x -> Navbar.Item.div
            [Navbar.Item.HasDropdown; Navbar.Item.IsHoverable]
            [ linkItem;
              Navbar.Dropdown.div [] (List.map (navLink dispatch current) x)
            ]


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

