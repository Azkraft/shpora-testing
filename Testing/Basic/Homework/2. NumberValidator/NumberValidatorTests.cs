using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1)]
    [TestCase(1, -1)]
    [TestCase(1, 2)]
    [TestCase(-1, 2, true)]
    [TestCase(-1, 2, false)]
    [TestCase(1, 0, false, false)]
    public void CheckParametersValidation(
	    int precision,
	    int scale = 0,
	    bool onlyPositive = false,
	    bool shouldThrowException = true)
    {
	    var func = () => new NumberValidator(precision, scale, onlyPositive);
	    var message = $"""
	                   parameters are
	                   precision: {precision},
	                   scale: {scale},
	                   onlyPositive: {onlyPositive}

	                   """;


	    if (shouldThrowException)
		    func.Should().Throw<ArgumentException>(message);
	    else
		    func.Should().NotThrow(message);
    }

    [TestCase(null, false, 1)]
    [TestCase("", false, 1)]
    [TestCase("  \n\t  ", false, 1)]
    [TestCase("a.sd", false, 3, 2)]
	[TestCase("0", true, 17, 2)]
    [TestCase(" 0", false, 17, 2)]
    [TestCase("0 ", false, 17, 2)]
	[TestCase(".0", false, 17, 2)]
    [TestCase("0.", false, 17, 2)]
	[TestCase("0.0", true, 17, 2)]
    [TestCase("0.00", true, 17, 2)]
	[TestCase("0.000", false, 17, 2)]
	[TestCase("00.00", false, 3, 2)]
    [TestCase("-0.00", false, 3, 2)]
    [TestCase("-0.00", false, 4, 2)]
    [TestCase("-0.00", true, 4, 2, false)]
	[TestCase("+0.00", false, 3, 2)]
    [TestCase("+1.23", true, 4, 2)]
    [TestCase("+1.23", false, 3, 2)]
    [TestCase("-1.23", false, 3, 2)]
    [TestCase("+123", true, 4)]
	[TestCase("+123", false, 3)]
    [TestCase("-123", false, 3)]
    [TestCase("-123", false, 4)]
    [TestCase("-123", true, 4, 0, false)]
	[TestCase("123", true, 3)]
	public void CheckNumberValidation(
	    string number,
        bool isValid,
	    int precision,
	    int scale = 0,
	    bool onlyPositive = true)
    {
	    new NumberValidator(precision, scale, onlyPositive)
		    .IsValidNumber(number)
		    .Should()
		    .Be(isValid);
    }
}