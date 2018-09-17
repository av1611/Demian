module Demian.Tests.FSharp.Text.InMemoryTextTest

open System.Linq
open Demian
open FsUnit
open System;
open Xunit;

let at x = x

let asText value = (value) |> InMemoryText
let characters (text:InMemoryText) = text.Characters.ToArray()
let asCharacters (value:string) = value.Select(fun x -> new Character(x)).ToArray()
let writtenTo (value:string) (text:string) (at:int) = (text |> InMemoryText).Write(value, at)

[<Fact>]
let ``Text should fail to construct from null`` () =
    (fun () -> asText null |> ignore) |> shouldFail
    
[<Theory>]
[<InlineData("")>]
[<InlineData("Hello, world.")>]
[<InlineData("3.14")>]
[<InlineData("Ready to set the world on fire, hehehehehe.")>]
let ``Text constructed from string should contain same characters`` (value) =
    value |> asText |> characters |> should equal (value |> asCharacters)
    
[<Fact>]
let ``After writing "1" at 0 in "Hello" it should become "1Hello"``() =
    "1" |> writtenTo "Hello" <| at 0 |> should equal "1Hello"