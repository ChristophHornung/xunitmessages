# xunit.AssertMessages

Adds assert messages to all xunit `Assert` calls.

## Usage
All methods are static on the `AssertM` class.

Simply add the nuget package to your test project and add

``` csharp
// Alias the assert to quickly migrate existing code to use AssertM.
using Assert = XunitAssertMessages.AssertM;
```

to your usings. (For an example see the `xUnitMessages.Example` project.)
All aserts will work identical to before but will now have an _optional_ additional 
parameter that adds a custom message. E.g.

``` csharp
int expectedShippingDelay = 10;
int actualShippingDelay = 9;
Assert.Equal(expectedShippingDelay, actualShippingDelay, "The shipping delay is incorrectly configured.");
```

will output
```
Xunit.Sdk.XunitException
The shipping delay is incorrectly configured.
```

## Motivation
When working with tools like SpecFlow or other behaviour driven frameworks the result of a test
is often not read by a programmer but by domain experts. In that scenario an error message like
```
The vat was calculated incorrectly, the test expects 24.40$ but the calculated value is 34.40$
```

is a lot more helpful than 
```
expected 2440 got 3440
```
.

The default `xunit.assert` package does not support custom messages except on the `Assert.True`
and `Assert.False` methods, but (re-)writing tests with only those two methods is a lot of overhead.