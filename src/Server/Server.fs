open System.IO
open System.Threading.Tasks

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open FSharp.Control.Tasks.V2
open Giraffe
open Saturn
open Shared

open Giraffe.Serialization
open Giraffe.GiraffeViewEngine

let publicPath = Path.GetFullPath "./public"
let port = 8085us

let getInitCounter() : Task<Counter> = task { return 42 }

let renderPage model =
    let htmlStr = Fable.Helpers.ReactServer.renderToString(View.Main.View.view model ignore)

    html []
       [ head []
            [ title [] [str "Summit Investment Technologies LLC"]
              meta [_charset "utf-8" ]
              meta
                [ KeyValue ("name", "viewport");
                 _content "width=device-width, initial-scale=1"]
              link
                [ _rel "stylesheet"
                  _href "/js/main.css" ]
            ]
         body []
            [ div [_id "elmish-app"] [ rawText htmlStr ]
              script [ _src "/js/bundle.js" ] []
            ]
       ]
let defaultState : View.Main.Types.Model =
    { Route = Route.Home
      ContactModel = View.Contact.Types.modelDefault
      NavbarModel = { HamburgerOpen = false }
    };


let renderModel model =
    let rendered = renderPage model
    htmlView rendered
let homewView = renderModel
let webApp = router {
    get "/api/init" (fun next ctx ->
        task {
            let! counter = getInitCounter()
            return! Successful.OK counter next ctx
        })
    get "/" (fun next ctx ->
        task {
            let model = {defaultState with Route = Route.Home}
            let rendered = renderPage model
            return! (htmlView rendered) next ctx
        })
    get "/about" (fun next ctx ->
        task {
            let model = {defaultState with Route = Route.About}
            let rendered = renderPage model
            return! (htmlView rendered) next ctx
        })
    get "/blog" (fun next ctx ->
        task {
            let model = {defaultState with Route = Route.Blog}
            let rendered = renderPage model
            return! (htmlView rendered) next ctx
        })
    get "/contact" (fun next ctx ->
        task {
            let model = {defaultState with Route = Route.Contact}
            let rendered = renderPage model
            return! (htmlView rendered) next ctx
        })
}

let configureSerialization (services:IServiceCollection) =
    let fableJsonSettings = Newtonsoft.Json.JsonSerializerSettings()
    fableJsonSettings.Converters.Add(Fable.JsonConverter())
    services.AddSingleton<IJsonSerializer>(NewtonsoftJsonSerializer fableJsonSettings)

let app = application {
    url ("http://0.0.0.0:" + port.ToString() + "/")
    use_router webApp
    memory_cache
    use_static publicPath
    service_config configureSerialization
    use_gzip
}

run app
