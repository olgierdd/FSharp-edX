open System
 
 
let goldenRatio = ( 1.0 + System.Math.Sqrt(5.0)) / 2.0
 
let getNumbers =
    printfn "Enter number or Q to finish sequence"
    seq {
        let mutable cont = true
        while cont do
            Console.Write("Enter number: ");
            let line = lazy Console.ReadLine()
            let readNumber = line.Value
            cont <- readNumber.ToUpper() <> "Q"
            if cont then
                let parsed, value = Int32.TryParse(readNumber)
                if parsed then yield value
    }
    
 
[<EntryPoint>]
let main argv = 
    let numbers = getNumbers
                  |> Seq.map(fun x -> (x, ( float x ) * goldenRatio))
                  |> Seq.toList
    printfn "You entered %d numbers" numbers.Length
    for x in numbers do
        printfn "[%d, %f]" <|| x
    Console.ReadKey()
    0