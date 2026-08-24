using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Lndscaper;

const string Html = """
<!doctype html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>LNDscaper</title>
    <style>
        :root { font-family: Georgia, serif; color: #19302b; background: #edf2e8; }
        body { margin: 0; min-height: 100vh; display: grid; place-items: center; }
        main { width: min(680px, calc(100% - 40px)); padding: 48px 0; }
        h1 { font-size: clamp(2.8rem, 8vw, 5.4rem); line-height: .9; letter-spacing: -.04em; margin: 0 0 18px; }
        p { font-size: 1.15rem; line-height: 1.5; max-width: 540px; }
        form { margin-top: 34px; padding: 28px; border: 1px solid #a8b9a2; background: #f8faf3; box-shadow: 10px 10px 0 #cbd8c1; }
        label { display: block; font-size: .9rem; font-weight: bold; letter-spacing: .08em; text-transform: uppercase; margin-bottom: 10px; }
        input[type=file] { width: 100%; box-sizing: border-box; padding: 14px; border: 1px dashed #6d8a72; background: white; font: inherit; }
        button { margin-top: 18px; padding: 13px 22px; border: 0; background: #d7603f; color: white; font: bold 1rem Georgia, serif; cursor: pointer; }
        button:disabled { opacity: .55; cursor: wait; }
        #status { margin-top: 26px; white-space: pre-wrap; }
        dl { display: grid; grid-template-columns: 1fr 1fr; gap: 1px; margin-top: 18px; background: #a8b9a2; }
        dt, dd { margin: 0; padding: 12px; background: #f8faf3; } dt { font-weight: bold; } dd { text-align: right; }
    </style>
</head>
<body>
    <main>
        <h1>LND<br>Scaper</h1>
        <p>Inspect a LND file</p>
        <form id="parser-form">
            <label for="file">Land file</label>
            <input id="file" name="file" type="file" accept=".lnd" required>
            <button type="submit">Read LND file</button>
        </form>
        <section id="status" aria-live="polite"></section>
    </main>
    <script>
        const form = document.querySelector('#parser-form');
        const file = document.querySelector('#file');
        const button = form.querySelector('button');
        const status = document.querySelector('#status');
        form.addEventListener('submit', async (event) => {
            event.preventDefault();
            if (!file.files.length) return;
            button.disabled = true;
            status.textContent = 'Parsing...';
            try {
                const response = await fetch('/parse', { method: 'POST', body: new FormData(form) });
                const data = await response.statusText;
                if (!response.ok) throw new Error(data.error);
                status.innerHTML = `<p>File parsed successfully.</p>`;
            } catch (error) { status.textContent = error.message; }
            finally { button.disabled = false; }
        });
    </script>
</body>
</html>
""";

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.Content(Html, "text/html"));

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

                return Results.Ok();
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
