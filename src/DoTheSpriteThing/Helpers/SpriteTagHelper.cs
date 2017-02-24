using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DoTheSpriteThing.Helpers
{
    [HtmlTargetElement("spriteimg")]
    public class SpriteTagHelper : TagHelper
    {
        public string SpriteImageId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            output.Attributes.Add("id", SpriteImageId);
            output.Attributes.Add("src", "/images/blank.png");
        }
    }
}