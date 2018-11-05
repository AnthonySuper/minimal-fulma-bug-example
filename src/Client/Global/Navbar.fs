module Global.Navbar

open Elmish
open Fulma
open Fable.Helpers.React.Props
open Fable.Helpers.React
open Route
open Fulma
open Elmish.Browser.Navigation

type Model
    = { HamburgerOpen : bool }

type Msg =
    | OpenHamburger
    | CloseHamburger
    | ToggleHamburger
    | ChangeRoute of string


let init () = { HamburgerOpen = false }, Cmd.none

let update msg model =
    match msg with
    | OpenHamburger -> { model with HamburgerOpen = true }, Cmd.none
    | CloseHamburger -> { model with HamburgerOpen = false }, Cmd.none
    | ToggleHamburger ->
        { model with HamburgerOpen = not model.HamburgerOpen }, Cmd.none
    | ChangeRoute s ->
        model, Navigation.newUrl s

type LinkText =
    { Text: string
      Link: string
      Route: Route
      Sublinks : LinkText list }



let routes =
    [
        { Text = "Home"; Link = "/"; Route = Home; Sublinks = [] };
        { Text = "About Us"; Link = "/about"; Route = About; Sublinks = [] };
        { Text = "Contact"; Link = "/contact"; Route = Contact; Sublinks = [] };
        { Text = "Blog"; Link = "/blog"; Route = Blog; Sublinks = [] };
    ]

//why this function
// let private sameLink a b =
//     a = b

let replaceDefault (r : Lazy<unit>) (e : Fable.Import.React.SyntheticEvent) =
    e.preventDefault ()
    r.Force()

let defaultClick act = replaceDefault act |> OnClick

let navBrand imgSrc =
   Navbar.Brand.div [ ]
            [ Navbar.Link.a [ Navbar.Link.Props [ Href "#" ] ]
                [ img [ Src imgSrc ] ] ]

let rec navLink changeRoute current showActive link =
    let isActive = showActive && (current = link.Route)
    let item =
        Navbar.Item.a
            [ Navbar.Item.Props [Href link.Link; lazy changeRoute link.Link |> defaultClick];
              Navbar.Item.IsActive isActive;
              Navbar.Item.IsTab ]
            [ str link.Text ]
    let linkItem =
        Navbar.Link.a
            [ Navbar.Link.Props [Href link.Link; lazy changeRoute link.Link |> defaultClick ]
              Navbar.Link.IsActive isActive; ]
            [ str link.Text ]
    match link.Sublinks with
    | [] -> item
    | x -> Navbar.Item.div
            [Navbar.Item.HasDropdown; Navbar.Item.IsHoverable]
            [ linkItem;
              Navbar.Dropdown.div [] (List.map (navLink changeRoute current false) x)
            ]


let hiddenLinks links =
    Navbar.Brand.div [ Modifiers [Modifier.IsHidden (Screen.Tablet, false)] ] links

let burgerOptions dispatch model = List.ofSeq (seq {
        if model.HamburgerOpen then
            yield! [CustomClass "is-active"]
        yield Fulma.Common.Props [ (OnClick (fun _ -> dispatch ToggleHamburger)) ]
})

let brandLogo = navBrand "images/summit_logo.png"
let view dispatch model currentRoute =
    let links = (List.map (navLink (dispatch << ChangeRoute) currentRoute true) routes)
    Navbar.navbar []
        [  Navbar.Brand.div
            //QUESTION from JF: What is force-flex?
            [CustomClass "force-flex"]
            [ Navbar.Item.a
                [ Navbar.Item.Props
                    [ Href "#"
                      lazy (dispatch (ChangeRoute "#")) |> defaultClick] ]
                [img [Src "Images/summit_logo.png"]] //QUESTION from JF: Why this again?
              Navbar.burger (burgerOptions dispatch model)
                [ span [] []
                  span [] []
                  span [] [] ]
            ]
           Navbar.menu [Navbar.Menu.IsActive model.HamburgerOpen]
            [Navbar.End.div []
                links ]
        ]