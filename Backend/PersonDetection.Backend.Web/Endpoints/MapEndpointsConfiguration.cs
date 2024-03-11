namespace PersonDetection.Backend.Web.Endpoints;

public static class MapEndpointsConfiguration
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapGet("user", IdentifyEndpoint.Handler);
        app.MapPost("login", LoginEndpoint.HandlerAsync);
        app.MapPost("logout", LogoutEndpoint.Handler).RequireAuthorization();
        app.MapPost("register", RegisterEndpoint.HandlerAsync);
        
        app.MapPost("photo", PhotoEndpoint.HandlerAsync);
        
        app.MapPost("gallery", GalleryEndpoints.SaveToGalleryHandlerAsync).RequireAuthorization();
        app.MapGet("gallery", GalleryEndpoints.GetPhotosHandlerAsync).RequireAuthorization();
        app.MapDelete("gallery", GalleryEndpoints.DeletePhotoHandlerAsync).RequireAuthorization();
        
        return app;
    }
}