using static BooksShop.Enum.EnumMessages;

namespace BooksShop.Enum
{
    public class EnumMessages
    {
        public enum UserMessage
        {
            BooksNotFound,
            BookNotFound,
            BookIsFinished,
            IdNotFound,
            Success,
            ErrorDate,
            ErrorOrderyBy,
            ErrorId,
            ErrorCnt
        }
    }
    public static class UserMessageExtensions
    {
        public static string GetMessage(this UserMessage message)
        {
            return message switch
            {
                UserMessage.ErrorId => "Введен некорректный Id",
                UserMessage.ErrorOrderyBy => "Выбран неизвестный тип сортировки, поэтому она будет по умолчанию (Id)",
                UserMessage.BooksNotFound => "Простите, книги отсутствуют",
                UserMessage.ErrorDate => "Неверный формат даты, поэтому она не будет использоваться в фильтре",
                UserMessage.BookNotFound => "Простите, но данная книга не найдена",
                UserMessage.BookIsFinished => "Сожалению, но книги закончились",
                UserMessage.IdNotFound => "Книга с таким идентификатором не найдена",
                UserMessage.Success => "Выполнено!",
                UserMessage.ErrorCnt => "Некорректное значение в поле количества",
                _ => "Простите произошла ошибка"
            };
        }
    }
}
