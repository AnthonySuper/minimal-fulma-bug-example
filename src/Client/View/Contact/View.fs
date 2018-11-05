module View.Contact.View

open View.Contact.Types
open Fulma
open Helpers.Basic
open Fable.Helpers.React



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
let view (model : Model) (dispatch : Msg -> Unit) =
    div []
        [ hero
          Section.section []
            [Container.container [Container.IsFluid] [contactForm model dispatch]]
        ]