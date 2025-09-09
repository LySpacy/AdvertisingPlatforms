using System.Text;
using AdvertisingPlatforms.Domain.Interfaces;
using AdvertisingPlatforms.Service.Common.Exceptions;
using AdvertisingPlatforms.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;

namespace AdvertisingPlatforms.Service.Tests;

public class AdPlatformServiceTests
{
    /// <summary>
    /// Тест проверяет, что метод LoadAsync корректно читает строки из файла
    /// и вызывает репозиторий с правильным набором строк.
    /// </summary>
    [Fact]
    public async Task LoadAsync_ValidFile_CallsRepositoryLoad()
    {
        // Arrange: создаем мок репозитория и сервис
        var mockRepo = new Mock<IAdPlatformRepository>();
        var service = new AdPlatformService(mockRepo.Object);

        // Создаем "файл" с двумя строками
        var content = "Yandex:/ru\nLocalPaper:/ru/svrd/revda";
        var file = CreateFormFile(content, "file.txt");

        // Act: загружаем файл
        await service.LoadAsync(file);

        // Assert: проверяем, что репозиторий получил именно эти строки
        mockRepo.Verify(r => r.LoadFromLines(It.Is<List<string>>(l => 
            l.Count == 2 &&
            l[0] == "Yandex:/ru" &&
            l[1] == "LocalPaper:/ru/svrd/revda"
        )), Times.Once);
    }

    /// <summary>
    /// Тест проверяет работу LoadAsync при пустом файле: репозиторий должен получить пустой список.
    /// </summary>
    [Fact]
    public async Task LoadAsync_EmptyFile_CallsRepositoryWithEmptyList()
    {
        // Arrange
        var mockRepo = new Mock<IAdPlatformRepository>();
        var service = new AdPlatformService(mockRepo.Object);

        var file = CreateFormFile("", "empty.txt");

        // Act
        await service.LoadAsync(file);

        // Assert: репозиторий вызван с пустым списком строк
        mockRepo.Verify(r => r.LoadFromLines(It.Is<List<string>>(l => l.Count == 0)), Times.Once);
    }

    /// <summary>
    /// Тест проверяет работу метода Search: поиск платформ должен делегироваться репозиторию
    /// и возвращать корректный список платформ.
    /// </summary>
    [Fact]
    public void Search_CallsRepositoryFindPlatforms()
    {
        // Arrange: настроим мок, чтобы репозиторий возвращал одну платформу
        var mockRepo = new Mock<IAdPlatformRepository>();
        mockRepo.Setup(r => r.FindPlatforms("/ru")).Returns(new List<string> { "Yandex" });

        var service = new AdPlatformService(mockRepo.Object);

        // Act: вызываем поиск
        var result = service.Search("/ru");

        // Assert: результат содержит одну платформу "Yandex", репозиторий вызван один раз
        Assert.Single(result);
        Assert.Contains("Yandex", result);
        mockRepo.Verify(r => r.FindPlatforms("/ru"), Times.Once);
    }

    /// <summary>
    /// Тест проверяет, что если репозиторий выбрасывает исключение при загрузке файла,
    /// метод LoadAsync пробрасывает его дальше.
    /// </summary>
    [Fact]
    public async Task LoadAsync_InvalidFile_ShouldThrowExceptionFromRepository()
    {
        // Arrange: настраиваем репозиторий, чтобы он выбрасывал InvalidAdPlatformFileException
        var mockRepo = new Mock<IAdPlatformRepository>();
        mockRepo.Setup(r => r.LoadFromLines(It.IsAny<IEnumerable<string>>()))
                .Throws(new InvalidAdPlatformFileException("Invalid file"));

        var service = new AdPlatformService(mockRepo.Object);

        var file = CreateFormFile("BadLine", "bad.txt");

        // Act & Assert: метод должен пробросить исключение
        var ex = await Assert.ThrowsAsync<InvalidAdPlatformFileException>(() => service.LoadAsync(file));
        Assert.Equal("Invalid file", ex.Message);
    }

    #region Вспомогательные методы

    /// <summary>
    /// Вспомогательный метод для создания IFormFile из строки.
    /// Позволяет имитировать загружаемый файл без реальной файловой системы.
    /// </summary>
    private static IFormFile CreateFormFile(string content, string fileName)
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        return new FormFile(stream, 0, stream.Length, "file", fileName);
    }

    #endregion
}