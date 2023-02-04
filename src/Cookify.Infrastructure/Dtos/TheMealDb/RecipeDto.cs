using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Cookify.Infrastructure.Dtos.TheMealDb;

public record RecipeDto
{
    [JsonPropertyName("strMeal")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("strCategory")]
    public string CategoryName { get; set; } = null!;

    [JsonPropertyName("strArea")]
    public string AreaName { get; set; } = null!;
    
    [JsonPropertyName("strInstructions")]
    public string Instruction { get; set; } = null!;
    
    [JsonPropertyName("strMealThumb")]
    public string ImageLink { get; set; } = null!;
    
    [JsonPropertyName("strYoutube")]
    public string? YoutubeLink { get; set; } = null!;

    #region Ingredients

    [JsonPropertyName("strIngredient1")]
    public string? Ingredient1 { get; set; } 

    [JsonPropertyName("strIngredient2")]
    public string? Ingredient2 { get; set; } 
    
    [JsonPropertyName("strIngredient3")]
    public string? Ingredient3 { get; set; } 
    
    [JsonPropertyName("strIngredient4")]
    public string? Ingredient4 { get; set; } 
    
    [JsonPropertyName("strIngredient5")]
    public string? Ingredient5 { get; set; }
    
    [JsonPropertyName("strIngredient6")]
    public string? Ingredient6 { get; set; } 
    
    [JsonPropertyName("strIngredient7")]
    public string? Ingredient7 { get; set; } 
    
    [JsonPropertyName("strIngredient8")]
    public string? Ingredient8 { get; set; }
    
    [JsonPropertyName("strIngredient9")]
    public string? Ingredient9 { get; set; } 
    
    [JsonPropertyName("strIngredient10")]
    public string? Ingredient10 { get; set; } 

    [JsonPropertyName("strIngredient11")]
    public string? Ingredient11 { get; set; } 
    
    [JsonPropertyName("strIngredient12")]
    public string? Ingredient12 { get; set; } 
    
    [JsonPropertyName("strIngredient13")]
    public string? Ingredient13 { get; set; } 
    
    [JsonPropertyName("strIngredient14")]
    public string? Ingredient14 { get; set; }
    
    [JsonPropertyName("strIngredient15")]
    public string? Ingredient15 { get; set; } 
    
    [JsonPropertyName("strIngredient16")]
    public string? Ingredient16 { get; set; } 
    
    [JsonPropertyName("strIngredient17")]
    public string? Ingredient17 { get; set; } 
    
    [JsonPropertyName("strIngredient18")]
    public string? Ingredient18 { get; set; }
    
    [JsonPropertyName("strIngredient19")]
    public string? Ingredient19 { get; set; } 
    
    [JsonPropertyName("strIngredient20")]
    public string? Ingredient20 { get; set; } 
    
    #endregion

    #region Measures

    [JsonPropertyName("strMeasure1")]
    public string? Measure1 { get; set; } 

    [JsonPropertyName("strMeasure2")]
    public string? Measure2 { get; set; } 
    
    [JsonPropertyName("strMeasure3")]
    public string? Measure3 { get; set; } 
    
    [JsonPropertyName("strMeasure4")]
    public string? Measure4 { get; set; } 
    
    [JsonPropertyName("strMeasure5")]
    public string? Measure5 { get; set; } 
    
    [JsonPropertyName("strMeasure6")]
    public string? Measure6 { get; set; } 
    
    [JsonPropertyName("strMeasure7")]
    public string? Measure7 { get; set; } 
    
    [JsonPropertyName("strMeasure8")]
    public string? Measure8 { get; set; } 
    
    [JsonPropertyName("strMeasure9")]
    public string? Measure9 { get; set; } 
    
    [JsonPropertyName("strMeasure10")]
    public string? Measure10 { get; set; } 

    [JsonPropertyName("strMeasure11")]
    public string? Measure11 { get; set; } 
    
    [JsonPropertyName("strMeasure12")]
    public string? Measure12 { get; set; }
    
    [JsonPropertyName("strMeasure13")]
    public string? Measure13 { get; set; } 
    
    [JsonPropertyName("strMeasure14")]
    public string? Measure14 { get; set; } 
    
    [JsonPropertyName("strMeasure15")]
    public string? Measure15 { get; set; } 
    
    [JsonPropertyName("strMeasure16")]
    public string? Measure16 { get; set; } 
    
    [JsonPropertyName("strMeasure17")]
    public string? Measure17 { get; set; } 
    
    [JsonPropertyName("strMeasure18")]
    public string? Measure18 { get; set; } 
    
    [JsonPropertyName("strMeasure19")]
    public string? Measure19 { get; set; } 
    
    [JsonPropertyName("strMeasure20")]
    public string? Measure20 { get; set; } 
    
    #endregion
}