public class BookShelfService
{
    private readonly BookShelfRepository _repository;

    public BookShelfService(BookShelfRepository repository)
    {
        _repository = repository;
    }



    public Task<IEnumerable<BookDto>> SearchBookWhichShelfAsync(string keyword)
    {
        return _repository.SearchBookWhichShelfAsync(keyword);
    }
}