﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Chirp.Core.Entities;
using Microsoft.EntityFrameworkCore;

//Co-Authored by ChatGPT-3.5

namespace Chirp.CoreTests.Entities;

public class AuthorDTOTests
{
    [Fact]
    public void AuthorDTO_Name_ShouldHaveRequiredAttribute()
    {
        // Arrange and Act
        var propertyInfo = typeof(AuthorDTO).GetProperty("Name");
        var attribute = propertyInfo.GetCustomAttribute<RequiredAttribute>();
        
        // Assert
        Assert.NotNull(attribute);
    }
    
    [Fact]
    public void AuthorDTO_Name_ShouldHaveStringLengthAttributeWithMax50()
    {
        // Arrange and Act
        var propertyInfo = typeof(AuthorDTO).GetProperty("Name");
        var stringLengthAttribute = propertyInfo.GetCustomAttributes(typeof(StringLengthAttribute), true).FirstOrDefault() as StringLengthAttribute;
        
        // Assert
        Assert.NotNull(stringLengthAttribute);
        Assert.Equal(50, stringLengthAttribute.MaximumLength);
    }
    
    [Fact]
    public void AuthorDTO_Email_ShouldHaveRequiredAttribute()
    {
        // Arrange and Act
        var propertyInfo = typeof(AuthorDTO).GetProperty("Email");
        var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault() as RequiredAttribute;

        // Assert
        Assert.NotNull(requiredAttribute);
    }

    [Fact]
    public void AuthorDTO_Email_ShouldHaveStringLengthAttributeWithMax50()
    {
        // Arrange and Act
        var propertyInfo = typeof(AuthorDTO).GetProperty("Email");
        var stringLengthAttribute = propertyInfo.GetCustomAttributes(typeof(StringLengthAttribute), true).FirstOrDefault() as StringLengthAttribute;

        // Assert
        Assert.NotNull(stringLengthAttribute);
        Assert.Equal(50, stringLengthAttribute.MaximumLength);
    }

    [Fact]
    public void AuthorDTO_IndexAttribute_ShouldBeUnique()
    {
        // Arrange and Act
        var indexAttribute = typeof(AuthorDTO).GetCustomAttributes(typeof(IndexAttribute), true).FirstOrDefault() as IndexAttribute;

        // Assert
        Assert.NotNull(indexAttribute);
        Assert.True(indexAttribute.IsUnique);
    }

    [Fact]
    public void AuthorDTO_Cheeps_ShouldBeInitialized()
    {
        // Arrange and Act
        AuthorDTO author = new AuthorDTO
        { 
            AuthorId = Guid.NewGuid(), 
            Name = "TestAuthor", 
            Email = "mock@email.com" 
        };
        
        // Assert
        Assert.NotNull(author.Cheeps);
        Assert.Empty(author.Cheeps);
    }
}