module View.Contact.Types


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

let modelDefault =
    { Name = ""
      Email = ""
      Comment = ""
      Telephone = ""
      Submitted = false
      Submitting = false }