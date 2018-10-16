module Routes.Contact
open Elmish
open Fulma
open Helpers.Basic
open Fable.Helpers.React
open Route

type Model
    = { Name : string
        Email : string
        Comment : string
        Telephone : string
        Submitted : bool
        Submitting : bool }

type Msg =
| ChangeName of string
| ChangeEmail of string
| ChangeComment of string
| ChangeTelephone of string
| TrySubmit 
| SubmitSuccess
let init() : Model * Cmd<Msg> =
    { Name = ""; 
      Email = ""; 
      Comment = "";
      Telephone = "";
      Submitted = false; 
      Submitting = false }, Cmd.none

// Say that we don't actually have a command to run
let noCommand m = (m, Cmd.none)

// If the form is modified, stop displaying the "submitted" message,
// since that particular instance of the form isn't actually submitted yet
let noSubmit m = { m with Submitted = false } |> noCommand

// Readability alias
let yesSubmit = noCommand

// Used as a dummy value for the promise
let submitSucess _ = SubmitSuccess

// Failure can't happen so we just have this to make the compiler happy
let submitFailure _ = SubmitSuccess

// Simulate talking to the server by waiting for half a second.
// We do this by making a `Promise` that resolves after the given amount of time.
let promise _ = Fable.PowerPack.Promise.create(fun resolve reject ->
    Fable.Import.Browser.window.setTimeout(resolve, 500, []) |> ignore
)

// Try to "submit" the form, which actually just waits before informing us of
// our "successful submission."
let trySubmit m =
    (m, Cmd.ofPromise promise m submitSucess submitFailure)

// Somewhat interestingly, Elm handles side-effects in the "update" functions.
// More specifically, each update can dispatch a "command," which touches the real
// world in some way and then returns another message. In our case, we only ever
// care about dispatching a command in response to a `TrySubmit` message, which
// will dispatch a command that "submits" the form (really just waits a bit
// before claiming success, as a test simulation sort of thing).
let update msg model =
    match msg with
    | ChangeEmail e -> {model with Email = e} |> noSubmit
    | ChangeName n -> {model with Name = n} |> noSubmit
    | ChangeComment c -> {model with Comment = c} |> noSubmit
    | ChangeTelephone c -> {model with Telephone = c} |> noSubmit
    | TrySubmit -> { model with Submitting = true } |> trySubmit
    | SubmitSuccess -> 
        { model with Submitted = true; 
                     Submitting = false;
                     Comment = "" } |> yesSubmit

// First, the big hero introduction to the page
let hero = 
    bigHeroS "Contact" "Let's get in touch"

// A very simple function converting a `FormEvent` to the value of 
// that field 
let toValue (c: Fable.Import.React.FormEvent) = c.Value

// Wrap an element in a container div with a label 
let withLabel label body = 
    Field.div []
        [ Label.label [] [str label]
          body ] 

// A labeled text input.
// We pass the `value` of the input because we want this value to be
// controlled entirely from our client code.
// That is, we want the field in our Model to always corospond with this input.
// Thus, we explicitly set the value on each render.
// When the input changes, then the `change` event is fired *with the new value of the field*,
// which lets us update our field in the model. On the next rendering, we ensure that
// no weirdness happened by re-setting the value again.
//
// This probably seems strange, but Elmish is React-based,
// and that's how you make rich forms with React.
// See: https://reactjs.org/docs/forms.html
let labledText label value change =
    withLabel label 
        (Input.text 
            [ Input.OnChange (change << toValue) 
              Input.Value value
              Input.Props
                [Props.Required true]])

// See above, but for email
let labeledEmail label value change =
    withLabel label 
        (Input.email 
            [ Input.OnChange (change << toValue)
              Input.Value value
              Input.Props 
                [ Props.Pattern ".+@.+\\..+" 
                  Props.Required true]])


let labeledTel label value change =
    Input.tel
        [ Input.OnChange (change << toValue) 
          Input.Value value
          Input.Props 
            [ Props.Required false ]]
        |> withLabel label 

// See above, but for a text area
let labeledArea label value change =
    withLabel label 
        (Textarea.textarea 
              [ Textarea.OnChange (change << toValue) 
                Textarea.Value value
                Textarea.Props [ Props.Required true ] ] [] )

// Display a submission notification.
// That is, if we've just submitted, tell the user.
// If we're trying to submit, tell the user.
// Otherwise, render "nothing" (technically a blank span,
// which is another react-ism. Technically more recent versions
// of react do allow a "blank" node to be rendered, but as far 
// as I can tell, Elmish's version is older than that.
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

// Abstract out the form into its own function for readability
let contactForm model dispatch =
    form [Props.OnSubmit (fun c -> c.preventDefault (); dispatch TrySubmit ) ] 
        [ labledText "Name" model.Name (dispatch << ChangeName)
          labeledEmail "Email" model.Email (dispatch << ChangeEmail) 
          labeledTel "Telephone (Optional)" model.Telephone (dispatch << ChangeTelephone)
          labeledArea "Inquiry" model.Comment (dispatch << ChangeComment)
          Field.div []
            [ Control.div [] 
                [Button.button 
                    [ Button.Color IsPrimary
                      Button.Props [ Props.Type "submit" ] ]
                    [ str "Submit"] ] ]
          submitNotification model ]

// Nice, composite view
let view (model : Model) (dispatch : Msg -> Unit) (routeDispatch : Route -> Unit) =
    div []
        [ hero
          Section.section [] 
            [Container.container [Container.IsFluid] [contactForm model dispatch]]
        ]