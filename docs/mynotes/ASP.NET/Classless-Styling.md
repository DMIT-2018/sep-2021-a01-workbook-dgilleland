# Classless Styling

> TODO: Steps to remove the Bootstrap/JQuery files

## Cleaned Razor Pages (.cshtml)

### _Layout.cshtml

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - WebApp</title>
    <link rel="stylesheet" href="~/css/site.css" />
</head>
<body>
    <header>
        <nav>
            <ul>
                <li><a asp-area="" asp-page="/Index">WebApp</a></li>
                <li><a asp-area="" asp-page="/Index">Home</a></li>
                <li><a asp-area="" asp-page="/Privacy">Privacy</a></li>
                <partial name="_LoginPartial" />
            </ul>
        </nav>
    </header>
    <main role="main">
        @RenderBody()
    </main>

    <footer>
        <div>
            &copy; 2021 - WebApp - <a asp-area="" asp-page="/Privacy">Privacy</a>
        </div>
    </footer>

    <script src="~/js/site.js" asp-append-version="true"></script>

    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

### _LoginPartial.cshtml

```html
@using Microsoft.AspNetCore.Identity
@inject SignInManager<IdentityUser> SignInManager
@inject UserManager<IdentityUser> UserManager

@if (SignInManager.IsSignedIn(User))
{
    <li>
        <a asp-area="Identity" asp-page="/Account/Manage/Index" title="Manage">Hello @User.Identity.Name!</a>
    </li>
    <li>
        <form asp-area="Identity" asp-page="/Account/Logout" asp-route-returnUrl="@Url.Page("/", new { area = "" })" method="post" >
            <button  type="submit">Logout</button>
        </form>
    </li>
}
else
{
    <li>
        <a asp-area="Identity" asp-page="/Account/Register">Register</a>
    </li>
    <li>
        <a asp-area="Identity" asp-page="/Account/Login">Login</a>
    </li>
}
```

### Error.cshtml

```html
@page
@model ErrorModel
@{
    ViewData["Title"] = "Error";
}

<h1>Error.</h1>
<h2>An error occurred while processing your request.</h2>

@if (Model.ShowRequestId)
{
    <p>
        <strong>Request ID:</strong> <code>@Model.RequestId</code>
    </p>
}

<h3>Development Mode</h3>
<p>
    Swapping to the <strong>Development</strong> environment displays detailed information about the error that occurred.
</p>
<p>
    <strong>The Development environment shouldn't be enabled for deployed applications.</strong>
    It can result in displaying sensitive information from exceptions to end users.
    For local debugging, enable the <strong>Development</strong> environment by setting the <strong>ASPNETCORE_ENVIRONMENT</strong> environment variable to <strong>Development</strong>
    and restarting the app.
</p>
```

### Index.cshtml

```html
@page
@model IndexModel
@{
    ViewData["Title"] = "Home page";
}

<h1>Welcome</h1>
<p>Learn about <a href="https://docs.microsoft.com/aspnet/core">building Web apps with ASP.NET Core</a>.</p>
```
