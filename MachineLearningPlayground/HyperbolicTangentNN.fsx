#r @"C:\Users\Matthew\Desktop\MLPlayground\packages\MathNet.Numerics.FSharp.3.8.0\lib\net40\MathNet.Numerics.FSharp.dll"
#r @"C:\Users\Matthew\Desktop\MLPlayground\packages\MathNet.Numerics.3.8.0\lib\net40\MathNet.Numerics.dll"

open System
open MathNet.Numerics
open MathNet.Numerics.LinearAlgebra

type ErrType = string
type ActFn = | ID    //identity: f(x) = x
             | TANH
type Layer = {wMatx: Matrix<float>; actFn: ActFn;}
type Network = Layer list
type NodesVect = {sVect: Vector<float>; xVect: Vector<float>} //each nodesVect will be S => without actFn and X => with actFn
type NodesVectList = NodesVect list

let epochs = 1//50
let lr = 0.04//0.0001

let getActFn (fn: ActFn) : (float->float) =
    match fn with
    | ID -> id
    | TANH -> tanh
let getActFnDeriv (fn: ActFn) : (float->float) =
    match fn with
    | ID -> (fun _ -> 1.0)
    | TANH -> (fun x  -> 1.0 - (tanh(x)**2.0))

//Only works with MSE as metric
let lastLayerDeriv (x_L : Vector<float>) (s_L : Vector<float>) (y : Vector<float>) (theta_L' : ActFn) =
    (2.0*(x_L - y)).PointwiseMultiply(s_L.Map (Func<float, float> (getActFnDeriv (theta_L'))))
let mse (h : Vector<float>) (y: Vector<float>) =
            (h - y).PointwisePower(2.0)


let initNetwork (nodeLst : int list) (actFnLst : ActFn list) : Result<Network, ErrType> =
    let genRandMatx (rows) (cols) : Matrix<float> = CreateMatrix.Random(rows,cols)

    match (nodeLst.Length - actFnLst.Length) with
    | 1 -> 
        match nodeLst.Length  with
        | x when x > 1 ->
            match (List.length (List.filter (fun x -> x > 0) nodeLst)) with
            | y when y = x ->
                List.init (actFnLst.Length) (fun i ->
                    {
                        wMatx = genRandMatx (nodeLst.[i] + 1) (nodeLst.[i+1]);
                        actFn = actFnLst.[i];
                    }
                )
                |> Ok
            | _ -> Error("The specification was incorrect, all item in nodeLst must be positive.")
        | _ -> Error("The specification was incorrect, need at-least two items in nodeLst.")
    | _ -> Error("The specification was incorrect, nodeLst must be exactly one item greater than actFnLst.")
