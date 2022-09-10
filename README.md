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

You can includ the original message from xUnit via `{xMsg}`

Example:
```
Assert.Equal(10, 9, "This is so wrong. {xMsg}");
```

## Motivation
When working with tools like SpecFlow or other behaviour driven frameworks the result of a test
is often not read by a programmer but by domain experts. In that scenario an error message like
```
The vat was calculated incorrectly, the test expects 24.40$ but the calculated value is 34.40$
```

is a lot more helpful than 
```
Expected: 2440 Actual: 3440
```
.

The default `xunit.assert` package does not support custom messages except on the `Assert.True`
and `Assert.False` methods, but (re-)writing tests with only those two methods is a lot of overhead.

## Technical details
Please note the following technical details:

* The type of exception thrown by assert will always be `Xunit.Sdk.XunitExcepion`
* The original xunit.assert exception will not be included as inner exception, instead 
the original inner exception will be kept.
* If using `{xMsg}` to include the original xunit.assert exception message a newline will be prepended.
(As most exceptions will already include newlines in their message)