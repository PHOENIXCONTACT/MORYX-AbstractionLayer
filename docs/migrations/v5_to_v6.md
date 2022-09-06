## Migrate to Abstraction Layer 6

### Interfaces
Several interfaces created for minor confirm updates got merged or removed throughout the update
- `INamedTaskStep` (removed)
- IActivityTracing, ITracing -> Tracing
- IRecipeTemplating -> IRecipe
- IActivityProgress -> Tracing
- IProductTypeEntityRepository -> IProductTypeRepository
- IProductRecipeEntityRepository -> IProductRecipeRepository
