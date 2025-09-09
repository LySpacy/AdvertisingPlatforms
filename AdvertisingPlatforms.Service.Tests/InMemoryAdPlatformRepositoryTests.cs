using AdvertisingPlatforms.Service.Common.Exceptions;

namespace AdvertisingPlatforms.Service.Tests;

public class InMemoryAdPlatformRepositoryTests
{
    /// <summary>
    /// Проверяем, что корректные данные из строк загружаются в репозиторий,
    /// и метод FindPlatforms возвращает правильные платформы для разных локаций.
    /// </summary>
    [Fact]
    public void LoadFromLines_CorrectData_ShouldStorePlatforms()
    {
        // Arrange
        var repo = new InMemoryAdPlatformRepository();
        var lines = new List<string>
        {
            "Yandex:/ru",
            "LocalPaper:/ru/svrd/revda,/ru/svrd/pervik",
            "Gazeta:/ru/msk,/ru/permobl,/ru/chelobl",
            "CoolAds:/ru/svrd"
        };

        // Act
        repo.LoadFromLines(lines);

        // Assert: проверяем платформы для конкретных локаций
        var result1 = repo.FindPlatforms("/ru/msk");
        Assert.Contains("Yandex", result1);
        Assert.Contains("Gazeta", result1);

        var result2 = repo.FindPlatforms("/ru/svrd");
        Assert.Contains("Yandex", result2);
        Assert.Contains("CoolAds", result2);

        var result3 = repo.FindPlatforms("/ru/svrd/revda");
        Assert.Contains("Yandex", result3);
        Assert.Contains("CoolAds", result3);
        Assert.Contains("LocalPaper", result3);

        var result4 = repo.FindPlatforms("/ru");
        Assert.Single(result4);
        Assert.Contains("Yandex", result4);
    }

    /// <summary>
    /// Проверяем, что при наличии некорректных строк метод LoadFromLines выбрасывает
    /// InvalidAdPlatformFileException.
    /// </summary>
    [Fact]
    public void LoadFromLines_InvalidLines_ShouldThrowException()
    {
        // Arrange
        var repo = new InMemoryAdPlatformRepository();
        var lines = new List<string>
        {
            "Yandex:/ru",
            "InvalidLineWithoutColon",
            "Another:/ru/svrd",
            "Bad:/wrongformat"
        };

        // Act & Assert: должно выбросить исключение
        var ex = Assert.Throws<InvalidAdPlatformFileException>(() => repo.LoadFromLines(lines));
        Assert.Contains("Invalid file", ex.Message);
    }

    /// <summary>
    /// Проверяем, что метод FindPlatforms возвращает пустой список
    /// при пустой, null или некорректной локации.
    /// </summary>
    [Fact]
    public void FindPlatforms_EmptyOrInvalidLocation_ShouldReturnEmptyList()
    {
        // Arrange
        var repo = new InMemoryAdPlatformRepository();
        var lines = new List<string> { "Yandex:/ru" };
        repo.LoadFromLines(lines);

        // Act
        var result1 = repo.FindPlatforms("");
        var result2 = repo.FindPlatforms(null!);
        var result3 = repo.FindPlatforms("invalid"); // не начинается с "/"

        // Assert
        Assert.Empty(result1);
        Assert.Empty(result2);
        Assert.Empty(result3);
    }

    /// <summary>
    /// Проверяем, что если заданная локация отсутствует в дереве, 
    /// возвращаются платформы ближайших родительских нод.
    /// </summary>
    [Fact]
    public void FindPlatforms_NonExistingLocation_ShouldReturnClosestParents()
    {
        // Arrange
        var repo = new InMemoryAdPlatformRepository();
        var lines = new List<string> { "Yandex:/ru" };
        repo.LoadFromLines(lines);

        // Act
        var result = repo.FindPlatforms("/ru/nowhere");

        // Assert: платформа Yandex действует на /ru и все вложенные локации
        Assert.Contains("Yandex", result);
    }

    /// <summary>
    /// Проверяем, что дублирующиеся платформы возвращаются уникальными (Distinct)
    /// </summary>
    [Fact]
    public void FindPlatforms_DuplicatePlatforms_ShouldReturnDistinct()
    {
        // Arrange
        var repo = new InMemoryAdPlatformRepository();
        var lines = new List<string>
        {
            "Yandex:/ru",
            "Yandex:/ru/svrd"
        };
        repo.LoadFromLines(lines);

        // Act
        var result = repo.FindPlatforms("/ru/svrd");

        // Assert: должно быть только одно вхождение платформы
        Assert.Single(result);
        Assert.Contains("Yandex", result);
    }
}