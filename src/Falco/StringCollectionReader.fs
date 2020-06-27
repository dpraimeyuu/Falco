﻿[<AutoOpen>]
module Falco.StringCollectionReader

open System.Collections.Generic
open Microsoft.Extensions.Primitives
open Falco.StringParser
open Falco.StringUtils

type StringCollectionReader (c : seq<KeyValuePair<string,StringValues>>) = 

    let coll : KeyValuePair<string,StringValues> array = c |> Seq.toArray

    /// Safely retrieve value from StringCollectionReader
    member _.TryGetValue (name : string) =                 
        match coll |> Array.tryFind (fun kvp -> strEquals kvp.Key name) with
        | Some v when v.Value.Count > 0 -> Some v.Value
        | _                             -> None
    
    /// Retrieve value from StringCollectionReader
    member this.GetValue (name : string) = 
        match this.TryGetValue name with
        | Some v -> v 
        | None -> failwith (sprintf "Could not find %s" name)

    member this.TryGetString (name : string)           = name |> this.TryGetValue |> Option.bind (fun v -> Some v.[0])
    member this.TryGet (name : string)                 = this.TryGetString name
    member this.TryGetInt16 (name : string)            = name |> this.TryGetValue |> Option.bind (fun v -> parseInt16 v.[0])
    member this.TryGetInt32 (name : string)            = name |> this.TryGetValue |> Option.bind (fun v -> parseInt32 v.[0])
    member this.TryGetInt (name : string)              = this.TryGetInt32 name
    member this.TryGetInt64 (name : string)            = name |> this.TryGetValue |> Option.bind (fun v -> parseInt64 v.[0])
    member this.TryGetBoolean (name : string)          = name |> this.TryGetValue |> Option.bind (fun v -> parseBoolean v.[0])
    member this.TryGetFloat (name : string)            = name |> this.TryGetValue |> Option.bind (fun v -> parseFloat v.[0])
    member this.TryGetDecimal (name : string)          = name |> this.TryGetValue |> Option.bind (fun v -> parseDecimal v.[0])
    member this.TryGetDateTime (name : string)         = name |> this.TryGetValue |> Option.bind (fun v -> parseDateTime v.[0])
    member this.TryGetDateTimeOffset (name : string)   = name |> this.TryGetValue |> Option.bind (fun v -> parseDateTimeOffset v.[0])
    member this.TryGetGuid (name : string)             = name |> this.TryGetValue |> Option.bind (fun v -> parseGuid v.[0])
    member this.TryGetTimeSpan (name : string)         = name |> this.TryGetValue |> Option.bind (fun v -> parseTimeSpan v.[0])       
    member this.TryArrayString (name : string)         = name |> this.TryGetValue |> Option.map  (fun v -> v.ToArray())
    member this.TryArrayInt16 (name : string)          = name |> this.TryGetValue |> Option.bind (tryParseArray parseInt16)
    member this.TryArrayInt32 (name : string)          = name |> this.TryGetValue |> Option.bind (tryParseArray parseInt32)
    member this.TryArrayInt (name : string)            = this.TryArrayInt32 name
    member this.TryArrayInt64 (name : string)          = name |> this.TryGetValue |> Option.bind (tryParseArray parseInt64)
    member this.TryArrayBoolean (name : string)        = name |> this.TryGetValue |> Option.bind (tryParseArray parseBoolean)
    member this.TryArrayFloat (name : string)          = name |> this.TryGetValue |> Option.bind (tryParseArray parseFloat)
    member this.TryArrayDecimal (name : string)        = name |> this.TryGetValue |> Option.bind (tryParseArray parseDecimal)
    member this.TryArrayDateTime (name : string)       = name |> this.TryGetValue |> Option.bind (tryParseArray parseDateTime)
    member this.TryArrayDateTimeOffset (name : string) = name |> this.TryGetValue |> Option.bind (tryParseArray parseDateTimeOffset)
    member this.TryArrayGuid (name : string)           = name |> this.TryGetValue |> Option.bind (tryParseArray parseGuid)
    member this.TryArrayTimeSpan (name : string)       = name |> this.TryGetValue |> Option.bind (tryParseArray parseTimeSpan)
        
let (?) (q : StringCollectionReader) = q.GetValue

type StringValues with 
    member this.AsString () =
        match this.Count with
        | 0 -> failwith "StringValues is empty"
        | _ -> this.[0]
    
    member this.AsInt16 ()               = this.AsString() |> parseOrFail parseInt16 "Not a valid Int16"
    member this.AsInt32 ()               = this.AsString() |> parseOrFail parseInt32 "Not a valid Int32"
    member this.AsInt ()                 = this.AsInt32 ()
    member this.AsInt64 ()               = this.AsString() |> parseOrFail parseInt64 "Not a valid Int64"
    member this.AsBoolean ()             = this.AsString() |> parseOrFail parseBoolean "Not a valid Boolean"
    member this.AsFloat ()               = this.AsString() |> parseOrFail parseFloat "Not a valid Float"
    member this.AsDecimal ()             = this.AsString() |> parseOrFail parseDecimal "Not a valid Decimal"
    member this.AsDateTime ()            = this.AsString() |> parseOrFail parseDateTime "Not a valid DateTime"
    member this.AsDateTimeOffset ()      = this.AsString() |> parseOrFail parseDateTimeOffset "Not a valid DateTimeOffset"
    member this.AsGuid ()                = this.AsString() |> parseOrFail parseGuid "Not a valid Guid"
    member this.AsTimeSpan ()            = this.AsString() |> parseOrFail parseTimeSpan "Not a valid TimeSpan"
    member this.AsArrayString ()         = this.ToArray()
    member this.AsArrayInt16 ()          = this.AsArrayString() |> tryParseArray parseInt16 |> Option.defaultValue [||]
    member this.AsArrayInt32 ()          = this.AsArrayString() |> tryParseArray parseInt32 |> Option.defaultValue [||]
    member this.AsArrayInt ()            = this.AsArrayInt32 ()
    member this.AsArrayInt64 ()          = this.AsArrayString() |> tryParseArray parseInt64 |> Option.defaultValue [||]
    member this.AsArrayBoolean ()        = this.AsArrayString() |> tryParseArray parseBoolean |> Option.defaultValue [||]
    member this.AsArrayFloat ()          = this.AsArrayString() |> tryParseArray parseFloat |> Option.defaultValue [||]
    member this.AsArrayDecimal ()        = this.AsArrayString() |> tryParseArray parseDecimal |> Option.defaultValue [||]
    member this.AsArrayDateTime ()       = this.AsArrayString() |> tryParseArray parseDateTime |> Option.defaultValue [||]
    member this.AsArrayDateTimeOffset () = this.AsArrayString() |> tryParseArray parseDateTimeOffset |> Option.defaultValue [||]
    member this.AsArrayGuid ()           = this.AsArrayString() |> tryParseArray parseGuid |> Option.defaultValue [||]
    member this.AsArrayTimeSpan ()       = this.AsArrayString() |> tryParseArray parseTimeSpan |> Option.defaultValue [||]


