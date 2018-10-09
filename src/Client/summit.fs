module Summit
open Fulma
open Fable.Helpers.React.Props
open Fable.Helpers.React

type LinkText = 
    {   Text: string
        Link: string}
    static member GetLinks =
        [   {Text = "Home"; Link = "#"}
            {Text = "About Us"; Link = "#AboutUs"}
            {Text = "Contact"; Link = "#Contact"}  ]

type Pages =
| Home
| About
| Contact
let navItems links = 
        links
        |> List.map (fun l -> 
                Navbar.Item.a [ Navbar.Item.Props [Href l.Link]] [str l.Text] )       
let navBrand imgSrc =
   Navbar.Brand.div [ ]
            [ Navbar.Item.a [ Navbar.Item.Props [ Href "#" ] ]
                [ img [ Style [ Width "10rem"]
                        Src imgSrc ] ] ]    
let nav =  
    let links = 
        [ Navbar.Item.div [ ]
            (navItems LinkText.GetLinks)]
    Navbar.navbar [] 
        ((navBrand "./images/summit_logo.png")::links)

let tabs links = 
    let eachTab =
        links
        |> List.mapi (fun i l -> 
            Tabs.tab [] [ a [Href l.Link] [str l.Text]] )
    Tabs.tabs [] eachTab

let hero =
    Hero.hero[]
        [   Hero.head [] [nav]
            Hero.body [] 
                [ Container.container [ Container.IsFluid
                                        Container.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                        [ Heading.h1 [ ]
                            [ str "Header" ]
                          Heading.h2 [ Heading.IsSubtitle ]
                            [ str "Subtitle" ] ] ] ] 
            
let site = 
    (hero)