using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Web;

namespace Suduko.Helpers
{
    public static class ListExtensions
    {
        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> listOfThings)
        {
            listOfThings = listOfThings.OrderBy(i => Guid.NewGuid()).ToList();
        }
    }
}