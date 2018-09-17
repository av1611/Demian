module Demian.Tests.FSharp.Text.InMemoryTextTest

open System.Linq
open Demian
open FsUnit
open System;
open Xunit;

let at x = x
let in' x = x

let asText value = value |> InMemoryText
let characters (text:InMemoryText) = text.Characters.ToArray()
let asCharacters (value:string) = value.Select(fun x -> new Character(x)).ToArray()

let write part at value =
    let text = value |> asText
    text.Write(part, at) |> ignore
    text.AsString()
    
let successOfWrite part at value =
    let text = value |> asText
    text.Write(part, at).Success

[<Fact>]
let ``Text should fail to construct from null``() =
    (fun () -> null |> asText |> ignore) |> shouldFail
    
[<Theory>]
[<InlineData("")>]
[<InlineData("Hello, world.")>]
[<InlineData("3.14")>]
[<InlineData("Ready to set the world on fire, hehehehehe.")>]
let ``Text constructed from string should contain same characters`` (value) =
    value |> asText |> characters |> should equal (value |> asCharacters)

[<Fact>]
let ``When write "-" at 0 in "Hello" it should equal "-Hello"``() =
    write "-" <| at 0 <| in' "Hello" |> should equal "-Hello"
        
[<Fact>]
let ``When write "-" at 1 in "Hello" it should equal "H-ello"``() =
    write "-" <| at 1 <| in' "Hello" |> should equal "H-ello"
    
[<Fact>]
let ``When write "-" at 5 in "Hello" it should equal "Hello-"``() =
    write "-" <| at 5 <| in' "Hello" |> should equal "Hello-"
    
[<Fact>]
let ``When write "--" at 0 in "Hello" it should equal "--Hello"``() =
    write "--" <| at 0 <| in' "Hello" |> should equal "--Hello"
    
[<Fact>]
let ``When write "--" at 1 in "Hello" it should equal "H--ello"``() =
    write "--" <| at 1 <| in' "Hello" |> should equal "H--ello"
    
[<Fact>]
let ``When write "--" at 5 in "Hello" it should equal "Hello--"``() =
    write "--" <| at 5 <| in' "Hello" |> should equal "Hello--"
    
[<Fact>]
let ``Writing null should fail``() =
    successOfWrite null <| at 0 <| in' "Hello" |> should equal false
    
[<Fact>]
let ``Writing any string at -1 should fail``() =
    successOfWrite "" <| at -1 <| in' "Hello" |> should equal false
    
[<Fact>]
let ``Writing any string at 5 in "1234" should fail``() =
    successOfWrite "" <| at 5 <| in' "1234" |> should equal false