# LambdaSharp
Functional programming language extensions for C#. The hope is to make 
C# a more declarative, terse and functional programming language through 
the use of constructs from functional-first languages like F#, Clojure, 
Haskell... 

## Featuring
* Match - for basic pattern matching
* Pipe - to replace C#'s lack of a a pipeline operator
* In - easy, inline membership testing
* Memoize - Wraps a function call, and caches input to output mappings
* Combinatorics - containing various combinatorial functions (just 
Cartesian Product at the moment though) 


The below examples are a little contrived, but from them you should 
begin to see the value here.

Note: not all IDEs can place breakpoints inside lambdas. This kind of 
code may affect your debugging abilities. I recommend JetBrains IDEs: 
Rider or Resharper for Visual Studio.


## Match
Basic pattern matching. 

### For Example: 
```
var getThanks = new Match<User, string>() 
    .When(user => user.AccountType == AccountType.Unpaid, user => $"Thanks, Freeloader {user.FirstName}")
    .When(user => user.Contributions < 1000, user => $"Thanks, Bronze Tier User {user.Name}")
    .When(user => user.Contributions >= 1000 && user.Contributions < 10000, "Thanks Mr. Silver!")
    .When(user => user.Contributions >= 10000, user => $"Thanks Mrs. Gold! We love you {user.FirstName}")
    .Otherwise(user => $"We thank you for your contribution, and hope you suck less in the future, {user.FirstName}");
                          
var thanks = users.ToDictionary(user => user, user => getThanks.For(user));
                                
```


## Pipe
A C# pipe operator! Now you can create lazy/eager sequences of function calls without nesting calls.

### For Example: 
```
// BAD
var availableUglyDoctors = GetDoctors(GetHospital(GetPatientLocation(GetPatientAddress(GetPatient(GetPatientProvider(), patientId)))));

// GOOD
var availablePrettyDoctors = new EagerPipe(getPatientProvider())
    .Into(provider => GetPatient(provider, patientId))
    .Into(GetPatientAddress)
    .Into(GetPatientLocation)
    .Into(GetHospital)
    .Into(GetDoctors)
    .Result; // Eager: each line is run as it is encountered

// BEST
var availableSuperModelDoctors = new Pipe(GetPatientProvider())
    .Into(provider => GetPatient(provider, patientId))
    .Into(GetPatientAddress)
    .Into(GetPatientLocation)
    .Into(GetHospital)
    .Into(GetDoctors)
    .Result(); // Lazy: none of the above is run until here
```


## In
An inline membership tester. This is an extension method on T, allowing 
you to call it on any member whose membership in a set interests you.

### For Example: 
```
if(user.ActiveUserType.In(UserType.Admin, UserType.Root, UserType.Manager) {
     fancyUsers.Add(user);
}
```


## Memoize
Just a standard memoizer

### For Example: 
```
// Take an expensive input sanitization process, and cache results, this way an input doesn't need recalculation.
var sanitizeMemoized = Memoize(SanitizeUserInput);
```


## Combinatorics - Cartesian Product
Numerous problems come down to a Cartesian Product between two sets. 
This makes it much easier.

### Code-free Example
We have two collections: 1-5, A-F, and we want to make every unique 
combination. That is a Cartesian Product.
```
     A    B    C    D    E    F 
  ______________________________
1 |  1A   1B   1C   1D   1E   1F
2 |  2A   2B   2C   2D   2E   2F
3 |  3A   3B   3C   3D   3E   3F
4 |  4A   4B   4C   4D   4E   4F
5 |  5A   5B   5C   5D   5E   5F
```

### For Example: 
```
var users = GetUsersFromWhoCares();
var thirdPartyEulasToAgreeTo = GetThemFromSomewhere();
var userEulaCombos = new List<(User, Eula)>(); // We need a EULA pairing for each user-EULA combination

// BAD
foreach(user in users) {
    foreach(eula in thirdPartyEulasToAgreeTo) { 
        userEulaCombos.Add((user, eula)); 
    }
}

// GOOD
userEulaCombos = users.CartesianProduct(thirdPartyEulasToAgreeTo).ToList();
```
