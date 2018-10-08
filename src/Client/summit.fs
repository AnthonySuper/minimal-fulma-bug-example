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

let navItems links = 
    links
    |> List.map (fun l -> 
        Fulma.Navbar.Item.a [ Navbar.Item.Props [Href l.Link]] [str l.Text] )       
     
let nav =  
     Navbar.navbar [] (navItems (LinkText.GetLinks))     

let site = 
    nav