module Summit
open Fulma
open Fable.Helpers.React.Props
open Fable.Helpers.React


type Pages =
| Home
| About
| Contact

let hero =
    Hero.hero []
        [   Hero.head [] []
            Hero.body [] 
                [ Container.container [ Container.IsFluid
                                        Container.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                        [ Heading.h1 [ ]
                            [ str "Header" ]
                          Heading.h2 [ Heading.IsSubtitle ]
                            [ str "Subtitle" ] ] ] ] 
            
let site = 
    (hero)