namespace Microsoft.AspNetCore.Mvc.Rendering
{
    using System.Collections.Generic;
    using System.Linq;
    using YuukoBlog.Models;

    public static class TagHelper
    {
        public static string TagSerialize(this IHtmlHelper self, IEnumerable<PostTag> Tags)
        {
            var ret = Tags.Aggregate("", (current, t) => current + (t.Tag + ", "));
            return ret.TrimEnd(' ').TrimEnd(',');
        }
    }
}