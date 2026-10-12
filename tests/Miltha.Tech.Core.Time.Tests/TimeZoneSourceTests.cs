using Microsoft.AspNetCore.Http;
using Miltha.Tech.Core.Time;

namespace Miltha.Tech.Core.Time.Tests
{
    public class TimeZoneSourceTests
    {
        private static readonly TimeZoneOptions Options = new() { DefaultZoneId = "Etc/UTC", HeaderName = "X-Timezone", ClaimType = "tz" };

        private static HttpContext Context(Action<HttpContext>? configure = null)
        {
            var context = new DefaultHttpContext();
            configure?.Invoke(context);
            return context;
        }

        [Fact]
        public void HeaderSource_ShouldResolveIanaZone()
        {
            // Arrange
            var source = new HeaderTimeZoneSource(Microsoft.Extensions.Options.Options.Create(Options));
            var context = Context(c => c.Request.Headers["X-Timezone"] = "America/Sao_Paulo");

            // Act
            var zone = source.Resolve(context);

            // Assert
            Assert.NotNull(zone);
            Assert.Equal("America/Sao_Paulo", zone!.Id);
        }

        [Fact]
        public void HeaderSource_ShouldReturnNull_WhenHeaderMissingOrInvalid()
        {
            // Arrange
            var source = new HeaderTimeZoneSource(Microsoft.Extensions.Options.Options.Create(Options));

            // Act
            var missing = source.Resolve(Context());
            var invalid = source.Resolve(Context(c => c.Request.Headers["X-Timezone"] = "Invalid/Zone"));

            // Assert
            Assert.Null(missing);
            Assert.Null(invalid);
        }

        [Fact]
        public void ClaimSource_ShouldResolveFromPrincipal()
        {
            // Arrange
            var source = new ClaimTimeZoneSource(Microsoft.Extensions.Options.Options.Create(Options));
            var context = Context(c => c.User = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity([new System.Security.Claims.Claim("tz", "Europe/Lisbon")], "Test")));

            // Act
            var zone = source.Resolve(context);

            // Assert
            Assert.NotNull(zone);
            Assert.Equal("Europe/Lisbon", zone!.Id);
        }

        [Fact]
        public void IpSource_ShouldResolveViaResolver()
        {
            // Arrange
            var resolver = new FakeGeoResolver("America/New_York");
            var source = new IpGeoTimeZoneSource(resolver);
            var context = Context(c => c.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("8.8.8.8"));

            // Act
            var zone = source.Resolve(context);

            // Assert
            Assert.NotNull(zone);
            Assert.Equal("America/New_York", zone!.Id);
            Assert.Equal("8.8.8.8", resolver.LastIp);
        }

        [Fact]
        public void Provider_ShouldResolveByPriority_HeaderOverClaimOverDefault()
        {
            // Arrange
            var provider = new TimeZoneProvider(
                new HttpContextAccessorStub(Context(c =>
                {
                    c.Request.Headers["X-Timezone"] = "Asia/Tokyo";
                    c.User = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity([new System.Security.Claims.Claim("tz", "Europe/Lisbon")], "Test"));
                })),
                [new HeaderTimeZoneSource(Microsoft.Extensions.Options.Options.Create(Options)), new ClaimTimeZoneSource(Microsoft.Extensions.Options.Options.Create(Options))],
                Microsoft.Extensions.Options.Options.Create(Options));

            // Act
            var zone = provider.GetTimeZone();

            // Assert
            Assert.Equal("Asia/Tokyo", zone.Id);
        }

        [Fact]
        public void Provider_ShouldFallbackToDefault_WhenNothingResolves()
        {
            // Arrange
            var provider = new TimeZoneProvider(
                new HttpContextAccessorStub(Context()),
                [new HeaderTimeZoneSource(Microsoft.Extensions.Options.Options.Create(Options))],
                Microsoft.Extensions.Options.Options.Create(Options));

            // Act
            var zone = provider.GetTimeZone();

            // Assert
            Assert.Equal("Etc/UTC", zone.Id);
        }

        private sealed class FakeGeoResolver : IGeoTimeZoneResolver
        {
            public FakeGeoResolver(string zoneId)
            {
                ZoneId = zoneId;
            }

            public string ZoneId { get; }

            public string? LastIp { get; private set; }

            public TimeZoneInfo? Resolve(string ip)
            {
                LastIp = ip;
                return TimeZoneInfo.FindSystemTimeZoneById(ZoneId);
            }
        }

        private sealed class HttpContextAccessorStub : IHttpContextAccessor
        {
            public HttpContextAccessorStub(HttpContext httpContext)
            {
                HttpContext = httpContext;
            }

            public HttpContext? HttpContext { get; set; }
        }
    }
}