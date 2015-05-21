namespace Exira

module ErrorHandling =
    type Result<'TSuccess, 'TFailure> =
        | Success of 'TSuccess
        | Failure of 'TFailure

    // convert a single value into a two-track result
    let succeed x =
        Success x

    // convert a single value into a two-track result
    let fail x =
        Failure x

    let failAsync x =
        async { return Failure x }

    // apply either a success function or failure function
    let either successFunc failureFunc twoTrackInput =
        match twoTrackInput with
        | Success s -> successFunc s
        | Failure f -> failureFunc f

    // convert a switch function into a two-track function
    let bind f = either f fail

    // convert a one-track function into a switch
    let switch f = f >> succeed

    // convert a one-track function into a two-track function
    let map f = either (f >> succeed) fail

    // convert a dead-end function into a one-track function
    let tee f x = f x; x

    // convert a switch function into a two-track function
    let bindAsync f = either f failAsync

    // map the messages to a different error type
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

    /// given a function that transforms a value
    /// apply it only if the result is on the Success branch
    let lift f result =
        let f' =  succeed f
        apply f' result

    /// infix version of apply
    let (<*>) = apply

    /// infix version of lift
    let (<!>) = lift