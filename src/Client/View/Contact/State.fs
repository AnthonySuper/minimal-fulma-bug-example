module View.Contact.State

open View.Contact.Types
open Elmish

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