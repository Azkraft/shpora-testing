using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
	private const int MaxGenDepth = 1000;

    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null!));

        var currentActual = actualTsar;
        var currentExpected = expectedTsar;
        for (var i = 0; i > -MaxGenDepth; i--)
        {
	        if (currentExpected is null && currentActual is null)
		        return;

            CheckTsarFields(currentActual, currentExpected, i);

            currentActual = currentActual?.Parent;
            currentExpected = currentExpected?.Parent;
        }

		Assert.Fail("The verification depth has reached its maximum value.");
	}

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null!));

        // Какие недостатки у такого подхода? 

        /*
         * Мы не получаем полезной информации при падении теста.
         * Было бы полезно знать в каком поле наблюдается различие.
         * Также мы не знаем в каком поколении находится это различие.
         * Кроме того, реализация AreEqual рекурсивная
         * и при этом не защищена от замыкания (зацикливания) цепочки родословной.
         * Несколько лучшим решением было бы реализовать класс с интерфейсом IEqualityComparer<Person>.
         */

        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

    private bool AreEqual(Person? actual, Person? expected) 
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }

    private void CheckTsarFields(Person? actual, Person? expected, int genNumber)
    {
	    var message = $"the generation {genNumber}";

	    if (expected is null)
	    {
		    actual.Should().BeNull(message);
		    return;
	    }

		actual.Should().NotBeNull(message);
	    expected.Should().NotBeNull(message);
	    actual!.Name.Should().Be(expected!.Name, message);
		actual!.Age.Should().Be(expected!.Age, message);
		actual!.Height.Should().Be(expected!.Height, message);
		actual!.Weight.Should().Be(expected!.Weight, message);
    }
}
