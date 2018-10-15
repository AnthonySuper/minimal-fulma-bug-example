module Route

open Elmish
open Fulma
open Fable.Helpers.React.Props
open Fable.Helpers.React

type BlogId = string

type Route
    = Home
    | Services
    | About
    | Contact
    | BlogIndex
    | BlogPost of BlogId


let showRoute route = 
    match route with
    | Home -> "Home"
    | Services -> "Services"
    | About -> "About"
    | Contact -> "Contact"
    | _ -> "Blog"



type LinkText = 
    { Text: string
      Link: string
      Route: Route }

let routes =
    [
        { Text = "Home"; Link = "#"; Route = Home };
        { Text = "Services"; Link = "#Services"; Route = Services };
        { Text = "About Us"; Link = "#About"; Route = About };
        { Text = "Contact"; Link = "#Contact"; Route = Contact };
        { Text = "Blog"; Link = "#Blog"; Route = BlogIndex };
    ]

let private sameLink a b = 
    match a, b with
    | (BlogIndex, BlogIndex) -> true 
    | (BlogPost _, BlogIndex) -> true
    | (BlogIndex, BlogPost _) -> true
    | (a, b) -> a = b

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

let brandLogo = navBrand "./images/summit_logo.png"
let navDisp dispatch current =
    let links = (List.map (navLink dispatch current) routes)
    Navbar.navbar []
        (brandLogo::links)

