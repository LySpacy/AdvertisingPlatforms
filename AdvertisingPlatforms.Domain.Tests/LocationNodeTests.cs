using AdvertisingPlatforms.Domain.Models;


namespace AdvertisingPlatforms.Domain.Tests
{
    /// <summary>
    /// Юнит-тесты для модели LocationNode
    /// </summary>
    public class LocationNodeTests
    {
        /// <summary>
        /// Проверяет, что все свойства корректно инициализируются по умолчанию
        /// </summary>
        [Fact]
        public void LocationNode_ShouldInitializeProperties()
        {
            var node = new LocationNode();

            Assert.NotNull(node.Children);
            Assert.Empty(node.Children);

            Assert.NotNull(node.Platforms);
            Assert.Empty(node.Platforms);

            Assert.Equal(string.Empty, node.Name);
        }

        /// <summary>
        /// Проверяет возможность добавления дочернего узла
        /// </summary>
        [Fact]
        public void LocationNode_CanAddChildNode()
        {
            var root = new LocationNode { Name = "root" };
            var child = new LocationNode { Name = "child" };

            root.Children.Add(child.Name, child);

            Assert.Single(root.Children);
            Assert.True(root.Children.ContainsKey("child"));
            Assert.Equal(child, root.Children["child"]);
        }

        /// <summary>
        /// Проверяет возможность добавления платформы к узлу
        /// </summary>
        [Fact]
        public void LocationNode_CanAddPlatform()
        {
            var node = new LocationNode { Name = "node" };

            node.Platforms.Add("Yandex");

            Assert.Single(node.Platforms);
            Assert.Contains("Yandex", node.Platforms);
        }
    }
}
