using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Newtonsoft.Json;
using RimSynapse.WorldNews.Models;

namespace RimSynapse.WorldNews.Newspaper
{
    public static class SynapseNewspaperGenerator
    {
        public static void Generate(List<string> unpublishedEvents)
        {
            if (unpublishedEvents == null || unpublishedEvents.Count == 0) return;

            string eventsText = string.Join("\n- ", unpublishedEvents);

            string systemPrompt = @"You are a frontier journalist on a lawless rimworld.
You have collected raw events and must synthesize them into a cohesive narrative newspaper issue.
Your writing should be dramatic, colorful, and fit the setting of a harsh sci-fi frontier.

You MUST respond in valid JSON matching this schema:
{
  ""Headline"": ""An overarching dramatic headline for the issue"",
  ""Date"": ""The current date"",
  ""Stories"": [
    {
      ""Title"": ""Title of story 1"",
      ""Content"": ""Rich flavor text for story 1""
    },
    {
      ""Title"": ""Title of story 2"",
      ""Content"": ""Rich flavor text for story 2""
    },
    {
      ""Title"": ""Title of story 3"",
      ""Content"": ""Rich flavor text for story 3""
    }
  ]
}";

            string userMessage = $@"Raw Events:
- {eventsText}

Write the newspaper issue based on these events.";

            SynapseClient.PromptAsync(
                RimSynapseWorldNewsMod.ModHandle,
                systemPrompt,
                userMessage,
                result =>
                {
                    if (result.success)
                    {
                        try
                        {
                            string json = RimSynapse.Utils.JsonHelper.ExtractJson(result.content);
                            if (json != null)
                            {
                                var issue = JsonConvert.DeserializeObject<NewspaperIssue>(json);
                                if (issue != null)
                                {
                                    // Send a letter to the player
                                    Find.LetterStack.ReceiveLetter(
                                        "Newspaper Published: " + issue.Headline,
                                        "A new issue of the local newspaper has been published.\nClick to read.",
                                        LetterDefOf.PositiveEvent,
                                        null, // lookTargets
                                        null, // relatedFaction
                                        null, // quest
                                        null, // hyperlinkThingDefs
                                        issue.Headline // debugInfo
                                    );
                                    
                                    // Normally we would show the dialog immediately or via a custom letter action.
                                    // For now, let's just log it.
                                    RimSynapse.SynapseLogger.Message($"[RimSynapse-WorldNews] Newspaper generated: {issue.Headline}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            RimSynapse.SynapseLogger.Warn("worldnews", $"[RimSynapse-WorldNews] Failed to parse newspaper: {ex.Message}");
                        }
                    }
                },
                new RimSynapse.ChatOptions { priority = 5, requestName = "Newspaper Generation" }
            );
        }
    }
}
