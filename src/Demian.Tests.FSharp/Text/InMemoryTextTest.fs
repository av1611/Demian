module Demian.Tests.FSharp.Text.InMemoryTextTest

open System.Linq
open Demian
open FsUnit
open System;
open Xunit;

let at x = x
let in' x = x

let asText value = (value) |> InMemoryText
let characters (text:InMemoryText) = text.Characters.ToArray()
let asCharacters (value:string) = value.Select(fun x -> new Character(x)).ToArray()
let writtenTo (value:string) (text:string) (at:int) = (text |> InMemoryText).Write(value, at)

let write (part:string) (at:int) (value:string) =
    let text = value |> asText
    text.Write(part, at) |> ignore;
    text.AsString()

[<Fact>]
let ``Text should fail to construct from null`` () =
    (fun () -> null |> asText |> ignore) |> shouldFail
    
[<Theory>]
[<InlineData("")>]
[<InlineData("Hello, world.")>]
[<InlineData("3.14")>]
[<InlineData("Ready to set the world on fire, hehehehehe.")>]
let ``Text constructed from string should contain same characters`` (value) =
    value |> asText |> characters |> should equal (value |> asCharacters)
    
[<Fact>]
let ``After writing "1" at 0 in "Hello" it should become "1Hello"``() =
    write "1" <| at 0 <| in' "Hello" |> should equal "1Hello"
    
[<Fact>]
let ``After writing "1" at 1 in "Hello" it should become "H1ello"``() =
    write "1" <| at 0 <| in' "Hello" |> should equal "1Hello"