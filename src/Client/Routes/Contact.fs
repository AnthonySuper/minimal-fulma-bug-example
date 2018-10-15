module Routes.Contact
    open Elmish
    open Fulma
    open Helpers.Basic
    open Fable.Helpers.React

    type Model
        = { Name : string
            Email : string
            Comment : string
            Submitted : bool
            Submitting : bool }

    type Msg =
    | ChangeName of string
    | ChangeEmail of string
    | ChangeComment of string
    | TrySubmit 
    | SubmitSuccess
    let init() : Model * Cmd<Msg> =
        { Name = ""; 
          Email = ""; 
          Comment = ""; 
          Submitted = false; 
          Submitting = false }, Cmd.none

    let noCommand m = (m, Cmd.none)

    let noSubmit m = { m with Submitted = false } |> noCommand

    let yesSubmit = noCommand

    let submitSucess _ = SubmitSuccess

    // Failure can't happen so we just have this to make the compiler happy
    let submitFailure _ = SubmitSuccess

    let promise _ = Fable.PowerPack.Promise.create(fun resolve reject ->
        Fable.Import.Browser.window.setTimeout(resolve, 500, []) |> ignore
    )

    let trySubmit m =
        (m, Cmd.ofPromise promise m submitSucess submitFailure)

    let update msg model =
        match msg with
        | ChangeEmail e -> {model with Email = e} |> noSubmit
        | ChangeName n -> {model with Name = n} |> noSubmit
        | ChangeComment c -> {model with Comment = c} |> noSubmit
        | TrySubmit -> { model with Submitting = true } |> trySubmit 
        | SubmitSuccess -> 
            { model with Submitted = true; 
                         Submitting = false;
                         Comment = "" } |> yesSubmit
   

    let private title = bigTitleS "Contact" "Let's get in touch"

    let hero = 
        Hero.hero []
            [
                Hero.head [] []
                Hero.body [] [title]
                Hero.foot [] []
            ]
    
    let toValue (c: Fable.Import.React.FormEvent) = c.Value

    let withLabel label body = 
        Field.div []
            [ Label.label [] [str label]
              body ] 

    let labledText label value change =
        withLabel label 
            (Input.text 
                [ Input.OnChange (change << toValue) 
                  Input.Value value])
       
    let labeledEmail label value change =
        withLabel label 
            (Input.email 
                [ Input.OnChange (change << toValue)
                  Input.Value value])

    let labeledArea label value change =
        withLabel label 
            (Textarea.textarea 
                  [ Textarea.OnChange (change << toValue) 
                    Textarea.Value value ] [] )

    let submitNotification model =
        if model.Submitted then
            Notification.notification 
                [Notification.Color IsSuccess]
                [str "Submitted"]
        elif model.Submitting then
            Notification.notification
                [] [str "Submitting..."]
        else
           span [] []

    let contactForm model dispatch =
        form [] 
            [ labledText "Name" model.Name (dispatch << ChangeName)
              labeledEmail "Email" model.Email (dispatch << ChangeEmail) 
              labeledArea "Inquiry" model.Comment (dispatch << ChangeComment)
              Field.div []
                [ Control.div [] 
                    [Button.button 
                        [ Button.Color IsPrimary
                          Button.OnClick (fun c -> c.preventDefault (); dispatch TrySubmit )] 
                        [ str "Submit"] ] ]
              submitNotification model ]

    let view (model : Model) (dispatch : Msg -> Unit) (routeDispatch) =
        div []
            [ hero
              Section.section [] 
                [Container.container [Container.IsFluid] [contactForm model dispatch]]
            ]
            