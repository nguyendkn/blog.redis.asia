using System.Text;
using Blog.Service.NotionService.Responses;
using Blog.Service.NotionService.Responses.Components.Properties;

namespace Blog.Service.NotionService;

public static class NotionExtensions
{
    public static string ToHtml(this Page? page, out Dictionary<string,string>? indexes)
    {
        if (page is null)
        {
            indexes = new Dictionary<string,string>();
            return string.Empty;
        }

        indexes = new Dictionary<string, string>();
        var sb = new StringBuilder();
        foreach (var block in page.Results)
        {
            Transform(block, sb, indexes);
        }
        return sb.ToString();
    }
    private static void Transform(Block? block, StringBuilder sb, Dictionary<string, string> indexes)
    {
        if (block is null)
        {
            return;
        }
        switch (block.Type)
        {
            case "heading_1":
                TransformHeading1(block, sb,indexes);
                break;
            case "heading_2":
                TransformHeading2(block, sb, indexes);
                break;
            case "heading_3":
                TransformHeading3(block, sb, indexes);
                break;
            case "paragraph":
                TransformParagraph(block, sb);
                break;
            case "image":
                TransformImage(block, sb);
                break;
            case "bulleted_list_item":
                TransformListItem(block, sb);
                break;
        }
        
    }

    private static void TransformListItem(Block block, StringBuilder sb)
    {
        sb.Append("<div class=\"onfluencer-list-item\" style=\"margin-top: 10px; margin-bottom: 10px;\">").Append("- ")
            .Append(block.BulletedListItem.RichText.First().Text.Content).AppendLine("</div>");
    }
    private static void TransformHeading1(Block block, StringBuilder sb, Dictionary<string, string> indexes)
    {
        indexes.TryAdd(block.Id, block.Heading1.RichText.FirstOrDefault()?.Text.Content ?? string.Empty);
        sb.Append("<h1 class=\"text-2xl\" id=\"" + block.Id + "\">");
        Append(block.Heading1.RichText, sb).AppendLine("</h1>");
        
    }
    private static void TransformHeading2(Block block, StringBuilder sb, Dictionary<string, string> indexes)
    {
        indexes.TryAdd(block.Id, block.Heading2.RichText.FirstOrDefault()?.Text.Content ?? string.Empty);
        sb.Append("<h2 class=\"text-xl\" id=\"" + block.Id + "\">");
        Append(block.Heading2.RichText, sb).AppendLine("</h2>");
    }
    private static void TransformHeading3(Block block, StringBuilder sb, Dictionary<string, string> indexes)
    {
        
        indexes.TryAdd(block.Id, block.Heading3.RichText.FirstOrDefault()?.Text.Content ?? string.Empty);
        sb.Append("<h3 class=\"text-lg\" id=\"" + block.Id + "\">");
        Append(block.Heading3.RichText, sb).AppendLine("</h3>");
    }
    private static void TransformParagraph(Block block, StringBuilder sb)
    {
        sb.Append("<div class=\"onfluencer-paragraph\">");
        Append(block.Paragraph, sb);
        sb.AppendLine("</div>");
    }

    private static void Append(Paragraph block, StringBuilder sb)
    {
        Append(block.RichText, sb);
    }

    private static void TransformImage(Block block, StringBuilder sb)
    {
        switch (block.Image.Type)
        {
            case Image.ExternalType:
                sb.Append("<div class=\"onfluencer-image-block\" style=\"margin-top: 10px; margin-bottom: 10px;\">")
                    .AppendImage(block.Image).AppendLine("</div>");
                break;
            case Image.FileType:
                if (block.Image.File != null)
                    sb.Append("<div class=\"onfluencer-image-block\" style=\"margin-top: 10px; margin-bottom: 10px;\">")
                        .AppendImage(block.Image.File.Url)
                        .AppendLine("</div>");
                break;
        }
    }

    private static StringBuilder AppendImage(this StringBuilder sb, string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl)) return sb;
        sb.Append("<img style=\"width: 100%\"").Append(" src=\"").Append(imageUrl).Append("\"/>");
        return sb;
    }

    private static StringBuilder AppendImage(this StringBuilder sb, Image image)
    {
        var imageUrl = string.Empty;
        if (image.External is not null)
            sb.Append("<img style=\"width: 100%\"").Append(" src=\"").Append(image.External.Url).Append("\"/>");
        else
            sb.Append("<img src=\"").Append(imageUrl).Append("\"/>");

        return sb;
    }

    private static StringBuilder Append(IEnumerable<RichText> data, StringBuilder sb)
    {
        foreach (var line in data)
            Append(line, sb);

        return sb;
    }

    private static void Append(RichText line, StringBuilder sb)
    {
        sb.Append("<div class=\"onfluencer-line\" style=\"margin: 10px 0;\">");
        var tag = line.HasAttribute ? (line.Href != null ? "a" : "span") : null;

        if (tag != null)
        {
            sb.Append('<').Append(tag);

            if (line.Href != null)
                sb.Append(" href=\"").Append(Uri.EscapeDataString(line.Href)).Append('"');

            if (line.HasStyle)
            {
                sb.Append(" class=\"");
                if (line.Annotations.Bold)
                    sb.Append(" onfluencer-bold");
                if (line.Annotations.Italic)
                    sb.Append(" onfluencer-italic");
                if (line.Annotations.Strikethrough)
                    sb.Append(" onfluencer-strikethrough");
                if (line.Annotations.Underline)
                    sb.Append(" onfluencer-underline");
                if (line.Annotations.Color != null)
                    sb.Append(" onfluencer-color-").Append(line.Annotations.Color);
                sb.Append(" onfluencer-code");
                sb.Append('"');
            }

            sb.Append('>');
            switch (line.Type)
            {
                case RichText.TypeText:
                    Append(line.Text, sb);
                    break;
                case RichText.TypeEquation:
                    break;
                case RichText.TypeLink:
                    break;
                case RichText.TypeMention:
                    break;
#if DEBUG
                default:
                    throw new ArgumentException($"Unknown RichText type {line.Type}");
#endif
            }

            sb.Append("</").Append(tag).Append('>');
        }
        else
        {
            Append(line.Text, sb);
        }

        sb.Append("</div>");
    }

    private static void Append(Text text, StringBuilder sb)
    {
        var hasLink = text.Link?.Url != null;
        if (hasLink && text.Link != null)
            sb.Append("<a href=\"").Append(Uri.EscapeDataString(text.Link.Url)).Append("\">");
        sb.Append(text.Content);
        if (hasLink)
            sb.Append("</a>");
    }
}