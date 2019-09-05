using System;
using System.Collections.Generic;

namespace Seq.Input.Octopus
{
    public class OctopusEvent
    {
        public DateTime Timestamp { get; set; }
        public string EventType { get; set; }
        public Payload Payload { get; set; }
    }

    public class Payload
    {
        public string ServerUri { get; set; }
        public string ServerAuditUri { get; set; }
        public DateTime BatchProcessingDate { get; set; }
        public Subscription Subscription { get; set; }
        public Event Event { get; set; }
        public string BatchId { get; set; }
        public int TotalEventsInBatch { get; set; }
        public int EventNumberInBatch { get; set; }
    }

    public class Subscription
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public bool IsDisabled { get; set; }
        public EventNotificationSubscription EventNotificationSubscription { get; set; }
        public string SpaceId { get; set; }
    }

    public class Event
    {
        public string Id { get; set; }
        public List<string> RelatedDocumentIds { get; set; }
        public string Category { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public bool IsService { get; set; }
        public string IdentityEstablishedWith { get; set; }
        public string UserAgent { get; set; }
        public DateTime Occurred { get; set; }
        public string Message { get; set; }
        public string MessageHtml { get; set; }
        public List<MessageReference> MessageReferences { get; set; }
        public object Comments { get; set; }
        public object Details { get; set; }
        public string SpaceId { get; set; }
    }

    public class Filter
    {
        public List<object> Users { get; set; }
        public List<string> Projects { get; set; }
        public List<object> Environments { get; set; }
        public List<string> EventGroups { get; set; }
        public List<string> EventCategories { get; set; }
        public List<object> EventAgents { get; set; }
        public List<object> Tenants { get; set; }
        public List<object> Tags { get; set; }
        public List<object> DocumentTypes { get; set; }
    }

    public class EventNotificationSubscription
    {
        public Filter Filter { get; set; }
        public List<object> EmailTeams { get; set; }
        public string EmailFrequencyPeriod { get; set; }
        public int EmailPriority { get; set; }
        public object EmailDigestLastProcessed { get; set; }
        public object EmailDigestLastProcessedEventAutoId { get; set; }
        public string EmailShowDatesInTimeZoneId { get; set; }
        public string WebhookURI { get; set; }
        public List<object> WebhookTeams { get; set; }
        public string WebhookTimeout { get; set; }
        public object WebhookHeaderKey { get; set; }
        public object WebhookHeaderValue { get; set; }
        public DateTime WebhookLastProcessed { get; set; }
        public int WebhookLastProcessedEventAutoId { get; set; }
    }

    public class MessageReference
    {
        public string ReferencedDocumentId { get; set; }
        public int StartIndex { get; set; }
        public int Length { get; set; }
    }
}
