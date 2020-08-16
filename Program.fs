// F#

open System

// Customer Contains two strings, but doesn't manage them. Just formats
// It's loosely associated with Age
// The focus is on my responsibility to make the type do what I want
type Customer(fName:string,lName:string) =
    member private x.FirstName=fName
    member private x.LastName=lName
    member x.Age:int=0
    member x.FullName=x.FirstName + " " + x.LastName
    member x.ReverseName() =
        let temp=x.FullName.ToCharArray() |> Array.toList |> List.rev |> List.toArray
        String.Join(null,temp)
    override x.ToString() =
        sprintf "%s" x.FirstName

// By passing the primitive type to manage
// The system can manage groups of them
// The focus is on a pre-canned set of responsibilities I don't have to worry about
let myHobbies=new System.Collections.Generic.List<string>()
myHobbies.Add("Stamp Collecting")
myHobbies.Add("Needlepoint")
myHobbies.Add("Model Building")

// Generic.List<> is a generic type. Generic.List<string> is a parameterized type

// Here we have a completely new concept
// I am technically-reponsible for everything!
// So Quadruple only means what I want it to mean and nothing else
type Quadruple<'a>(a:^TYPE1 when ^TYPE1:>IEquatable<_>, b:^TYPE2, c:^TYPE3, d:^TYPE4) =
    member x.Item1 = a
    member x.Item2 = b
    member x.Item3 = c
    member x.Item4 = d
    // Since it's completely new, we're in charge of making it work
    // With anybody who's using us
    override x.ToString() =
        sprintf "(%s, %s, %s, %s)" (string<'TYPE1> x.Item1) (string<'TYPE2> x.Item2) (string<'TYPE3> x.Item3) (string<'TYPE4> x.Item4)
    // Two ways to do this, overrides and implementing NET interfaces
    // IEquatable shouldn't box, whereas overriding equals should. Have no idea if implemented or not
    override x.Equals obj = 
        match obj with
                  | :? Quadruple<_> as other -> (x.Item1 :> IEquatable<_>).Equals (other.Item1:> IEquatable<_>)
                  | _                    -> false
    override x.GetHashCode() = hash x.Item1
// NOTE: 'a is a dynamic type, filled-in at runtime. It's the default .NET generic
// ^A is a static type, filled-in at compile-time. It allows us to use type constraints
// QUACK!


[<EntryPoint>]
let main argv =
    printfn "Hi There!\n"
    let k=Customer("Jim", "Bob")
    printf "Customer's Name %s\n" k.FullName
    printf "Customer's Name Backwards %A\n" (k.ReverseName())

    printf "\n"
    printf "My Hobbies %A\n" myHobbies
    let myTuple=("seven",9)
    printf "My Tuple %A\n" myTuple

    let myQuadruple=Quadruple(1,2,3,"cat")
    printf "My Quadruple %s\n" (myQuadruple.ToString())

    0 // return an integer exit code

    // COUPLING AND COHESION ARE CONTEXTUAL
    // List<_> is highly-cohesive and loosely-coupled
    // But only at the code level! It's only cohesive at the Biz
    // level with "Add", something I could easily do somewhere else

    // Quadruple is highly-cohesive and loosely-coupled
    // Both at the Sys and Biz level. It only does the things
    // I require it to do

    // Customer is not-so-cohesive and not-so-coupled
    // at both levels
    // Age:int can mean anything to anybody, Poor cohesion
    // I can only use it with Customers that have two names
    // else it does nothing. Poor coupling (the same thing could (and should)
    // be done with two functions.

    // myHobbies exists for me to add things to them. Nothing else.
    // It shouldn't be a type. It does nothing. It's a static struct
    
    // Customer doesn't do anything that two functions don't do, and
    // those functions don't have to have anything to do with customers
    // It shouldn't be a type

    // Quadruple is something never seen and it's able to compare instances
    // of itself.If we had functions that did things to Quadruples and only
    // quadruples, it should exist. Otherwise it's a struct

    // BEHAVIOR SHOULD ALWAYS PULL OUT STRUCTURE
