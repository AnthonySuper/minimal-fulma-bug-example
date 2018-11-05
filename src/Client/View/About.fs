module View.About

open Elmish
open Fulma
open Helpers.Basic
open Fable.Helpers.React
open Fable.Helpers.React.Props

type Model = unit
type Msg = unit

let init() : Model * Cmd<unit> =
    (), Cmd.none

let update msg model = model, Cmd.none

let mediaSection' imsrc title body =
    Media.media []
        [
            Media.left []
                [
                    Image.image [Image.Option.Is128x128] [ img [Src imsrc ] ]
                ]
            Media.content []
                [
                    yield Heading.h5 [Heading.IsSpaced] [str title]
                    yield! body
                ]
        ]

let mediaSection imgsrc title body =
    Section.section []
        [mediaSection' imgsrc title body]

let private exImgUrl =
    "https://d2gg9evh47fn9z.cloudfront.net/800px_COLOURBOX4588709.jpg"

let private hero = bigHeroS "About" "Who we are"

let paragraph s = p [] [str s]
let frankBody =
    [
        paragraph
            """
            As Chief Technology Officer and co-founder of Summit Investment Technologies, LLC,
            Mr. Frank is focused on helping organizations leverage technology to solve difficult
            problems. He leads the Summit's overall technical direction, software/data consulting effort,
            client solution architecture and implementation.
            """
        paragraph
            """
            Prior to founding Summit Mr. Frank was DIrector of Quantitative Research at 361 Capital, LLC,
            where he had wide-ranging responsibilities that included all aspects of quantitative asset management:
            from strategy development to full stack software development, creating proprietary tools for
            analysis and trading. During his tenure at 361 Capital Mr. Frank was instrumental in
            growing the firm from < $100 million to nearl $1 billion in assets under management.
            """
        paragraph
            """
            Mr. Frank is a functional first programmer, with a strong preferenc for F# and similarly structured
            programming languages. He also has expertise in C#, R, SQL, MongoDB, as well as strong familiarity
            with various cloud technologies and many financial industry software programs such as Bloomberg
            and Clarifi.
            """

        paragraph
            """
            Mr. Frank holds a BA in Business Administration from Northwest University and a Master of
            Science in Finance from Boston College. In addition to a passion for investments and technology
            he is an avid trail runner, completing numerous ultra-marathons, including two finishes
            of the Leadville 100 Trail Run. However, his man enjoyment comes from spending time with his
            beautiful wife and five incredible children.
            """
    ]

let revyBody =
    [
        paragraph
            """
            As a partner at Sumit Investment Technologies,
            Mr. Revy is responsible for research and implementation of cutting
            edge technologies and is currently focused on solidity programming and
            developing blockchain solutions. He has a strong
            background in bond trading and analytics, particularly within the convertible
            bond scope.
            """
        paragraph
            """
            In addition to Mr. Revy's passion for FinTech, he manages a global convertible
            bond portfolio for Valex Capital AG and is Chairmen of Staub Holding AG, a firm focused
            in Switzerland real estate development. He has been active in the investment
            industry since 1002, where he started at Lehman as a software developer and moved into trading.
            He served as head of tech/research in convertable bonds during his tenture there.
            Mr. Revy holds a BA in Linquistics and Computer Science from UCLA and an MBA in
            International Business from Pepperdine University. In his space time he coaches for
            REAL Colorado soccer, a competitive youth soccer league.
            """
    ]

let warwickBody =
    [
        paragraph
            """
            Ben Warwick is an active board member and partner at Summit Investment Technologies,
            where he helps the firm's strategy and direction. Mr. Warwick began his investment
            career in 1990. He has previously the founer of Quantitative Equity Strategies (QES),
            a quantitative investment management firm that developed indices and strategies
            focused on serving the mutual fund and ETF industry.
            """
        paragraph
            """
            In addition to developing investment strategies, Mr. Warwick has edited and contributed
            to several books on the use of exchange traded futures, including the wisely used
            college text The Futures Game (McGraw-Hill, 1998) and The Handbook of Managed
            Futures (McGraw-Hill, 1996). Mr. Warwick is also the author of Searching for Alpha:
            The Quest for Exceptional Investment Performance (Wiley, 2000).
            """
        paragraph
            """
            Mr. Warwick earned an MBA from the University of North Carolina, a BS in Chemcial Engineering
            from the University of Florida, and additional undergraduate degrees in physics and chemistry.
            He enjoys playing tennis, scuba diving, and other activities with his wife of more than
            thirty years and their three children.
            """
    ]

let view (model : Model) (dispatch : Msg -> Unit) =
    div []
        [
            hero;
            mediaSection exImgUrl "Jeremy Frank, FRM - Chief Technology Officer" frankBody
            mediaSection exImgUrl "Michael Revy - Partner" revyBody
            mediaSection exImgUrl "Ben Warwick - Board Member/Partner" warwickBody
        ]

