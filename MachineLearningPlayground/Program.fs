open System
// holy fuk
open Accord.Statistics.Models.Regression
open Accord.Statistics.Models.Regression.Fitting

let logisticRegression () = 
    let input = [|
                    [| 55.; 0. |]; 
                    [| 28.; 0. |]; 
                    [| 65.; 1. |]; 
                    [| 46.; 0. |]; 
                    [| 86.; 1. |]; 
                    [| 56.; 1. |]; 
                    [| 85.; 0. |]; 
                    [| 33.; 0. |]; 
                    [| 21.; 1. |]; 
                    [| 42.; 1. |]; 
                |]

    let output = [|0.; 0.; 0.; 1.; 1.; 1.; 0.; 0.; 0.; 1.|]

    let regression = new LogisticRegression 2 // dep

    let teacher = new IterativeReweightedLeastSquares(regression)

    let rec teach () : unit = 
        match teacher.Run(input, output) with // dep
        | x when x > 0.001 -> teach ()
        | _ -> ()

    teach()

    printfn "Please input your age, seperated by (,)"
    let input = Console.ReadLine ()

    if String.IsNullOrWhiteSpace (input) then
        // throw new exeption
        Environment.Exit(1)

    let computeOutput = regression.Compute ([| 18.; 0. |]) // dep 
    printfn "High probability of lung cancer - %A" (computeOutput * 100.)

logisticRegression ()

Console.ReadKey() |> ignore
()
