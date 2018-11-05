module Client

open Elmish
open Elmish.React
open Elmish.Browser
open Elmish.Browser.Navigation



open Fable.PowerPack.Fetch
open Route
open Shared
open Global
open Fulma



#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif



Program.mkProgram View.Main.State.init View.Main.State.update View.Main.View.view
|> Program.toNavigable (UrlParser.parsePath View.Main.Types.route) View.Main.State.urlUpdate
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReactHydrate "elmish-app"
|> Program.run