module Demian.Tests.FSharp.Text.InMemoryTextTest

open System.Linq
open Demian
open FsUnit
open Pocket.Common
open System;
open Xunit;

let at x = x
let in' x = x

let asText value = value |> InMemoryText
let characters (text:InMemoryText) = text.Characters.ToArray()
let asCharacters (value:string) = value.Select(fun x -> new Character(x)).ToArray()
let throwIfFail (result:Result) = if (result.Fail) then raise (() |> Exception)

let write part at value =
    let text = value |> asText
    text.Write(part, at) |> ignore
    text.AsString()
    
let writing part at value =
    (fun () -> (asText value).Write(part, at) |> throwIfFail |> ignore)
    
let remove length at value =
    let text = value |> asText
    text.Remove(length, at) |> ignore
    text.AsString()
    
let removing length at value =
    (fun () -> (asText value).Remove(length, at) |> throwIfFail |> ignore)

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
    writing null <| at 0 <| in' "Hello" |> shouldFail
    
[<Fact>]
let ``Writing empty string at -1 should fail``() =
    writing "" <| at -1 <| in' "Hello" |> shouldFail
    
[<Fact>]
let ``Writing empty string at 5 in "1234" should fail``() =
    writing "" <| at 5 <| in' "1234" |> shouldFail
    
[<Fact>]
let ``Writing empty string at 5 in "1234" should fail2``() =
    writing "" <| at 5 <| in' "1234" |> shouldFail
    
[<Fact>]
let ``When remove 1 character at 0 in "Hello" it should equal "ello"``() =
    remove 1 <| at 0 <| in' "Hello" |> should equal "ello"
    
[<Fact>]
let ``When remove 1 character at 1 in "Hello" it should equal "Hllo"``() =
    remove 1 <| at 1 <| in' "Hello" |> should equal "Hllo"
    
[<Fact>]
let ``When remove 1 character at 4 in "Hello" it should equal "Hell"``() =
    remove 1 <| at 4 <| in' "Hello" |> should equal "Hell"
    
[<Fact>]
let ``When remove 3 characters at 0 in "Hello" it should equal "lo"``() =
    remove 3 <| at 0 <| in' "Hello" |> should equal "lo"
    
[<Fact>]
let ``When remove 3 characters at 1 in "Hello" it should equal "Ho"``() =
    remove 3 <| at 1 <| in' "Hello" |> should equal "Ho"

[<Fact>]
let ``When remove 3 characters at 2 in "Hello" it should equal "He"``() =
    remove 3 <| at 2 <| in' "Hello" |> should equal "He"
    
[<Fact>]
let ``When remove 5 characters at 0 in "Hello" it should equal ""``() =
    remove 5 <| at 0 <| in' "Hello" |> should equal ""
    
[<Fact>]
let ``Removing -1 at 0 in "Hello" should fail``() =
    removing -1 <| at 0 <| in' "Hello" |> shouldFail
    
[<Fact>]
let ``Removing 0 at 0 in "Hello" should fail``() =
    removing 0 <| at 0 <| in' "Hello" |> shouldFail
    
[<Fact>]
let ``Removing 6 at 0 in "1" should fail``() =
    removing 6 <| at 0 <| in' "Hello" |> shouldFail
    
[<Fact>]
let ``Removing 1 at -1 in "Hello" should fail``() =
    removing 1 <| at -1 <| in' "Hello" |> shouldFail
    
[<Fact>]
let ``Removing 1 at 5 in "Hello" should fail``() =
    removing 1 <| at 5 <| in' "Hello" |> shouldFail