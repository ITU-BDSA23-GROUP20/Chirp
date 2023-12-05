﻿namespace Chirp.Web.Models;

using Chirp.Core.Entities;

public class UserModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    // Add more properties if needed

    // Constructor to initialize properties based on an Author entity
    public UserModel(Author author)
    {
        Username = author.UserName;
        Email = author.Email;
        // Map additional properties if needed
    }
}

