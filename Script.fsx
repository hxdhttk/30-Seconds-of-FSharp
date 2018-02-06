open System

let chunk numbers size = numbers |> Array.chunkBySize size

let concat first second = Array.concat [first; second]