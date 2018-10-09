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
        Fulma.Navbar.Item.a [ Navbar.Item.Props [Href l.Link]] [str l.Text] )       
let navBrand imgSrc =
   Navbar.Brand.div [ ]
            [ Navbar.Item.a [ Navbar.Item.Props [ Href "#" ] ]
                [ img [ Style [ Width "4rem"]
                        Src imgSrc ] ] ]    
let nav =  
     Navbar.navbar [] ((navBrand "./images/summit_mtn.png")::(navItems LinkText.GetLinks))     

let tabs links = 
    let eachTab =
        links
        |> List.mapi (fun i l -> 
            Tabs.tab [] [ a [Href l.Link] [str l.Text]] )
    Tabs.tabs [Tabs.IsCentered] eachTab

let hero =
    Hero.hero[]
        [Hero.head []
            [tabs LinkText.GetLinks]]
let site = 
    (tabs LinkText.GetLinks)