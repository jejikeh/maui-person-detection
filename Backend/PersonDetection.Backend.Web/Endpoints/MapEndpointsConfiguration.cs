namespace PersonDetection.Backend.Web.Endpoints;

public static class MapEndpointsConfiguration
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapGet("user", IdentityEndpoints.IdentifyHandlerAsync);
        app.MapPost("login", IdentityEndpoints.LoginHandlerAsync);
        app.MapPost("logout", IdentityEndpoints.LogoutHandlerAsync).RequireAuthorization();
        app.MapPost("register", IdentityEndpoints.RegisterHandlerAsync);
        
        app.MapPost("photo", PhotoEndpoint.HandlerAsync);
        
        app.MapPost("gallery", GalleryEndpoints.SaveToGalleryHandlerAsync).RequireAuthorization();
        app.MapGet("gallery", GalleryEndpoints.GetPhotosHandlerAsync).RequireAuthorization();
        app.MapDelete("gallery", GalleryEndpoints.DeletePhotoHandlerAsync).RequireAuthorization();
        
        return app;
    }
}