using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 0, false, "Non-positive total number of digits")]
    [TestCase(1, -1, false, "Negative number of digits in fractional part")]
    [TestCase(1, 2, false, 
	    "Number of digits in fractional part greater or equal to total number of digits")]
    public void NumberValidatorConstructor_ShouldThrow_ArgumentException(
	    int precision,
	    int scale,
	    bool onlyPositive,
	    string message)
    {
	    var func = () => new NumberValidator(precision, scale, onlyPositive);

	    func.Should().Throw<ArgumentException>(message);
	}

    [TestCase(1, 0, false,
	    "Positive precision, non-negative scale and scale less than precision")]
	public void NumberValidatorConstructor_ShouldNotThrow_Exception(
	    int precision,
	    int scale,
	    bool onlyPositive,
	    string message)
	{
		var func = () => new NumberValidator(precision, scale, onlyPositive);
		
		func.Should().NotThrow(message);
	}

    [TestCase(null, 17, 2, true)]
    [TestCase("", 17, 2, true)]
    [TestCase("  \n\t  ", 17, 2, true)]
	public void IsValidNumber_ShouldBe_False_When_NullOrEmpty(
	    string number,
	    int precision,
	    int scale,
	    bool onlyPositive)
		=> CheckNumberValidation(number, precision, scale, onlyPositive, false);

	[TestCase("asd", 17, 2, true)]
    [TestCase(" 0", 17, 2, true)]
    [TestCase("0 ", 17, 2, true)]
    [TestCase(".0", 17, 2, true)]
    [TestCase("0.", 17, 2, true)]
    [TestCase("a23", 17, 2, true)]
    [TestCase("1e3", 17, 2, true)]
    [TestCase("1.a23", 17, 2, true)]
	public void IsValidNumber_ShouldBe_False_When_DoesNotMatchFormat(
	    string number,
	    int precision,
	    int scale,
	    bool onlyPositive)
		=> CheckNumberValidation(number, precision, scale, onlyPositive, false);

	[TestCase("00.00", 3, 2, true)]
    [TestCase("-0.00", 3, 2, false)]
	[TestCase("+123", 3, 2, true)]
	[TestCase("1234", 3, 2, true)]
	public void IsValidNumber_ShouldBe_False_When_DigitsPlusSignGreaterPrecision(
	    string number,
	    int precision,
	    int scale,
	    bool onlyPositive)
		=> CheckNumberValidation(number, precision, scale, onlyPositive, false);

	[TestCase("00.00", 4, 2, true)]
	[TestCase("-0.00", 4, 2, false)]
	[TestCase("+123", 4, 2, true)]
	[TestCase("1234", 4, 2, true)]
	public void IsValidNumber_ShouldBe_True_When_DigitsPlusSignLessOrEqualPrecision(
		string number,
		int precision,
		int scale,
		bool onlyPositive)
		=> CheckNumberValidation(number, precision, scale, onlyPositive, true);

	[TestCase("0.000", 17, 2, true)]
    public void IsValidNumber_ShouldBe_False_When_FractionGreaterScale(
	    string number,
	    int precision,
	    int scale,
	    bool onlyPositive)
		=> CheckNumberValidation(number, precision, scale, onlyPositive, false);

	[TestCase("0", 17, 2, true)]
	[TestCase("0.0", 17, 2, true)]
	[TestCase("0.00", 17, 2, true)]
	public void IsValidNumber_ShouldBe_True_When_FractionLessOrEqualScale(
		string number,
		int precision,
		int scale,
		bool onlyPositive)
		=> CheckNumberValidation(number, precision, scale, onlyPositive, true);

	[TestCase("-0.00", 4, 2, true)]
	[TestCase("-123", 4, 2, true)]
	public void IsValidNumber_ShouldBe_False_When_MinusAndOnlyPositive(
		string number,
		int precision,
		int scale,
		bool onlyPositive)
		=> CheckNumberValidation(number, precision, scale, onlyPositive, false);

	private void CheckNumberValidation(
		string number,
		int precision,
		int scale,
		bool onlyPositive,
		bool isValid)
    {
	    new NumberValidator(precision, scale, onlyPositive)
		    .IsValidNumber(number)
		    .Should()
		    .Be(isValid);
	}
}