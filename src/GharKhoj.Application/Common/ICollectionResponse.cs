namespace GharKhoj.Application.Common;

public interface ICollectionResponse<TModel>
{
    List<TModel> Items { get; init; }
}
