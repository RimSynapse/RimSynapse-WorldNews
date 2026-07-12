using UnityEngine;
using Verse;
using RimSynapse.WorldNews.Models;

namespace RimSynapse.WorldNews.UI
{
    public class Dialog_Newspaper : Window
    {
        private NewspaperIssue issue;

        public override Vector2 InitialSize => new Vector2(800f, 600f);

        public Dialog_Newspaper(NewspaperIssue issue)
        {
            this.issue = issue;
            this.forcePause = true;
            this.doCloseX = true;
            this.closeOnClickedOutside = true;
            this.absorbInputAroundWindow = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            if (issue == null) return;

            Text.Font = GameFont.Medium;
            Rect titleRect = new Rect(inRect.x, inRect.y, inRect.width, 40f);
            Widgets.Label(titleRect, issue.Headline);

            Text.Font = GameFont.Small;
            Rect dateRect = new Rect(inRect.x, titleRect.yMax, inRect.width, 20f);
            Widgets.Label(dateRect, "Date: " + issue.Date);

            // Asymmetric Layout:
            // Left column (33% width) for main story, right column for the rest
            float mainStoryWidth = inRect.width * 0.33f;
            float margin = 10f;
            
            if (issue.Stories.Count > 0)
            {
                Rect mainStoryRect = new Rect(inRect.x, dateRect.yMax + margin, mainStoryWidth, inRect.height - dateRect.yMax - margin);
                DrawStory(mainStoryRect, issue.Stories[0]);

                float rightColumnX = mainStoryRect.xMax + margin;
                float rightColumnWidth = inRect.width - mainStoryWidth - margin;
                
                if (issue.Stories.Count > 1)
                {
                    float halfHeight = (inRect.height - dateRect.yMax - margin - margin) / 2f;
                    Rect topRightRect = new Rect(rightColumnX, dateRect.yMax + margin, rightColumnWidth, halfHeight);
                    DrawStory(topRightRect, issue.Stories[1]);

                    if (issue.Stories.Count > 2)
                    {
                        Rect bottomRightRect = new Rect(rightColumnX, topRightRect.yMax + margin, rightColumnWidth, halfHeight);
                        DrawStory(bottomRightRect, issue.Stories[2]);
                    }
                }
            }
        }

        private void DrawStory(Rect rect, NewspaperStory story)
        {
            Widgets.DrawMenuSection(rect);
            Rect innerRect = rect.ContractedBy(8f);
            
            Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(innerRect.x, innerRect.y, innerRect.width, 30f), story.Title);
            
            Text.Font = GameFont.Small;
            Widgets.Label(new Rect(innerRect.x, innerRect.y + 35f, innerRect.width, innerRect.height - 35f), story.Content);
        }
    }
}
