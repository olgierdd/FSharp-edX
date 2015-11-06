module Module4

open System
open System.IO

let Gravity = 9.81

let calcDistance speed theta =
    Math.Pow(speed, 2.0) * Math.Sin(2.0 * theta) / Gravity 

let angleOfReach distance speed =
    let maxDistance = calcDistance speed (Math.PI / 4.0)
    if distance <= maxDistance then
        Some(Math.Asin( Gravity * distance / speed / speed ) / 2.0)
    else
        None

let calcAngle y x =
    Math.Atan(y / x)

let hitTarget distance expectedDistance = abs( distance - expectedDistance) <= 0.01

type Target = {
    X : double
    Y : double
    Speed: double
    ExpectedDistance: double
    Name : string
}

type Result = {
    TravelledDistance : double
    HitTheExpectedDistance: bool
    AngleOfReach: Option<double>
    Name: string
}

let ParseValue value =
    let (succeeded, result) = Double.TryParse value
    if succeeded then result else Double.NaN

let ReadInput (fileName: string) = 
    use input = new StreamReader(fileName)
    [  while not input.EndOfStream do
        let line = input.ReadLine()
        let values = line.Split(',')
        if values.Length = 5 then
            yield {
                X = ParseValue values.[0]
                Y = ParseValue values.[1]
                Speed = ParseValue values.[2]
                ExpectedDistance = ParseValue values.[3]
                Name = values.[4]
            }
     ]

let GetResults targets = 
    seq {
        for t in targets do
            let angle = (calcAngle t.X t.Y)
            let distance =  calcDistance t.Speed angle
            let expectedDistance = t.ExpectedDistance
            let requiredAngle = angleOfReach t.Speed t.ExpectedDistance
            //printfn "%A distance: %A expected: %A orginal angle: %A requiredAngle:%A" t.Name distance expectedDistance angle requiredAngle
            yield {
                TravelledDistance = distance
                HitTheExpectedDistance =  hitTarget distance expectedDistance 
                AngleOfReach = requiredAngle
                Name = t.Name
        }
    }
[<EntryPoint>]
let main argv = 
    try
        let fileName =
            match argv.Length with
            | x when x > 0 -> argv.[0]
            | _ ->  Console.Write("Enter path to the input file name: ")
                    Console.ReadLine()

        let targets = ReadInput fileName

        let results = GetResults targets
        for result in results do
            printf "The %A:" result.Name 
            printfn " %s hit the expected distance" (if result.HitTheExpectedDistance then "" else "didn't")
            if not result.HitTheExpectedDistance then 
                match result.AngleOfReach with
                    | Some(deg) -> printfn "The angle required to reach its distance is %A degrees." deg
                    | None -> printfn "There is no angle possible to hit the expected distance."
            printfn ""
        Console.ReadKey() |> ignore
        0
    with
    | :? System.IO.FileNotFoundException ->
        Console.Write("File Not Found. Press a key to exit")
        Console.ReadKey() |> ignore
        -1
    | _ ->
        Console.Write("Something else happened")
        Console.ReadKey() |> ignore
        -1