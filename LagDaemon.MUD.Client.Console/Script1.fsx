

type Process<'T> = 
    | Action of ('T -> unit)
    | Function of ('T -> 'T)

let action f = Action f
let func f = Function f

let bind x f =
    match f with
    | Action f' -> f' x; x
    | Function f' -> f' x

