# 30 seconds of F#

> 你可以在30秒或更短时间内收集有用的F#代码片段。

- 使用 <kbd>Ctrl</kbd> + <kbd>F</kbd> 或者 <kbd>command</kbd> + <kbd>F</kbd> 来查找代码片段。
- 代码片段基于 F#，如果你还不熟悉可以在[这里](https://docs.microsoft.com/zh-cn/dotnet/fsharp/tour)学习。
- 代码片段转译自 [little-java-functions](https://github.com/shekhargulati/little-java-functions)

## 目录

### 📚 Array (数组相关)

<details>
<summary>详细信息</summary>

* [`chunk`](#chunk)
* [`concat`](#concat)
* [`countOccurrences`](#countoccurrences)
* [`deepFlatten`](#deepflatten)
* [`difference`](#difference)
* [`differenceWith`](#differencewith)
* [`distinctValuesOfArray`](#distinctvaluesofarray)
* [`dropElements`](#dropelements)
* [`dropRight`](#dropright)
* [`everyNth`](#everynth)
* [`filterNonUnique`](#filternonunique)
* [`flatten`](#flatten)
* [`flattenDepth`](#flattendepth)
* [`groupBy`](#groupby)
* [`head`](#head)
* [`initial`](#initial)
* [`initializeArrayWithRange`](#initializearraywithrange)
* [`initializeArrayWithValues`](#initializearraywithvalues)
* [`intersection`](#intersection)
* [`isSorted`](#issorted)
* [`join`](#join)
* [`nthElement`](#nthelement)
* [`pick`](#pick)
* [`reducedFilter`](#reducedfilter)
* [`remove`](#remove)
* [`sample`](#sample)
* [`sampleSize`](#samplesize)
* [`shuffle`](#shuffle)
* [`similarity`](#similarity)
* [`sortedIndex`](#sortedindex)
* [`symmetricDifference`](#symmetricdifference)
* [`tail`](#tail)
* [`take`](#take)
* [`takeRight`](#takeright)
* [`union`](#union)
* [`without`](#without)
* [`zip`](#zip)
* [`zipObject`](#zipobject)

</details>

### ➗ Math (数学相关)

<details>
<summary>详细信息</summary>

* [`average`](#average)
* [`gcd`](#gcd)
* [`lcm`](#lcm)
* [`findNextPositivePowerOfTwo`](#findnextpositivepoweroftwo)
* [`isEven`](#iseven)
* [`isPowerOfTwo`](#ispoweroftwo)
* [`generateRandomInt`](#generaterandomint)

</details>

## Array

### chunk

将数组分割成特定大小的小数组。

```fsharp
let chunk numbers size = numbers |> Array.chunkBySize size
```

<br>[⬆ 回到顶部](#目录)

### concat

将两个数组合并为一个数组。

```fsharp
let concat first second = Array.concat [first; second]
```

<br>[⬆ 回到顶部](#目录)

### countOccurrences

计算数组中某个值出现的次数。

使用 `Array.filter` 计算等于指定值的值的总数。

```fsharp
let countOccurrences numbers value = numbers |> ((Array.filter ((=) value)) >> Array.length)
```

<br>[⬆ 回到顶部](#目录)

### deepFlatten

数组扁平化，需要将各元素装箱为 `obj`。

使用递归实现，以及 `Computation Expression` 简化表达。

```fsharp
let rec deepFlatten (input: obj array) =
    [| for element in input do
           let t = element.GetType()
           if t.IsArray then
               yield! deepFlatten (element :?> obj array)
           else
               yield element |]
```

<br>[⬆ 回到顶部](#目录)

### difference

返回两个数组之间的差异。

使用 `Array.except` 实现。 

```fsharp
let difference first second = Array.except second first
```

<br>[⬆ 回到顶部](#目录)

### differenceWith

从比较器函数不返回true的数组中筛选出所有值，将comparator（比较器）作为函数参数传入。

使用 `Array.filter` 和 `Array.exists` 查找相应的值。

```fsharp
let differenceWith first second comparator =
    first
    |> Array.filter (fun a -> second |> Array.exists (fun b -> comparator a b) |> not)
```

<br>[⬆ 回到顶部](#目录)

### distinctValuesOfArray

返回数组的所有不同值。 

使用 `Array.distinct` 去除所有重复的值。

```fsharp
let distinctValuesOfArray elements = elements |> Array.distinct
```

<br>[⬆ 回到顶部](#目录)

### dropElements

移除数组中的元素，直到传递的函数返回true为止。返回数组中的其余元素。 

使用 `Array.skipWhile` 实现。

```fsharp
let dropElements elements condition = elements |> Array.skipWhile condition
```

<br>[⬆ 回到顶部](#目录)

### dropRight

返回一个新数组，从右边移除n个元素。 

检查n是否短于给定的数组，返回相应的数组切片或空数组，使用 `Option` 实现 `null` 安全。

```fsharp
let dropRight elements n =
    match n < 0 with
    | true -> None
    | false -> let arrayLength = Array.length elements
               match n < arrayLength with
               | true -> Some elements.[.. (arrayLength - n - 1)]
               | false -> Some [||]
```

<br>[⬆ 回到顶部](#目录)

### everyNth

返回数组中的每个第n个元素。 

使用 `Array.indexed` 创建一个包含索引的新数组，通过索引需要满足的的条件筛选原数组的值。

```fsharp
let everyNth elements nth =
    [| for index, element in elements |> Array.indexed do
           if index % nth = nth - 1 then
               yield element |]
```

<br>[⬆ 回到顶部](#目录)

### indexOf

查找数组中元素的索引，在不存在元素的情况下返回-1。 

使用 `Array.tryFindIndex` 安全查找数组中元素的索引。

```fsharp
let indexOf elements el = elements |> Array.tryFindIndex ((=) el)
                                   |> function | Some x -> x | None -> -1
```

<br>[⬆ 回到顶部](#目录)

### lastIndexOf

查找数组中元素的最后索引，在不存在元素的情况下返回-1。 

使用 `Array.tryFindIndexBack` 安全查找数组中元素的索引。

```fsharp
let lastIndexOf elements el = elements |> Array.tryFindIndexBack ((=) el)
                                       |> function | Some x -> x | None -> -1
```

<br>[⬆ 回到顶部](#目录)

### filterNonUnique

筛选出数组中的非唯一值。

```fsharp
let filterNonUnique elements = elements |> Array.groupBy id
                                        |> Array.filter (snd >> Array.length >> (=) 1)
                                        |> Array.map fst
```

<br>[⬆ 回到顶部](#目录)

### flatten

使数组扁平。

```fsharp
let flatten (elements: obj array) =
    [| for element in elements do
           let t = element.GetType()
           if t.IsArray then 
               yield! (element :?> obj array)
           else
               yield element |]
```

<br>[⬆ 回到顶部](#目录)

### flattenDepth

将数组压平到指定的深度。

```fsharp
let rec flattenDepth (elements: obj array) depth =
        match depth with
        | 0 -> elements
        | _ -> [| for element in elements do
                      let t = element.GetType()
                      if t.IsArray then
                          yield! flattenDepth (element :?> obj array) (depth - 1)
                      else
                          yield element |]
```

<br>[⬆ 回到顶部](#目录)

### groupBy

根据给定函数对数组元素进行分组。

使用 `Arrays.groupBy` 分组。

```fsharp
let groupBy elements func = elements |> Array.groupBy func
```

<br>[⬆ 回到顶部](#目录)

### initial

返回数组中除去最后一个的所有元素。

直接使用切片返回除最后一个之外的所有元素。

```fsharp
let initial (elements: 'a array) = elements.[..(elements.Length - 2)]
```

<br>[⬆ 回到顶部](#目录)

### initializeArrayWithRange

初始化一个数组，该数组包含在指定范围内的数字，传入 `start` 和 `end`。

```fsharp
let initializeArrayWithRange ``end`` start = [| start .. ``end`` |]
```

<br>[⬆ 回到顶部](#目录)

### initializeArrayWithValues

使用指定的值初始化并填充数组。

```fsharp
let initializeArrayWithValues n value = [| for _ in 1 .. n -> value |]
```

<br>[⬆ 回到顶部](#目录)

### intersection

返回两个数组中存在的元素列表。 

在 a 上使用 `Array.filter` 来筛选包含在 b 中的值。

```fsharp
let intersection first second = first |> Array.filter (fun x -> set second |> Set.contains x)
```

<br>[⬆ 回到顶部](#目录)

### isSorted

如果数组按升序排序，则返回 `1`，如果数组按降序排序，返回 `-1`，如果没有排序，则返回 `0`。

Naive implementation，将会改进。

```fsharp
let isSorted arr =
    match arr = (arr |> Array.sort), arr = (arr |> Array.sortDescending) with
    | true, _ -> 1
    | _, true -> -1
    | _, _ -> 0
```

<br>[⬆ 回到顶部](#目录)

### join

将数组的所有元素连接到字符串中，并返回此字符串。

使用 `String.concat` 将元素组合成字符串。

```fsharp
let join arr separator ``end`` =
    arr |> Array.map string
        |> String.concat separator
        |> (fun x -> x + ``end``)
```

<br>[⬆ 回到顶部](#目录)

### nthElement

返回数组的第n个元素。

```fsharp
let nthElement (arr: 'a array) n =
    match n > 0 with
    | true -> arr.[n - 1]
    | false -> arr.[arr.Length + n]
```

<br>[⬆ 回到顶部](#目录)

### pick

从对象中选择与给定键对应的键值对。

使用 `Map` 将所有的 `seq<'a * 'b>` 转换为Map。

```fsharp
let pick obj arr =
    arr |> Array.filter (fun x -> obj |> Map.containsKey x)
        |> Array.map (fun x -> x, obj.[x])
        |> Map
```

<br>[⬆ 回到顶部](#目录)

### reducedFilter

根据条件筛选对象数组，同时筛选出未指定的键。

使用 `Array.filter` 根据谓词 `fn` 过滤数组，以便返回条件为真的对象。
对于每个过滤的Map对象，创建一个新的Map，其中包含 `keys` 中的键。最后，将Map对象收集到一个数组中。

```fsharp
let reducedFilter data keys fn =
    data
    |> Array.filter fn
    |> Array.map (fun el -> keys |> Array.filter (fun x -> el |> Map.containsKey x)
                                 |> Array.map (fun x -> x, el.[x])
                                 |> Map)
```

<br>[⬆ 回到顶部](#目录)

### sample

从数组中返回一个随机元素。

使用 `Random().NextDouble()` 生成一个随机数，然后将它乘以数组的 `length`，然后使用 `floor >> int` 获得一个最近的整数，该方法也适用于字符串。

```fsharp
let sample (arr: 'a array) = let rnd = Random()
                             arr.[(rnd.NextDouble() * (float arr.Length)) |> (floor >> int)]
```

<br>[⬆ 回到顶部](#目录)

### sampleSize

从 `array` 到 `array` 大小的唯一键获取 `n` 个随机元素。

根据[Fisher-Yates算法](https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle)。

```fsharp
let sampleSize input n =
    let rnd = Random()
    let rec inner (arr: 'a array) m =
        match m with
        | 0 -> arr
        | _ -> let i = (rnd.NextDouble() * (float m)) |> (floor >> int)
               let tmp = arr.[i]
               arr.[i] <- arr.[m]
               arr.[m] <- tmp
               inner arr (m - 1)
    let copy = input |> Array.copy
    let length = input.Length
    if n > length then (inner copy (length - 1)) else (inner copy n) |> Array.take n
```

<br>[⬆ 回到顶部](#目录)

### shuffle

将数组值的顺序随机化，返回一个新数组。

根据 [Fisher-Yates 算法](https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle) 重新排序数组的元素。

```fsharp
let shuffle input =
    let rnd = Random()
    let rec inner (arr: 'a array) m =
        match m with
        | 0 -> arr
        | _ -> let i = (rnd.NextDouble() * (float m)) |> (floor >> int)
               let tmp = arr.[i]
               arr.[i] <- arr.[m]
               arr.[m] <- tmp
               inner arr (m - 1)
    let copy = input |> Array.copy
    let length = input.Length
    inner copy (length - 1)
```

<br>[⬆ 回到顶部](#目录)

### similarity

返回出现在两个数组中的元素数组。

与 `intersection` 实现一致，因为 F# 的函数是`自动泛化`的不需要专门实现同一函数的泛型版本。

```fsharp
let similarity first second = first |> Array.filter (fun x -> set second |> Set.contains x)
```

<br>[⬆ 回到顶部](#目录)

### sortedIndex

返回值应该插入到数组中的最低索引，以保持其排序顺序。

检查数组是否按降序（松散地）排序。 建立索引的数组并使用 `Array.filter` 来找到元素应该被插入的合适的索引。

```fsharp
let sortedIndex (arr: 'a array) el =
    let isDescending = arr.[0] > arr.[1]
    [| 0 .. arr.Length - 1 |]
    |> Array.filter (fun index -> if isDescending then el >= arr.[index] else el <= arr.[index])
    |> Array.tryHead
    |> function | Some x -> x | None -> arr.Length
```

<br>[⬆ 回到顶部](#目录)

### symmetricDifference

返回两个数组之间的对称差异。

从每个数组中创建一个 `Set`，然后使用 `Array.filter` 来保持其他值不包含的值。最后，连接两个数组并创建一个新数组并返回。

```fsharp
let symmericDifference first second =
    [| first |> Array.filter (fun x -> set second |> Set.contains x);
       second |> Array.filter (fun x -> set first |> Set.contains x) |]
    |> Array.concat
```

<br>[⬆ 回到顶部](#目录)

### tail

返回数组中除第一个元素外的所有元素。

```fsharp
let tail arr = arr |> Array.tail
```

<br>[⬆ 回到顶部](#目录)

### take

返回一个从开头删除n个元素的数组。

```fsharp
let take arr n = arr |> Array.skip n
```

<br>[⬆ 回到顶部](#目录)

### takeRight

返回从末尾移除n个元素的数组。

使用切片语法。

```fsharp
let takeRight (arr: 'a array) n = arr.[(arr.Length - n)..]
```

<br>[⬆ 回到顶部](#目录)

### union

返回两个数组中任何一个中存在的每个元素一次。

```fsharp
let union first second = set first |> Set.union (set second) |> Array.ofSeq
```

<br>[⬆ 回到顶部](#目录)

### without

筛选出具有指定值之一的数组的元素。

```fsharp
let without arr elements = arr |> Array.filter (fun x -> set elements |> Set.contains x |> not)
```

<br>[⬆ 回到顶部](#目录)

### zip

根据原始数组中的位置创建元素数组。

```fsharp
let zip (arrays: 'a array array) =
    let maxIndex = arrays |> Array.map (fun x -> x.Length)
                          |> Array.max
    [| for index in [0 .. maxIndex - 1] do
           yield arrays |> Array.map (fun x -> if index < x.Length then Some x.[index] else None) |]
```

<br>[⬆ 回到顶部](#目录)

### zipObject

给定有效的属性标识符数组和值数组，返回将属性与值关联的对象。

```fsharp
let zipObject props values =
    props
    |> Array.mapi (fun index prop -> prop, if index < (values |> Array.length) then Some values.[index] else None)
    |> Map
```

<br>[⬆ 回到顶部](#目录)

## Maths

### average

返回两个或两个以上数字的平均值。

```fsharp
let inline average (arr: ^a array when ^a : (static member (+) : ^a * ^a -> ^a)) = arr |> Array.average
```

<br>[⬆ 回到顶部](#目录)

### gcd

计算一系列数字的最大公约数(gcd)。

使用 `Array.reduce` 和 GCD（使用递归公式）计算一组数字的最大公约数。

```fsharp
let gcd numbers =
    let rec inner a b =
        match b = 0 with
        | true -> a
        | false -> inner b (a % b)
    numbers |> Array.reduce inner
```

<br>[⬆ 回到顶部](#目录)

### lcm

计算数字数组的最低公共倍数(LCM)。

使用 `Array.reduce` 和 LCM公式(使用递归)来计算数字数组的最低公共倍数。

```fsharp
let lcm numbers =
    let rec gcd' a b =
        match b = 0 with
        | true -> a
        | false -> gcd' b (a % b)
    let lcm' a b = (a * b) / (gcd' a b)
    numbers |> Array.reduce lcm'
```

<br>[⬆ 回到顶部](#目录)

### findNextPositivePowerOfTwo

查找大于或等于该值的下一个幂。

```fsharp
let findNextPositivePowerOfTwo value = 1 <<< Convert.ToString(value - 1, 2).Length
```

<br>[⬆ 回到顶部](#目录)

### isEven

检查数字是否是偶数。

```fsharp
let isEven value = (value &&& 1) = 0
```

<br>[⬆ 回到顶部](#目录)

### isPowerOfTwo

检查一个值是2的正幂。

```fsharp
let isPowerOfTwo value = (value > 0) && ((value &&& (~~~value + 1)) = value)
```

<br>[⬆ 回到顶部](#目录)

### generateRandomInt

生成一个介于 `Int32.MinValue` 和 `Int32.MaxValue` 之间的随机数。

```fsharp
let generateRandomInt () =
    let rnd = Random()
    rnd.Next(Int32.MinValue, Int32.MaxValue)
```

<br>[⬆ 回到顶部](#目录)
