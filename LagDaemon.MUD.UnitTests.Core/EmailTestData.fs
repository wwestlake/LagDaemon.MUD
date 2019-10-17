module EmailTestData

open System
open FsCheck
open LagDaemon.MUD.Core

// TODO: Clean up test setup make nice module


let validEmailList = [

    ("email@example.com"                                                      , true)
    ("firstname.lastname@example.com"                                         , true)
    ("email@subdomain.example.com"                                            , true)
    ("firstname+lastname@example.com"                                         , true)
    ("email@123.123.123.123"                                                  , true)
    ("email@[123.123.123.123]"                                                , true)
    ("\"email\"@example.com"                                                  , true)
    ("1234567890@example.com"                                                 , true)
    ("email@example-one.com"                                                  , true)
    ("_______@example.com"                                                    , true)
    ("email@example.name"                                                     , true)
    ("email@example.museum"                                                   , true)
    ("email@example.co.jp"                                                    , true)
    ("firstname-lastname@example.com"                                         , true)
    ("much.\”more\ unusual\”@example.com"                                     , true)
    ("very.unusual.\”@\”.unusual.com@example.com"                             , true)
    ("very.\”(),:;<>[]\”.VERY.\”very@\\ \"very\”.unusual@strange.example.com" , true)
]

let invalidEmailList = [

    ("plainaddress"                               ,false)
    ("#@%^%#$@#$@#.com"                           ,false)
    ("@example.com"                               ,false)
    ("Joe Smith <email@example.com>"              ,false)
    ("email.example.com"                          ,false)
    ("email@example@example.com"                  ,false)
    (".email@example.com"                         ,false)
    ("email.@example.com"                         ,false)
    ("email..email@example.com"                   ,false)
    ("あいうえお@example.com"                       ,false)
    ("email@example.com (Joe Smith)"              ,false)
    ("email@example"                              ,false)
    ("email@-example.com"                         ,false)
    ("email@example.web"                          ,false)
    ("email@111.222.333.44444"                    ,false)
    ("email@example..com"                         ,false)
    ("Abc..123@example.com"                       ,false)
    ("\”(),:;<>[\]@example.com"                   ,false)
    ("just\”not\”right@example.com"               ,false)
    ("this\ is\"really\"not\allowed@example.com"  ,false)

]

let emailList1 = validEmailList @ invalidEmailList
let emailArray = List.toArray emailList1

let rand = new System.Random((int) System.DateTime.Now.Ticks)

let swap (a: _[]) x y =
    let tmp = a.[x]
    a.[x] <- a.[y]
    a.[y] <- tmp

let shuffle a =
    Array.iteri (fun i _ -> swap a i (rand.Next(i, Array.length a))) a

shuffle emailArray 

let emailTestList = emailArray |> Array.toList 





let chooseFromList xs = 
  gen { let! i = Gen.choose (0, List.length xs-1) 
        return List.item i xs }


let random = Random(int DateTime.Now.Ticks)


let firstPart = 
        [
        'a'..'z'] 
        @ ['A'..'A'] 
        @ ['0'..'9'] 
        @ ['_';'%';'+';'-']
        |> List.toArray

let secondPart =
        [
        'a'..'z'] 
        @ ['A'..'A'] 
        @ ['0'..'9'] 
        @ ['-']
        |> List.toArray

let thirdPart =
        [
        'a'..'z'] 
        @ ['A'..'A'] 
        |> List.toArray



let randomEmailAddress min max (validChars: char array) =


    let pick () = validChars.[ random.Next(Array.length validChars - 1) ]
    let stringFromCharList (cl : char list) =
        String.concat "" <| List.map string cl
    
    let rec inner len  list =
        if len = 0  then list
        else inner (len - 1) ( pick() :: list)
    
    inner (random.Next(min, max)) [] |> stringFromCharList

let emailList () = Seq.initInfinite (fun i ->  
                        RawEmailAddress <|
                            (randomEmailAddress 5 10 firstPart ) + "." 
                            + (randomEmailAddress 5 10 firstPart ) + "@" 
                            + (randomEmailAddress 5 10 secondPart ) + "." 
                            + (randomEmailAddress 2 4 thirdPart)
                    )
    
let strEmailList () = Seq.initInfinite (fun i ->  
        (randomEmailAddress 5 10 firstPart ) + "." 
        + (randomEmailAddress 5 10 firstPart ) + "@" 
        + (randomEmailAddress 5 10 secondPart ) + "." 
        + (randomEmailAddress 2 4 thirdPart)
)

    
type Generators =
    static member EmailAddress() =
      { new Arbitrary<EmailAddress>() with
          override x.Generator = emailList() |> Seq.take 1000 |> Seq.toList |>  Gen.elements }
    static member Email() =
      { new Arbitrary<string>() with
          override x.Generator = strEmailList() |> Seq.take 1000 |> Seq.toList |>  Gen.elements }

