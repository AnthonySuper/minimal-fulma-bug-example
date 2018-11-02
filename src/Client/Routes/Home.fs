module Routes.Home

open Route
open Elmish
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma
open Helpers.Basic


// A less-than-pretty basic counter serves as the gadget on the home page.
// So, our model consists of one Int
type Model
    = unit 

// We can increment or decrement our int
type Msg = unit

// To start off, our counter is at zero, and we do not do any side-effects
let init (a:unit) =
    (), Cmd.none

// We update by either incrementing or decrementing our Int
let update msg model = (), Cmd.none

// Separate out the counter buttons and display from the header for readability 

let descStr =
    """
    We are dedicated to helping investment firms leverage technology to solve
    difficult problems. The team at Summit Investment Technologies brings decades
    of software and quantitative investing experience, providing us with unique
    insights that we put to work for your business.
    """


let titleTile title body =
    let child = 
        Tile.child
            [Tile.Option.CustomClass "box"]
            [
                  Heading.h3 [Heading.IsSubtitle] [ str title ]
                  p [] [ str body ]
            ]
    Tile.parent [] [child]


let devTile =
    let body =
        """
        Your firm needs the best tools possible.
        We build software to integrate your business, automate your tasks,
        and analyze your data, so you can spend more time on the important things.
        From narrow process automation to full-stack application development,
        our software engineers deliver world-class solutions to your problems.
        """
    titleTile "Software Development" body

let dataTile =
    let body =
        """
        Data is one of your firm's most valuable assets, so
        it's imperative that you make the right choices for how to handle it.
        Our exprts will help you chose the right data system for your needs, structure your
        data to maximize its potential, and optimize it so you can use it efficiently.
        """
    titleTile "Data Architecture" body

let anTile =
    let body =
        """
        We're experts in developing quantitative analytics, but we don't stop there.
        We can help you monitor your analytics so you can react to changing conditions,
        and automate the entire process so you can get right to the insights.
        """
    titleTile "Quantitative Analysis" body

let servicesSection =
    Section.section
        []
        [
            Heading.h2 [Heading.IsSpaced] [ str "Services" ]
            Tile.ancestor 
                []
                [
                    devTile
                    dataTile
                    anTile
                ]
        ]
// Try to align the buttons and the counter so they look nice,
// with absolutely no success 
let aboutSection =
    Section.section
        []
        [ 
            Heading.h2 [Heading.IsSpaced] [ str "Summit Investment Technologies, LLC" ]
            p [] [ str descStr ]
        ]

// A nice hero title kinda thing
let hero = bigHeroS "Summit Investment Technologies" "Leveraging Technology to Reach Your Firm's Goals"

let view (model : Model) (dispatch : Msg -> Unit) =
    article []
        [ 
            hero;
            aboutSection;
            servicesSection;
        ]