namespace DishesAPI.Models;

public class IngredientDTO
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public Guid DisheId { get; set; }

}
