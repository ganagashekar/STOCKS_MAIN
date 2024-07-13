using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.MSN
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Category
    {
        public string topic { get; set; }
        public int score { get; set; }
    }

    public class ColorSample
    {
        public bool isDarkMode { get; set; }
        public string hexColor { get; set; }
    }

    public class CommentSummary
    {
        public int totalCount { get; set; }
    }

    public class DarkThemeSVGLogo
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
    }

    public class Feed
    {
        public string id { get; set; }
        public string feedName { get; set; }
    }

    public class FinanceMetadata
    {
        public List<Stock> stocks { get; set; }
        public List<SentimentRating> sentimentRatings { get; set; }
        public List<Category> categories { get; set; }
    }

    public class FocalRegion
    {
        public int x1 { get; set; }
        public int x2 { get; set; }
        public int y1 { get; set; }
        public int y2 { get; set; }
    }

    public class Image
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string caption { get; set; }
        public string source { get; set; }
        public List<ColorSample> colorSamples { get; set; }
        public FocalRegion focalRegion { get; set; }
    }

    public class LightThemeSVGLogo
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
    }

    public class Provider
    {
        public string id { get; set; }
        public string name { get; set; }
        public string logoUrl { get; set; }
        public string profileId { get; set; }
        public LightThemeSVGLogo lightThemeSVGLogo { get; set; }
        public DarkThemeSVGLogo darkThemeSVGLogo { get; set; }
    }

    public class ReactionSummary
    {
        public int totalCount { get; set; }
        public List<SubReactionSummary> subReactionSummaries { get; set; }
    }

    public class ef_finance_trending_growth
    {
        public string nextPageUrl { get; set; }
        public List<SubCard> subCards { get; set; }
    }

    public class SentimentRating
    {
        public string topic { get; set; }
        public int score { get; set; }
    }

    public class Stock
    {
        public string stockId { get; set; }
        public int score { get; set; }
    }

    public class SubCard
    {
        public string type { get; set; }
        public List<SubCard> subCards { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string @abstract { get; set; }
        public int? readTimeMin { get; set; }
        public string url { get; set; }
        public string locale { get; set; }
        public FinanceMetadata financeMetadata { get; set; }
        public DateTime? publishedDateTime { get; set; }
        public bool? isFeatured { get; set; }
        public List<Image> images { get; set; }
        public List<ColorSample> colorSamples { get; set; }
        public Provider provider { get; set; }
        public string category { get; set; }
        public ReactionSummary reactionSummary { get; set; }
        public string reactionStatus { get; set; }
        public CommentSummary commentSummary { get; set; }
        public string commentStatus { get; set; }
        public double? relevanceScore { get; set; }
        public string subscriptionProductType { get; set; }
        public Feed feed { get; set; }
        public List<Topic> topics { get; set; }
        public bool? isWorkNewsContent { get; set; }
        public string recoId { get; set; }
        public string source { get; set; }
        public bool? isLocalContent { get; set; }
    }

    public class SubReactionSummary
    {
        public int totalCount { get; set; }
        public string type { get; set; }
    }

    public class Topic
    {
        public string label { get; set; }
        public double weight { get; set; }
        public string feedId { get; set; }
        public string locale { get; set; }
    }




}
