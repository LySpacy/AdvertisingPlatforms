using AdvertisingPlatforms.API.Controllers;
using AdvertisingPlatforms.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AdvertisingPlatforms.API.Tests
{
    /// <summary>
    /// Unit-тесты для контроллера AdPlatformController
    /// </summary>
    public class AdPlatformControllerTests
    {
        private readonly Mock<IAdPlatformService> _serviceMock;
        private readonly AdPlatformController _controller;

        public AdPlatformControllerTests()
        {
            // Создаем мок сервиса
            _serviceMock = new Mock<IAdPlatformService>();
            _controller = new AdPlatformController(_serviceMock.Object);
        }

        /// <summary>
        /// Тестирует метод Load, когда файл null → возвращает BadRequest
        /// </summary>
        [Fact]
        public async Task Load_NullFile_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.Load(null!, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Файл пуст", badRequestResult.Value);
        }

        /// <summary>
        /// Тестирует метод Load, когда файл пустой → возвращает BadRequest
        /// </summary>
        [Fact]
        public async Task Load_EmptyFile_ReturnsBadRequest()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(0);

            // Act
            var result = await _controller.Load(fileMock.Object, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Файл пуст", badRequestResult.Value);
        }

        /// <summary>
        /// Тестирует метод Load с корректным файлом → возвращает Ok и вызывает LoadAsync сервиса
        /// </summary>
        [Fact]
        public async Task Load_ValidFile_ReturnsOk()
        {
            // Arrange
            var content = "Yandex:/ru";
            var fileName = "test.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.Length).Returns(ms.Length);
            fileMock.Setup(f => f.FileName).Returns(fileName);

            // Act
            var result = await _controller.Load(fileMock.Object, CancellationToken.None);

            // Assert
            _serviceMock.Verify(s => s.LoadAsync(fileMock.Object, It.IsAny<CancellationToken>()), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Файл загружен", okResult.Value);
        }

        /// <summary>
        /// Тестирует метод Search с корректной локацией → возвращает Ok с результатом
        /// </summary>
        [Fact]
        public void Search_ValidLocation_ReturnsOk()
        {
            // Arrange
            var location = "/ru/msk";
            var expectedPlatforms = new List<string> { "Yandex", "Gazeta" };
            _serviceMock.Setup(s => s.Search(location)).Returns(expectedPlatforms);

            // Act
            var result = _controller.Search(location);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.Equal(expectedPlatforms, value);
        }

        /// <summary>
        /// Тестирует метод Search с пустой локацией → возвращает Ok с пустым списком
        /// </summary>
        [Fact]
        public void Search_EmptyLocation_ReturnsOkWithEmptyList()
        {
            // Arrange
            var location = string.Empty;
            _serviceMock.Setup(s => s.Search(location)).Returns(new List<string>());

            // Act
            var result = _controller.Search(location);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.Empty(value);
        }
    }
}