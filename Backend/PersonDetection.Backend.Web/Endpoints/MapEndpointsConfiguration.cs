namespace PersonDetection.Backend.Web.Endpoints;

public static class MapEndpointsConfiguration
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapIdentityEndpoints();
        app.MapGalleryEndpoints();

        // This endpoints is used in mac maui application
        app.MapPost("photo", PhotoEndpoint.HandlerAsync);

        // For kubernetes
        app.MapGet("health-check", () => Results.Ok());

        return app;
    }

    private static WebApplication MapIdentityEndpoints(this WebApplication app)
    {
        app.MapGet("user", IdentityEndpoints.IdentifyHandlerAsync);

        app.MapPost("login", IdentityEndpoints.LoginHandlerAsync);

        app.MapPost("register", IdentityEndpoints.RegisterHandlerAsync);

        app.MapPost("logout", IdentityEndpoints.LogoutHandlerAsync)
            .RequireAuthorization();

        return app;
    }

    private static WebApplication MapGalleryEndpoints(this WebApplication app)
    {
        app.MapPost("gallery", GalleryEndpoints.SaveToGalleryHandlerAsync)
            .RequireAuthorization();

        app.MapGet("gallery", GalleryEndpoints.GetPhotosHandlerAsync)
            .RequireAuthorization();

        app.MapDelete("gallery", GalleryEndpoints.DeletePhotoHandlerAsync)
            .RequireAuthorization();

        return app;
    }
}