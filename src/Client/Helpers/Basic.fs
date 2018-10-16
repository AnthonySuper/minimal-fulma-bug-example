module Helpers.Basic

open Fulma
open Fable.Helpers.React 


let bigTitleS title subtitle =
    Container.container 
        [ Container.IsFluid 
          Container.Modifiers
            [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ]
        ]
        [ Heading.h1 [] [ str title ]
          Heading.h2 [ Heading.IsSubtitle ]
            [ str subtitle ] ]

    
let bigHeroS title subtitle =
  Hero.hero [ Hero.Color Color.IsPrimary ]
    [ Hero.head [] []
      Hero.body [] [ bigTitleS title subtitle ]
      Hero.foot [] [] ]


let button txt onClick =
    Button.button
        [ Button.Color IsPrimary
          Button.OnClick onClick ]
        [ str txt ]