using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
	[TestCase(-1, 0, false)]
	public void Constructor_ShouldThrow_ArgumentException_When_NegativeTotalNumberOfDigits(
		int precision,
		int scale,
		bool onlyPositive)
		=> CheckConstructorThrowArgumentException(precision, scale, onlyPositive, true);

	[TestCase(0, 0, false)]
	public void Constructor_ShouldThrow_ArgumentException_When_ZeroTotalNumberOfDigits(
		int precision,
		int scale,
		bool onlyPositive)
		=> CheckConstructorThrowArgumentException(precision, scale, onlyPositive, true);

	[TestCase(1, -1, false)]
	public void Constructor_ShouldThrow_ArgumentException_When_NegativeNumberOfDigitsInFractionalPart(
		int precision,
		int scale,
		bool onlyPositive)
		=> CheckConstructorThrowArgumentException(precision, scale, onlyPositive, true);

	[TestCase(1, 1, false)]
	public void Constructor_ShouldThrow_ArgumentException_When_NumberOfDigitsInFractionalPartEqualTotalNumberOfDigits(
		int precision,
		int scale,
		bool onlyPositive)
		=> CheckConstructorThrowArgumentException(precision, scale, onlyPositive, true);

	[TestCase(1, 2, false)]
	public void Constructor_ShouldThrow_ArgumentException_When_NumberOfDigitsInFractionalPartGreaterTotalNumberOfDigits(
		int precision,
		int scale,
		bool onlyPositive)
		=> CheckConstructorThrowArgumentException(precision, scale, onlyPositive, true);

	[TestCase(1, 0, false)]
	public void Constructor_ShouldNotThrow_Exception_When_PositivePrecisionNonNegativeScaleAndScaleLessPrecision(
	    int precision,
	    int scale,
	    bool onlyPositive)
		=> CheckConstructorThrowArgumentException(precision, scale, onlyPositive, false);

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
    [TestCase("1.a23", 17, 5, true)]
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

	private void CheckConstructorThrowArgumentException(
		int precision,
		int scale,
		bool onlyPositive,
		bool shouldThrow)
	{
		var func = () => new NumberValidator(precision, scale, onlyPositive);

		if (shouldThrow)
			func.Should().Throw<ArgumentException>();
		else
			func.Should().NotThrow();
	}
}