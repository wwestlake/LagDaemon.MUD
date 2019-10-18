namespace LagDaemon.MUD.UnitTests.Core

[<AutoOpen>]
module Generators =
    open FsCheck
    open System.Text.RegularExpressions
    open System
 
    type TestValidEmail = TestValidEmail of string                        
    type TestInvalidEmail = TestInvalidEmail of string
    type TestValidPassword = TestValidPassword of string
    
    let upperCase = ['A'..'Z']
    let lowerCase = ['a'..'z']
    let digits    = ['0'..'9']
    let special_1 = "._%+-" |> Seq.toList
    let special_2 = ".-" |> Seq.toList
    let special_3 = "~`!@#$%^&*()_-+={[}]|\\:;\"'<,>.?/" |> Seq.toList
    let firstPart = upperCase @ lowerCase @ digits @ special_1
    let secondPart = upperCase @ lowerCase @ digits @ special_2
    let thirdPart = upperCase @ lowerCase
        
    
    let random =
        let rand = Random(int DateTime.Now.Ticks)
        (fun min max -> rand.Next(min, max))
    
    let swap (a: _[]) x y =
        let tmp = a.[x]
        a.[x] <- a.[y]
        a.[y] <- tmp
    
    // shuffle an array (in-place)
    let shuffle a =
        Array.iteri (fun i _ -> swap a i (random i (Array.length a))) a
        a

    let randomFromList (xs: 'a list) =
        xs.[random 0 (List.length xs - 1)]
    
    let randomListFromList (xs: 'a list) count =
        [for i in 0 .. count do randomFromList xs]
    
    let listToString list = 
        list |> List.toArray |> System.String
    
    let generateEmail valid =
        let validate ea =
            let regex = @"^[a-zA-Z0-9][a-zA-Z0-9._%+-]{0,63}@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,62}[a-zA-Z0-9])?\.){1,8}[a-zA-Z]{2,63}$"
            Regex.IsMatch(ea, regex)
    
        let user () = randomListFromList firstPart (random 5 10)
        let domain () = randomListFromList secondPart (random 5 10)
        let topLevel () = randomListFromList thirdPart (random 2 10)
        let dot = ['.']
        let at = ['@']
    
        let rec create () count =
            let e = (user ()) @ dot @ (user ()) 
                    @ at @ (domain ()) @ dot @ (domain ()) @ dot @ (topLevel ())
                    |> listToString
            if (validate e) = valid then e 
            else if count <= 0 then e
            else create() (count - 1)
    
        create() 100

            
    
    let validEmailList =
        [ for i in 0..1000 do yield (generateEmail true |> TestValidEmail) ]
    
    let invalidEmailList =
        [ for i in 0..1000 do yield (generateEmail false |> TestInvalidEmail) ]
    
        
    let generatePassword minLengh maxLength minUpper minDigit minSpecial =
        let uppers = randomListFromList upperCase (random minUpper minUpper )
        let digits = randomListFromList digits (random minDigit minDigit )
        let special = randomListFromList special_3 (random minSpecial minSpecial )
        let lower = randomListFromList lowerCase (random minLengh (maxLength - minUpper - minDigit - minSpecial ))
        let result = uppers @ digits @ special @ lower |> List.toArray |> shuffle |>  System.String
        result

    let validPasswordList =
        [ for i in 0..1000 do yield (generatePassword 8 16 2 2 2) |> TestValidPassword]

    let chooseFromList xs = 
                      gen { let! i = Gen.choose (0, List.length xs-1) 
                            return List.item i xs }
    
    
    
    type Generators () =
        static member ValidEmailList() =
            { new Arbitrary<TestValidEmail>() with
                override x.Generator = (chooseFromList validEmailList) }
    
        static member InvalidEmailList() =
            { new Arbitrary<TestInvalidEmail>() with
                override x.Generator = (chooseFromList invalidEmailList) }
    
        static member ValidPasswordList() =
            { new Arbitrary<TestValidPassword>() with
                override x.Generator = (chooseFromList validPasswordList) }
            
