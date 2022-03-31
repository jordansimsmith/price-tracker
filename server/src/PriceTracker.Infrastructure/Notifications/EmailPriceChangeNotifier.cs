using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PriceTracker.Core.Entities;
using PriceTracker.Core.Models;
using PriceTracker.Core.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PriceTracker.Infrastructure.Notifications;

public class EmailPriceChangeNotifier : IPriceChangeNotifier
{
    private readonly ILogger<EmailPriceChangeNotifier> _logger;
    private readonly SendGridConfiguration _sendGridConfiguration;
    private readonly IEnumerable<SubscriberModel> _subscribers;

    public EmailPriceChangeNotifier(
        ILogger<EmailPriceChangeNotifier> logger,
        IOptions<SendGridConfiguration> sendGridOptions,
        IOptions<SubscriberConfiguration> subscriberOptions
    )
    {
        _logger = logger;
        _sendGridConfiguration = sendGridOptions.Value;
        _subscribers = subscriberOptions.Value.Subscribers;
    }

    public async Task NotifySubscribersAsync(IEnumerable<(PriceHistory current, PriceHistory previous)> priceChanges)
    {
        foreach (var subscriber in _subscribers)
        {
            await SendPriceChangeEmailAsync(subscriber, priceChanges);
        }
    }

    private async Task SendPriceChangeEmailAsync(SubscriberModel subscriber,
        IEnumerable<(PriceHistory current, PriceHistory previous)> priceChanges)
    {
        var client = new SendGridClient(_sendGridConfiguration.ApiKey);

        var sender = new EmailAddress(_sendGridConfiguration.SenderAddress, _sendGridConfiguration.SenderName);
        var recipient = new EmailAddress(subscriber.Email, subscriber.Name);

        // format the template data payload
        var templateData = new PriceChangeEmailTemplateData
        {
            Subject = "Price changes",
            Name = subscriber.Name,
            PriceChanges = priceChanges.Select(pc =>
            {
                var (current, previous) = pc;

                return new PriceChangeTemplateModel
                {
                    CurrentPrice = current.Price,
                    PreviousPrice = previous.Price,
                    TargetName = current.TargetName,
                    TargetPageUrl = current.TargetPageUrl
                };
            }).ToArray()
        };

        // configure the message
        var message = new SendGridMessage
        {
            From = sender,
            TemplateId = _sendGridConfiguration.PriceChangeTemplateId
        };
        message.AddTo(recipient);
        message.SetTemplateData(templateData);

        // send the message
        var response = await client.SendEmailAsync(message).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to send message to <{email}>", subscriber.Email);
        }
        else
        {
            _logger.LogInformation("Sent price change email to <{email}>", subscriber.Email);
        }
    }
}