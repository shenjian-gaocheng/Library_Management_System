public class BookService
{
    private readonly BookRepository _repository;

    public BookService(BookRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<BookDetailDto>> SearchBooksAsync(string keyword)
    {
        return _repository.SearchBooksAsync(keyword);
    }
}