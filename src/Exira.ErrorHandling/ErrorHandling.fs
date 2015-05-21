namespace Exira

module ErrorHandling =
    type Result<'TSuccess, 'TFailure> =
        | Success of 'TSuccess
        | Failure of 'TFailure

    /// Convert a single value into a two-track result.
    let succeed x =
        Success x

    /// Convert a single value into a two-track result.
    let fail x =
        Failure x

    /// Convert a single value into a two-track async result.
    let failAsync x =
        async { return Failure x }

    /// Apply either a success function or failure function.
    let either successFunc failureFunc twoTrackInput =
        match twoTrackInput with
        | Success s -> successFunc s
        | Failure f -> failureFunc f

    /// Convert a switch function into a two-track function.
    let bind f = either f fail

    /// Convert a one-track function into a switch.
    let switch f = f >> succeed

    /// Convert a one-track function into a two-track function.
    let map f = either (f >> succeed) fail

    /// Convert a dead-end function into a one-track function.
    let tee f x = f x; x

    /// Convert a switch function into a two-track function.
    let bindAsync f = either f failAsync

    /// Shorthand function to determine if a result is a Success, can be used in e.g. List.exists isSuccess.
    let isSuccess x = either (fun _ -> true) (fun _ -> false) x

    /// Shorthand function to determine if a result is a Failure, can be used in e.g. List.exists isFailure.
    let isFailure x = either (fun _ -> false) (fun _ -> true) x

    /// Shorthand function to select Success values, can be used in e.g. List.choose successOnly.
    let successOnly x = either (fun x -> Some x) (fun _ -> None) x

    /// Shorthand function to select Failure values, can be used in e.g. List.choose failureOnly.
    let failureOnly x = either (fun _ -> None) (fun x -> Some x) x

    /// Map the messages to a different error type.
    let mapMessages f result =
        match result with
        | Success x -> succeed x
        | Failure errors ->
            let errors' = List.map f errors
            fail errors'

    /// given a function wrapped in a result
    /// and a value wrapped in a result
    /// apply the function to the value only if both are Success
    let apply f result =
        match f, result with
        | Failure a', Success b' -> fail a'
        | Success a', Failure b' -> fail b'
        | Failure a', Failure b' -> fail (a' @ b')
        | Success f, Success b' -> succeed (f b')

    /// Given a function that transforms a value,
    /// apply it only if the result is on the Success branch.
    let lift f result =
        let f' =  succeed f
        apply f' result

    /// Infix version of apply.
    let (<*>) = apply

    /// iInfix version of lift.
    let (<!>) = lift

    let private constructionSuccess value =
        succeed value

    let private constructionFailure value =
        fail [value]

    /// Create with continuation.
    /// Assumes ctor looks like: let createWithCont success failure value =
    let construct ctor value =
        value
        |> ctor constructionSuccess constructionFailure