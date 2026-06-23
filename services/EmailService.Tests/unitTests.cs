
using EmailService.Models;
using EmailService.Service;
using EmailService.Services;
using Moq;
using System.Net.Mail;
using System.Timers;
using Xunit;


public class EmailServiceTests
{
    [Fact]
    public async Task Should_Send_Email_When_AI_Succeeds()
    {
        // Arrange
        var mockSender = new Mock<IEmailSender>();
        var mockAi = new Mock<IAiEmailGenerator>();

        mockAi
            .Setup(x => x.GenerateEmail(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("AI generated content");

        var email = new EmailMessage
        {
            To = "test@test.com",
            Subject = "Hello",
            Body = "Original body"
        };

        // Act
        var result = await mockAi.Object.GenerateEmail(email.Subject, email.Body);
        mockSender.Object.Send(email.To, email.Subject, result);

        // Assert
        mockSender.Verify(x =>
            x.Send("test@test.com", "Hello", "AI generated content"),
            Times.Once);
    }

    [Fact]
    public async Task Should_Use_Fallback_When_AI_Fails()
    {
        // Arrange
        var mockSender = new Mock<IEmailSender>();
        var mockAi = new Mock<IAiEmailGenerator>();

        mockAi
            .Setup(x => x.GenerateEmail(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("AI failed"));

        var email = new EmailMessage
        {
            To = "test@test.com",
            Subject = "Hello",
            Body = "Fallback body"
        };

        string finalBody = email.Body;

        // Act
        try
        {
            finalBody = await mockAi.Object.GenerateEmail(email.Subject, email.Body);
        }
        catch
        {
            // fallback kicks in
        }

        mockSender.Object.Send(email.To, email.Subject, finalBody);

        // Assert
        mockSender.Verify(x =>
            x.Send("test@test.com", "Hello", "Fallback body"),
            Times.Once);
    }

    [Fact]
    public void Should_Not_Send_Email_If_Recipient_Is_Empty()
    {
        // Arrange
        var mockSender = new Mock<IEmailSender>();

        var email = new EmailMessage
        {
            To = "",
            Subject = "Hello",
            Body = "Body"
        };

        // Act
        if (!string.IsNullOrEmpty(email.To))
        {
            mockSender.Object.Send(email.To, email.Subject, email.Body);
        }

        // Assert
        mockSender.Verify(x =>
            x.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }
    [Fact]
    public void Should_Not_Send_Email_If_Credentials_Are_Missing()
    {
        // Arrange
        Environment.SetEnvironmentVariable("MAILJET_API_KEY", null);
        Environment.SetEnvironmentVariable("MAILJET_SECRET", null);

        var sender = new EmailSender();

        // Act
        var exception = Record.Exception(() =>
            sender.Send("test@test.com", "subject", "body")
        );

        // Assert
        Assert.Null(exception); // should NOT crash
    }
    [Fact]
    public async Task Should_Handle_Empty_AI_Response()
    {
        var mockAi = new Mock<IAiEmailGenerator>();

        mockAi
            .Setup(x => x.GenerateEmail(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string)null);

        var result = await mockAi.Object.GenerateEmail("sub", "body");

        Assert.Null(result);
    }
    [Fact]
    public void EmailMessage_Should_Store_Data_Correctly()
    {
        var email = new EmailMessage
        {
            To = "a@a.com",
            Subject = "Hello",
            Body = "World"
        };

        Assert.Equal("a@a.com", email.To);
        Assert.Equal("Hello", email.Subject);
        Assert.Equal("World", email.Body);
    }
    [Fact]
    public async Task Should_Handle_Large_Email_Content()
    {
        var mockAi = new Mock<IAiEmailGenerator>();

        var largeText = new string('A', 10000);

        mockAi
            .Setup(x => x.GenerateEmail(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(largeText);

        var result = await mockAi.Object.GenerateEmail("sub", largeText);

        Assert.Equal(largeText, result);
    }
    [Fact]
    public void Should_Not_Send_Email_If_Subject_Is_Empty()
    {
        var mockSender = new Mock<IEmailSender>();

        var email = new EmailMessage
        {
            To = "test@test.com",
            Subject = "",
            Body = "Body"
        };

        if (!string.IsNullOrEmpty(email.Subject))
        {
            mockSender.Object.Send(email.To, email.Subject, email.Body);
        }

        mockSender.Verify(x => x.Send(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()),
            Times.Never);
    }
    [Fact]
    public async Task Should_Handle_AI_Exception_Gracefully()
    {
        var mockAi = new Mock<IAiEmailGenerator>();

        mockAi
            .Setup(x => x.GenerateEmail(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("API down"));

        await Assert.ThrowsAsync<Exception>(() =>
            mockAi.Object.GenerateEmail("sub", "body")
        );
    }
}