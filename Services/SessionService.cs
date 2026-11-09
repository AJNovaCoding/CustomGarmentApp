using System;
using System.Collections.Generic;
using System.Text;


namespace CustomGarmentApp.Services;

public static class SessionService
{
    // Stores the currently authenticated user's information.
    public static string? CurrentUserEmail { get; private set; }

    public static bool IsAdmin { get; private set; }

    // Indicates whether someone is signed in.
    public static bool IsAuthenticated =>
        !string.IsNullOrWhiteSpace(CurrentUserEmail);

    // Called only after successful password verification.
    public static void StartSession(string email, bool isAdmin)
    {
        CurrentUserEmail = email.Trim().ToLowerInvariant();
        IsAdmin = isAdmin;
    }

    // Removes the authenticated user's session information.
    public static void EndSession()
    {
        CurrentUserEmail = null;
        IsAdmin = false;
    }
}

