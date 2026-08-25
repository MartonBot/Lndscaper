using Lndscaper;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.File(Path.Combine(app.Environment.ContentRootPath, "index.html"), "text/html"));

app.MapPost("/parse", async (IFormFile? file) =>
{
        if (file is null || file.Length == 0)
        {
                return Results.BadRequest(new { error = "Choose an LND file first." });
        }

        var temporaryFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.lnd");
        try
        {
                await using (var output = File.Create(temporaryFile))
                {
                        await file.CopyToAsync(output);
                }

                var lnd = new Lnd();
                lnd.Read(temporaryFile);

                return Results.Ok(new
                {
                        header = lnd.Header,
                        blocks = lnd.Blocks.Select(block => new
                        {
                                index = block.BlockData.Index,
                                blockX = block.BlockData.BlockX,
                                blockY = block.BlockData.BlockY,
                                mapX = block.BlockData.MapX,
                                mapY = block.BlockData.MapY,
                                cells = block.Cells.Select(cell => new
                                {
                                        color = cell.Color,
                                        altitude = cell.Altitude,
                                        country = cell.Country,
                                        hasWater = cell.HasWater,
                                        coastline = cell.Coastline,
                                        fullWater = cell.FullWater,
                                        split = cell.Split
                                })
                        }),
                        loResTextures = lnd.LoResTextures.Select(texture => new
                        {
                                id = texture.ID,
                                material = texture.Material,
                                numSubTextures = texture.NumSubTextures,
                                size = texture.Size
                        })
                });
        }
        catch (Exception exception) when (exception is IOException or InvalidDataException or EndOfStreamException)
        {
                return Results.BadRequest(new { error = $"Could not parse the file: {exception.Message}" });
        }
        finally
        {
                File.Delete(temporaryFile);
        }
}).DisableAntiforgery();

app.Run();
